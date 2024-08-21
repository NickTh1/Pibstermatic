using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using Pibstermatic.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static WaveMix.EnginePlayer;

namespace WaveMix
{
    public partial class SCLEditor : Form
    {
        static float SnapRadius = 10.0f;
        static readonly float c_SliderQuantScale = 1e6f;
        static readonly int MaxNumPointsPerSample = 8;

        enum EPitchMode
        {
            Proportional,
            Manual,
        }

        [Flags]
        enum ESampleMinMaxMask
        {
            None = 0,
            Min = 1,
            Max = 2,
            Both = Min | Max,
        }

        class EnvelopePoint
        {
            public float m_RPM;
            public float m_Volume;

            public EnvelopePoint(float rpm, float volume)
            {
                m_RPM = rpm;
                m_Volume = volume;
            }

            public EnvelopePoint Clone()
            {
                return new EnvelopePoint(m_RPM, m_Volume);
            }
        }

        class Sample
        {
            public string m_Wav = "";

            public float m_ReferenceRPM;
            public float m_MinPitch;
            public float m_MaxPitch;

            public EPitchMode m_PitchMode;

            public bool m_AutoMinPitch;

            public List<EnvelopePoint> m_Points = new List<EnvelopePoint>();
        }

        class Layer
        {
            public List<Sample> m_Samples = new List<Sample>();
        }

        enum EDragType
        {
            Both,
            X,
            Y
        }

        string m_EnginePath;

        PlotModel m_PlotModel = new PlotModel();
        LineAnnotation m_AnnotationRPM = new LineAnnotation();
        LineAnnotation m_AnnotationDrag = new LineAnnotation();

        float m_CurrentRPM = 1000;

        Stopwatch m_TimerStopwatch = new Stopwatch();

        // 
        // State
        //
        StructuredProperties m_StructuredProps;
        float m_MaxRPM;
        List<Layer> m_Layers = new List<Layer>();
        int m_IndexLayer = -1;
        int m_IndexSample = -1;

        int m_Modifying = 1;

        LineSeries? m_CurrentLineSeries = null;
        int m_DraggingPoint = -1;
        float m_DragStartX;
        float m_DragStartY;
        List<EnvelopePoint> m_PointsForCancel = new List<EnvelopePoint>();
        MouseEventArgs m_LastMouseEventArgs = new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0);

        ContextMenuStrip m_ContextMenuStrip = new ContextMenuStrip();

        double m_TimeLastUpdateProps;
        bool m_PendingUpdateProps = false;

        float m_CurrentSampleVolume = 0;

        public event EventHandler? OnPropertyChanged;
        public event EventHandler? OnSelectedSampleChanged;
        public event EventHandler? OnSave;

        public SCLEditor(string engine_path, string engine_props_text)
        {
            m_EnginePath = engine_path;
            InitializeComponent();
            this.Text = engine_path;

            this.KeyPreview = true;

            m_StructuredProps = PropsFile.ParsePropsFile(engine_props_text);
            ParseProps(m_StructuredProps);

            {
                m_PlotModel.TextColor = OxyColors.White;
                m_PlotModel.TitleColor = OxyColors.White;
                m_PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, AxislineColor = OxyColors.White });
                m_PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, AxislineColor = OxyColors.White });

                m_PlotModel.Axes[0].Minimum = 0;
                m_PlotModel.Axes[0].Maximum = m_MaxRPM;
                m_PlotModel.Axes[1].Minimum = 0.0f;
                m_PlotModel.Axes[1].Maximum = 4.0f;

                m_PlotModel.IsLegendVisible = false;

                m_AnnotationRPM.Type = LineAnnotationType.Vertical;
                m_AnnotationDrag.Type = LineAnnotationType.Vertical;
                m_AnnotationDrag.Color = OxyColors.Gray;
                m_PlotModel.Annotations.Add(m_AnnotationRPM);

                plotViewEnvelope.Model = m_PlotModel;
            }

            {
                m_ContextMenuStrip.Opening += ContextMenuStrip_Opening;
                plotViewEnvelope.ContextMenuStrip = m_ContextMenuStrip;
            }

            IndexLayer = 0;

            UpdateSamples();
            IndexSample = 0;

            UpdatePlot();

            m_Modifying = 0;
            m_TimerStopwatch.Start();
        }

        static ToolStripItem CreateContextMenuItem(string text, EventHandler handler)
        {
            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Text = text;
            item.Click += handler;
            return item;
        }

        private void ContextMenuStrip_Opening(object? sender, CancelEventArgs e)
        {
            Control c = m_ContextMenuStrip.SourceControl as Control;

            e.Cancel = true;

            m_ContextMenuStrip.Items.Clear();

            MouseEventArgs mouse_event_args = m_LastMouseEventArgs;

            Sample sample = CurrentSample;

            int index_point = IdentifyPoint(mouse_event_args);
            if (index_point >= 0)
            {
                if (sample.m_Points.Count <= 2)
                    return;
                m_ContextMenuStrip.Items.Add(CreateContextMenuItem("Remove", (o, e) =>
                {
                    sample.m_Points.RemoveAt(index_point);
                    UpdatePlot();
                    UpdateSample();
                    UpdateProps();
                }));
            }
            else
            {
                if (sample.m_Points.Count >= MaxNumPointsPerSample)
                    m_ContextMenuStrip.Items.Add("No more add buttons for you, sir");
                else
                    m_ContextMenuStrip.Items.Add(CreateContextMenuItem("Add", (o, e) =>
                {
                    float rpm = (float)m_PlotModel.Axes[0].InverseTransform(mouse_event_args.X);
                    float volume = (float)m_PlotModel.Axes[1].InverseTransform(mouse_event_args.Y);
                    volume = Math.Max(volume, 0);
                    rpm = Math.Clamp(rpm, 0, m_MaxRPM);
                    EnvelopePoint pt = new EnvelopePoint(rpm, volume);

                    int dst_index = 0;
                    for (int i = 0; i < sample.m_Points.Count; i++)
                    {
                        if (sample.m_Points[i].m_RPM > rpm)
                            break;
                        dst_index = i + 1;
                    }

                    sample.m_Points.Insert(dst_index, pt);

                    UpdatePlot();
                    UpdateSample();
                    UpdateProps();
                }));
            }

            e.Cancel = false;
        }

        public float RPM
        {
            get { return m_CurrentRPM; }
            set
            {
                m_CurrentRPM = value;
                m_AnnotationRPM.X = value;
                m_PlotModel.InvalidatePlot(true);
            }
        }

        public StructuredProperties EngineProperties
        {
            get
            {
                return m_StructuredProps;
            }
        }

        int IndexLayer
        {
            get { return m_IndexLayer; }
            set
            {
                if (m_IndexLayer == value)
                    return;
                m_IndexLayer = value;
                using (Modifying modifying = Modify())
                {
                    comboBoxLayer.SelectedIndex = value;
                }
                RaiseOnSelectedSampleChanged();
            }
        }

        int IndexSample
        {
            get
            {
                return m_IndexSample;
            }
            set
            {
                if (m_IndexSample == value)
                    return;
                m_IndexSample = value;
                listBoxSamples.SelectedIndex = value;
                UpdateButtons();
                UpdateSample();

                RaiseOnSelectedSampleChanged();
            }
        }

        public int GetIndexLayer()
        {
            return IndexLayer;
        }

        public int GetIndexSample()
        {
            return IndexSample;
        }

        Layer CurrentLayer
        {
            get
            {
                return m_Layers[IndexLayer];
            }
        }

        Sample CurrentSample
        {
            get
            {
                Layer layer = CurrentLayer;
                return layer.m_Samples[IndexSample];
            }
        }

        StructuredProperties CurrentLayerProps
        {
            get
            {
                return m_StructuredProps.GetChild("Layer" + IndexLayer);
            }
        }

        void UpdateCurrentLineSeries()
        {
            if (m_CurrentLineSeries == null)
                return;
            float is_playing = Math.Clamp(m_CurrentSampleVolume * 5, 0, 1);
            OxyColor color_active = OxyColor.FromRgb(0xff, 0xe0, 0xff);
            m_CurrentLineSeries.Color = Utils.Lerp(is_playing, OxyColors.White, color_active);
            m_CurrentLineSeries.StrokeThickness = 2 + is_playing;
        }

        public void SetCurrentSampleState(float volume)
        {
            if (volume == m_CurrentSampleVolume)
                return;
            m_CurrentSampleVolume = volume;

            textBoxVolume.Text = volume.ToString("0.00", CultureInfo.InvariantCulture);

            Color color_active = Color.FromArgb(0xff, 0xff, 0xe0, 0xff);
            textBoxVolume.BackColor = (volume > 0) ? color_active : this.BackColor;

            UpdateCurrentLineSeries();
            m_PlotModel.InvalidatePlot(true);
        }

        public void SetSampleAutoMinPitch(int index_layer, int index_sample_in_layer, bool auto_min_pitch)
        {
            int key = (index_layer << 16) + index_sample_in_layer;
            Layer layer = m_Layers[index_layer];
            Sample sample = layer.m_Samples[index_sample_in_layer];
            if (sample.m_AutoMinPitch == auto_min_pitch)
                return;
            sample.m_AutoMinPitch = auto_min_pitch;
            if (index_layer == IndexLayer && index_sample_in_layer == IndexSample)
                UpdateSample();
        }

        void ParseProps(StructuredProperties struct_props)
        {
            m_MaxRPM = struct_props.GetFloat("MaxValue");
            List<Layer> layers = new List<Layer>();
            for (int index_layer = 0; index_layer < 2; index_layer++)
            {
                StructuredProperties layer_props = struct_props.GetChild("Layer" + index_layer);
                Layer layer = new Layer();

                int num_samples = layer_props.GetInteger("NumSamples");
                for (int j = 0; j < num_samples; j++)
                {
                    StructuredProperties sample_props = layer_props.GetChild("Sample" + j);
                    Sample sample = new Sample();
                    sample.m_Wav = sample_props.GetString("Wav");

                    sample.m_MinPitch = sample_props.GetFloat("MinPitch");
                    sample.m_MaxPitch = sample_props.GetFloat("MaxPitch");

                    int num_points = sample_props.GetInteger("NumPoints");
                    for (int k = 0; k < num_points; k++)
                    {
                        StructuredProperties pt_props = sample_props.GetChild("Point" + k);
                        float val = pt_props.GetFloat("Value");
                        float vol = pt_props.GetFloat("Volume");
                        float rpm = (float)Math.Round(val * m_MaxRPM);
                        EnvelopePoint pt = new EnvelopePoint(rpm, vol);
                        sample.m_Points.Add(pt);
                    }

                    sample.m_ReferenceRPM = m_MaxRPM;
                    sample.m_PitchMode = EPitchMode.Manual;
                    if (num_points > 0)
                    {
                        sample.m_ReferenceRPM = sample.m_Points[sample.m_Points.Count - 1].m_RPM / sample.m_MaxPitch;

                        float min_pitch = sample.m_Points[0].m_RPM / sample.m_ReferenceRPM;
                        if (Math.Abs(min_pitch - sample.m_MinPitch) < 1e-2f)
                            sample.m_PitchMode = EPitchMode.Proportional;
                    }

                    layer.m_Samples.Add(sample);
                }
                layers.Add(layer);
            }
            m_Layers = layers;
        }

        public void UpdateFromText(string engine_props_text)
        {
            try
            {
                StructuredProperties struct_props = PropsFile.ParsePropsFile(engine_props_text);
                ParseProps(struct_props);

                if (m_IndexSample > CurrentLayer.m_Samples.Count)
                    IndexSample = CurrentLayer.m_Samples.Count - 1;

                UpdateSamples();
                UpdateSample();
                UpdatePlot();
            }
            catch (Exception)
            {
            }
        }

        void RaiseOnSelectedSampleChanged()
        {
            EventHandler? handler = OnSelectedSampleChanged;

            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        void RaiseOnPropertyChanged()
        {
            EventHandler? handler = OnPropertyChanged;

            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        static void SortProps(StructuredProperties props, Func<string, string, int> predicate )
        {
            int num_props = props.Count;
            for( int i = 1; i < num_props; i++)
            {
                string key_i = props.GetKeyedValue(i).m_Key;
                for(int j = i; --j >= 0;)
                {
                    string key_j = props.GetKeyedValue(j).m_Key;
                    if (predicate(key_j, key_i) >= 0)
                        break;
                    props.SwapEntries(key_i, key_j, false);
                }
            }
        }

        static int SampleKeyOrder(string key)
        {
            try
            {
                if (key.StartsWith("Point"))
                    return 1000 + int.Parse(key.Substring(5));  // 5 = length("Point")
                if (key.Equals("NumPoints"))
                    return 100;
                return 0;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        static int SampleKeyCompare(string key1, string key2)
        {
            int op1 = SampleKeyOrder(key1);
            int op2 = SampleKeyOrder(key2);
            return op2 - op1;
        }

        void UpdateProps()
        {
            for (int index_layer = 0; index_layer < 2; index_layer++)
            {
                Layer layer = m_Layers[index_layer];
                StructuredProperties layer_props = m_StructuredProps.GetChild("Layer" + index_layer);
                int num_samples = layer.m_Samples.Count;
                int num_samples_props = layer_props.GetInteger("NumSamples");
                layer_props.SetInteger("NumSamples", num_samples);
                for (int i = num_samples; i < num_samples_props; i++)
                    layer_props.RemoveChild("Sample" + i);

                for (int index_sample = 0; index_sample < num_samples; index_sample++)
                {
                    Sample sample = layer.m_Samples[index_sample];
                    StructuredProperties sample_props = layer_props.GetOrAddChild("Sample" + index_sample);
                    sample_props.SetString("Wav", sample.m_Wav);

                    int num_points = sample.m_Points.Count;
                    int num_points_props = sample_props.GetInteger("NumPoints");
                    sample_props.SetInteger("NumPoints", num_points);
                    for (int j = num_points; j < num_points_props; j++)
                        sample_props.RemoveChild("Point" + j);

                    if (sample.m_PitchMode == EPitchMode.Manual || sample.m_Points.Count == 0)
                    {
                        sample_props.SetFloat("MaxPitch", sample.m_MaxPitch);
                        sample_props.SetFloat("MinPitch", sample.m_MinPitch);
                    }
                    else
                    {
                        EnvelopePoint pt_last = sample.m_Points[sample.m_Points.Count - 1];
                        EnvelopePoint pt_first = sample.m_Points[0];

                        sample_props.SetFloat("MaxPitch", pt_last.m_RPM / sample.m_ReferenceRPM);
                        sample_props.SetFloat("MinPitch", pt_first.m_RPM / sample.m_ReferenceRPM);
                    }

                    for (int index_point = 0; index_point < num_points; index_point++)
                    {
                        EnvelopePoint pt = sample.m_Points[index_point];
                        StructuredProperties pt_props = sample_props.GetOrAddChild("Point" + index_point);

                        pt_props.SetFloat("Value", pt.m_RPM / m_MaxRPM);
                        pt_props.SetFloat("Volume", pt.m_Volume);
                    }

                    SortProps(sample_props, SampleKeyCompare);
                }
            }
            RaiseOnPropertyChanged();
            m_TimeLastUpdateProps = m_TimerStopwatch.Elapsed.TotalSeconds;
            m_PendingUpdateProps = false;
            timer.Start();
        }

        void UpdatePropsSoon()
        {
            double dt = m_TimerStopwatch.Elapsed.TotalSeconds - m_TimeLastUpdateProps;
            if (dt < 16e-3)
                m_PendingUpdateProps = true;
            else
                UpdateProps();
        }

        void UpdateButtons()
        {
            Layer layer = CurrentLayer;

            buttonRemove.Enabled = (layer.m_Samples.Count > 1);     // Don't remove the last sample.

            buttonMoveUp.Enabled = (IndexSample > 0);
            buttonMoveDown.Enabled = (IndexSample < layer.m_Samples.Count - 1);
        }

        void UpdateSamples()
        {
            using (Modifying modifying = Modify())
            {
                listBoxSamples.Items.Clear();

                Layer layer = CurrentLayer;

                for (int i = 0; i < layer.m_Samples.Count; i++)
                {
                    Sample sample = layer.m_Samples[i];
                    string wav_name = Path.GetFileNameWithoutExtension(sample.m_Wav);
                    listBoxSamples.Items.Add(wav_name);
                }

                if (IndexSample >= 0)
                    listBoxSamples.SelectedIndex = IndexSample;

                UpdateButtons();
            }
        }

        void UpdateLineSeries(LineSeries line_series, int index_sample)
        {
            Layer layer = CurrentLayer;
            Sample sample = layer.m_Samples[index_sample];

            line_series.Points.Clear();
            bool is_current = (index_sample == IndexSample);
            line_series.Color = is_current ? OxyColors.White : OxyColor.FromRgb(0x40, 0x40, 0x40);
            line_series.MarkerType = MarkerType.Circle;
            line_series.MarkerSize = 3;
            line_series.CanTrackerInterpolatePoints = false;

            for (int j = 0; j < sample.m_Points.Count; j++)
            {
                EnvelopePoint pt = sample.m_Points[j];
                line_series.Points.Add(new DataPoint(pt.m_RPM, pt.m_Volume));
            }
        }

        void UpdatePlot()
        {
            m_PlotModel.Series.Clear();
            Layer layer = CurrentLayer;

            for (int j = 0; j < 2; j++)     // Twice, to guarantee that the current sample is drawn on top (and not obscured)
            {
                for (int i = 0; i < layer.m_Samples.Count; i++)
                {
                    bool is_current = (i == IndexSample);
                    if (is_current != (j != 0))
                        continue;

                    Sample sample = layer.m_Samples[i];
                    LineSeries line_series = new LineSeries();
                    line_series.TrackerFormatString = "RPM: {2:0}\nVolume: {4:0.000}";
                    UpdateLineSeries(line_series, i);

                    if (is_current)
                    {
                        m_CurrentLineSeries = line_series;
                        UpdateCurrentLineSeries();
                    }

                    m_PlotModel.Series.Add(line_series);
                }
            }
            m_PlotModel.InvalidatePlot(true);
        }

        void UpdateSampleMinMax(ESampleMinMaxMask mask = ESampleMinMaxMask.Both)
        {
            Sample sample = CurrentSample;

            using (Modifying modifying = Modify())
            {
                float max_pitch = sample.m_MaxPitch;
                float min_pitch = sample.m_MinPitch;

                if (sample.m_Points.Count > 0)
                {
                    EnvelopePoint pt_last = sample.m_Points[sample.m_Points.Count - 1];
                    EnvelopePoint pt_first = sample.m_Points[0];

                    if (sample.m_PitchMode == EPitchMode.Proportional)
                    {
                        max_pitch = pt_last.m_RPM / sample.m_ReferenceRPM;
                        min_pitch = pt_first.m_RPM / sample.m_ReferenceRPM;
                    }
                    else if (sample.m_AutoMinPitch)
                        min_pitch = pt_first.m_RPM * max_pitch / pt_last.m_RPM;
                }
                if ((mask & ESampleMinMaxMask.Min) != ESampleMinMaxMask.None)
                    textBoxMinPitch.Text = min_pitch.ToString("0.000", CultureInfo.InvariantCulture);
                if ((mask & ESampleMinMaxMask.Max) != ESampleMinMaxMask.None)
                    textBoxMaxPitch.Text = max_pitch.ToString("0.000", CultureInfo.InvariantCulture);
            }
        }

        void UpdateSample()
        {
            Sample sample = CurrentSample;
            using (Modifying modifying = Modify())
            {
                textBoxWav.Text = sample.m_Wav;

                EPitchMode pitch_mode = sample.m_PitchMode;
                radioButtonProportional.Checked = (pitch_mode == EPitchMode.Proportional);
                radioButtonManual.Checked = (pitch_mode == EPitchMode.Manual);

                textBoxReference.Enabled = (pitch_mode == EPitchMode.Proportional);
                trackBarReference.Enabled = (pitch_mode == EPitchMode.Proportional);
                textBoxMinPitch.Enabled = (pitch_mode == EPitchMode.Manual) && !sample.m_AutoMinPitch;
                textBoxMaxPitch.Enabled = (pitch_mode == EPitchMode.Manual);

                textBoxReference.Text = sample.m_ReferenceRPM.ToString("0.", CultureInfo.InvariantCulture);
                trackBarReference.Value = (int)Math.Round(sample.m_ReferenceRPM * c_SliderQuantScale / m_MaxRPM);
                UpdateSampleMinMax();
            }
        }

        bool IsModifying
        {
            get { return m_Modifying > 0; }
        }

        Modifying Modify()
        {
            m_Modifying++;
            return new Modifying(this);
        }

        class Modifying : IDisposable
        {
            SCLEditor m_Owner;

            internal Modifying(SCLEditor owner)
            {
                m_Owner = owner;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                m_Owner.m_Modifying--;
            }
        }

        private void listBoxSamples_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsModifying)
                return;
            this.IndexSample = listBoxSamples.SelectedIndex;
            UpdateButtons();
            UpdatePlot();
        }

        int IdentifyPoint(MouseEventArgs e)
        {
            int index_sample = IndexSample;
            Layer layer = CurrentLayer;
            if (index_sample < 0 || index_sample >= layer.m_Samples.Count)
                return -1;
            Sample sample = layer.m_Samples[index_sample];

            double sq_dist_nearest = 1e20f;
            int index_nearest = -1;
            for (int i = 0; i < sample.m_Points.Count; i++)
            {
                EnvelopePoint pt = sample.m_Points[i];
                double x = m_PlotModel.Axes[0].Transform(pt.m_RPM);
                double y = m_PlotModel.Axes[1].Transform(pt.m_Volume);
                double dx = e.X - x;
                double dy = e.Y - y;
                double sq_dist = dx * dx + dy * dy;
                if (sq_dist < sq_dist_nearest)
                {
                    sq_dist_nearest = sq_dist;
                    index_nearest = i;
                }
            }
            if (index_nearest < 0)
                return -1;
            float dist_nearest = (float)Math.Sqrt(sq_dist_nearest);
            if (dist_nearest < SnapRadius)
                return index_nearest;
            return -1;
        }

        EDragType DragType
        {
            get
            {
                if ((ModifierKeys & Keys.Control) != Keys.None)
                    return EDragType.Y;
                else if ((ModifierKeys & Keys.Shift) != Keys.None)
                    return EDragType.X;
                return EDragType.Both;
            }
        }

        bool ShouldSnap
        {
            get
            {
                return (ModifierKeys & Keys.Alt) == Keys.None;
            }
        }

        bool IsDragging
        {
            get { return m_DraggingPoint >= 0; }
        }

        void UpdateCursor(MouseEventArgs e)
        {
            Cursor curs = System.Windows.Forms.Cursors.Default;
            EDragType drag_type = DragType;
            if (m_DraggingPoint >= 0)
            {
                curs = System.Windows.Forms.Cursors.Cross;
                if (drag_type == EDragType.X)
                    curs = System.Windows.Forms.Cursors.VSplit;
                else if (drag_type == EDragType.Y)
                    curs = System.Windows.Forms.Cursors.HSplit;
            }
            else
            {
                if (e.Button == MouseButtons.None)
                {
                    int index_point = IdentifyPoint(e);
                    if (index_point >= 0)
                    {
                        curs = System.Windows.Forms.Cursors.SizeAll;
                        if (drag_type == EDragType.Y)
                            curs = System.Windows.Forms.Cursors.SizeNS;
                        else if (drag_type == EDragType.X)
                            curs = System.Windows.Forms.Cursors.SizeWE;
                    }
                }
            }
            Cursor = curs;
        }

        void CancelDrag()
        {
            Sample sample = CurrentSample;
            sample.m_Points = m_PointsForCancel;
            m_PointsForCancel.Clear();

            m_DraggingPoint = -1;
            Capture = false;
            UpdateCursor(m_LastMouseEventArgs);
            UpdateProps();
        }

        private void plotViewEnvelope_MouseDown(object sender, MouseEventArgs e)
        {
            m_LastMouseEventArgs = e;

            if (m_DraggingPoint > 0)
            {
                CancelDrag();
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                int index_point = IdentifyPoint(e);

                if (index_point < 0)
                    return;
                m_DraggingPoint = index_point;

                Sample sample = CurrentSample;
                EnvelopePoint pt = sample.m_Points[index_point];
                m_DragStartX = pt.m_RPM;
                m_DragStartY = pt.m_Volume;
                m_PointsForCancel.Clear();
                for (int i = 0; i < sample.m_Points.Count; i++)
                    m_PointsForCancel.Add(sample.m_Points[i].Clone());

                Capture = true;
            }
            UpdateCursor(e);
        }

        float SnapRPM(float rpm)
        {
            Layer layer = CurrentLayer;
            float diff_nearest = 1e9f;
            float rpm_nearest = 0;

            for (int i = 0; i < layer.m_Samples.Count; i++)
            {
                Sample sample = layer.m_Samples[i];
                for (int j = 0; j < sample.m_Points.Count; j++)
                {
                    EnvelopePoint point = sample.m_Points[j];
                    float diff = Math.Abs(point.m_RPM - rpm);
                    if (diff < diff_nearest)
                    {
                        diff_nearest = diff;
                        rpm_nearest = point.m_RPM;
                    }
                }
            }
            if (diff_nearest < 100)
                return rpm_nearest;
            return (float)Math.Round(rpm / 100.0f) * 100.0f;
        }

        void UpdateDragAnnotation()
        {
            if (m_PlotModel.Annotations.Count > 1)
                m_PlotModel.Annotations.RemoveAt(1);

            if (m_DraggingPoint >= 0)
            {
                m_PlotModel.Annotations.Add(m_AnnotationDrag);

                Sample sample = CurrentSample;
                EnvelopePoint pt = sample.m_Points[m_DraggingPoint];
                m_AnnotationDrag.X = pt.m_RPM;
            }
            m_PlotModel.InvalidatePlot(true);
        }

        bool DragTo(MouseEventArgs e)
        {
            EDragType drag_type = DragType;
            float x_drag = (float)m_PlotModel.Axes[0].InverseTransform(e.X);
            float y_drag = (float)m_PlotModel.Axes[1].InverseTransform(e.Y);

            if (ShouldSnap)
            {
                x_drag = SnapRPM(x_drag);
                y_drag = (float)Math.Round(y_drag * 10.0f) / 10.0f;
            }


            float x = (drag_type != EDragType.Y) ? x_drag : m_DragStartX;
            float y = (drag_type != EDragType.X) ? y_drag : m_DragStartY;
            x = Math.Clamp(x, 0, m_MaxRPM);
            y = Math.Max(y, 0);

            Sample sample = CurrentSample;
            EnvelopePoint pt = sample.m_Points[m_DraggingPoint];
            if (pt.m_RPM == x && pt.m_Volume == y)
                return false;

            for (int i = 0; i < sample.m_Points.Count; i++)
            {
                if (i == m_DraggingPoint)
                    continue;
                EnvelopePoint other_pt = sample.m_Points[i];
                if (i < m_DraggingPoint && other_pt.m_RPM > x)
                    other_pt.m_RPM = x;
                if (i > m_DraggingPoint && other_pt.m_RPM < x)
                    other_pt.m_RPM = x;
            }

            pt.m_RPM = x;
            pt.m_Volume = y;

            if (m_CurrentLineSeries != null)
                UpdateLineSeries(m_CurrentLineSeries, IndexSample);

            UpdateDragAnnotation();
            m_PlotModel.InvalidatePlot(true);

            UpdatePropsSoon();
            return true;
        }

        private void plotViewEnvelope_MouseMove(object sender, MouseEventArgs e)
        {
            m_LastMouseEventArgs = e;

            if (IsDragging)
                DragTo(e);
            else
            {
                Cursor curs = System.Windows.Forms.Cursors.Default;
                if (e.Button == MouseButtons.None)
                {
                    int index_point = IdentifyPoint(e);
                    if (index_point >= 0)
                    {
                        curs = System.Windows.Forms.Cursors.SizeAll;
                        if ((ModifierKeys & Keys.Control) != Keys.None)
                            curs = System.Windows.Forms.Cursors.SizeNS;
                        else if ((ModifierKeys & Keys.Alt) != Keys.None)
                            curs = System.Windows.Forms.Cursors.SizeWE;
                    }
                }
                Cursor = curs;
            }
        }

        private void plotViewEnvelope_MouseUp(object sender, MouseEventArgs e)
        {
            m_LastMouseEventArgs = e;
            if (m_DraggingPoint < 0)
                return;
            Capture = false;
            Cursor = System.Windows.Forms.Cursors.Default;
            m_DraggingPoint = -1;
            UpdateCursor(e);
            UpdateDragAnnotation();
        }

        private void plotViewEnvelope_KeyDown(object sender, KeyEventArgs e)
        {
            if (m_DraggingPoint >= 0 && e.KeyCode == Keys.Escape)
                CancelDrag();

            UpdateCursor(m_LastMouseEventArgs);
            if (IsDragging)
                DragTo(m_LastMouseEventArgs);
        }

        private void plotViewEnvelope_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateCursor(m_LastMouseEventArgs);
            if (IsDragging)
                DragTo(m_LastMouseEventArgs);
        }

        private void SCLEditor_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateCursor(m_LastMouseEventArgs);
            if (IsDragging)
                DragTo(m_LastMouseEventArgs);
        }

        private void SCLEditor_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateCursor(m_LastMouseEventArgs);
            if (IsDragging)
                DragTo(m_LastMouseEventArgs);
        }

        void SwapPosition(int index_sample1, int index_sample2)
        {
            {
                Layer layer = CurrentLayer;
                {
                    Sample tmp = layer.m_Samples[index_sample1];
                    layer.m_Samples[index_sample1] = layer.m_Samples[index_sample2];
                    layer.m_Samples[index_sample2] = tmp;
                }

                if (index_sample1 == IndexSample)
                    IndexSample = index_sample2;
                else if (index_sample2 == IndexSample)
                    IndexSample = index_sample1;
            }
            {
                StructuredProperties layer_props = CurrentLayerProps;
                layer_props.SwapEntries("Sample" + index_sample1, "Sample" + index_sample2, true);
            }

            UpdateSamples();
        }

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            SwapPosition(IndexSample, IndexSample - 1);
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            SwapPosition(IndexSample, IndexSample + 1);
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            int index_sample = IndexSample;
            Layer layer = CurrentLayer;
            {
                layer.m_Samples.RemoveAt(index_sample);
            }
            {
                // Keep data and props instances in sync.
                StructuredProperties layer_props = CurrentLayerProps;
                layer_props.RemoveChild("Sample" + index_sample);

                for (int i = index_sample + 1; i <= layer.m_Samples.Count; i++)
                    layer_props.Rename("Sample" + i, "Sample" + (i - 1));
            }
            if (index_sample >= layer.m_Samples.Count)
                IndexSample = index_sample - 1;

            UpdateSamples();
            UpdatePlot();
            UpdateProps();
        }

        string? PickWavFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Wav files (*.wav)|*.wav";

            string? folder = Path.GetDirectoryName(m_EnginePath);
            if (folder != null)
                dlg.InitialDirectory = folder;

            DialogResult result = dlg.ShowDialog();
            if (result != DialogResult.OK)
                return null;

            string path = dlg.FileName;
            string? chosen_folder = Path.GetDirectoryName(path);
            if (chosen_folder != null && folder != null && !String.Equals(folder, chosen_folder))
            {
                MessageBox.Show("Wav file must reside in the same folder as the scl file");
                return null;
            }

            return Path.GetFileName(path);
        }

        void AddSampleAt(int dst_index)
        {
            string? wav_file = PickWavFile();
            if (wav_file == null)
                return;

            Layer layer = CurrentLayer;
            int num_samples_before = layer.m_Samples.Count;

            {
                float rpm = this.RPM;

                Sample sample = new Sample();
                sample.m_Wav = wav_file;
                sample.m_ReferenceRPM = rpm;
                sample.m_PitchMode = EPitchMode.Proportional;

                float rpm0 = rpm - 2000;
                float rpm1 = rpm - 1000;
                float rpm2 = rpm + 1000;
                float rpm3 = rpm + 2000;
                if (rpm0 < 0)
                {
                    rpm1 = rpm * (2.0f / 3.0f);
                    rpm0 = rpm * (1.0f / 3.0f);
                }
                if (rpm3 > m_MaxRPM)
                {
                    rpm2 = rpm + (m_MaxRPM - rpm) * (1.0f / 3.0f);
                    rpm3 = rpm + (m_MaxRPM - rpm) * (2.0f / 3.0f);
                }
                sample.m_MaxPitch = rpm3 / rpm;
                sample.m_MinPitch = rpm0 / rpm;

                sample.m_Points.Add(new EnvelopePoint(rpm0, 0));
                sample.m_Points.Add(new EnvelopePoint(rpm1, 1));
                sample.m_Points.Add(new EnvelopePoint(rpm2, 1));
                sample.m_Points.Add(new EnvelopePoint(rpm3, 0));

                layer.m_Samples.Insert(dst_index, sample);
            }
            {
                StructuredProperties layer_props = CurrentLayerProps;
                // Add a placeholder - last.
                StructuredProperties sample_props = layer_props.AddChild("Sample" + num_samples_before);
                sample_props.SetInteger("NumPoints", 0);

                // Then move it up to the desired position
                for (int i = num_samples_before; i >= dst_index; i--)
                    layer_props.SwapEntries("Sample" + i, "Sample" + (i + 1), true);
            }

            UpdateSamples();
            UpdatePlot();
            UpdateProps();

            IndexSample = dst_index;
        }

        private void buttonAddAbove_Click(object sender, EventArgs e)
        {
            AddSampleAt(IndexSample);
        }

        private void buttonAddBelow_Click(object sender, EventArgs e)
        {
            AddSampleAt(IndexSample + 1);
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            string? filename = PickWavFile();
            if (filename == null)
                return;
            textBoxWav.Text = filename;
        }

        private void textBoxWav_TextChanged(object sender, EventArgs e)
        {
            if (IsModifying)
                return;
            Sample sample = CurrentSample;
            sample.m_Wav = textBoxWav.Text;

            UpdatePropsSoon();
        }

        private void textBoxReference_TextChanged(object sender, EventArgs e)
        {
            if (IsModifying)
                return;

            try
            {
                float ref_rpm = float.Parse(textBoxReference.Text);
                if (ref_rpm <= 0)
                    return;

                Sample sample = CurrentSample;
                sample.m_ReferenceRPM = ref_rpm;

                UpdateSampleMinMax();
                UpdatePropsSoon();
            }
            catch (Exception)
            {
            }
        }

        void UpdatePitchModeFromUI()
        {
            Sample sample = CurrentSample;
            sample.m_PitchMode = radioButtonProportional.Checked ? EPitchMode.Proportional : EPitchMode.Manual;
        }

        private void radioButtonProportional_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePitchModeFromUI();
            UpdateSample();
            UpdateProps();
        }

        private void radioButtonManual_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePitchModeFromUI();
            UpdateSample();
            UpdateProps();
        }

        private void trackBarReference_ValueChanged(object sender, EventArgs e)
        {
            if (IsModifying)
                return;
            float rpm = Math.Max((float)trackBarReference.Value * m_MaxRPM / c_SliderQuantScale, 1);
            Sample sample = CurrentSample;
            if (sample.m_ReferenceRPM == rpm)
                return;
            sample.m_ReferenceRPM = rpm;

            UpdateSample();
            UpdatePropsSoon();
        }

        private void textBoxMinPitch_TextChanged(object sender, EventArgs e)
        {
            if (IsModifying)
                return;
            try
            {
                float min_pitch = float.Parse(textBoxMinPitch.Text, CultureInfo.InvariantCulture);
                if (min_pitch < 0)
                    return;
                Sample sample = CurrentSample;
                sample.m_MinPitch = min_pitch;

                UpdatePropsSoon();
            }
            catch (Exception)
            {
            }
        }

        private void textBoxMaxPitch_TextChanged(object sender, EventArgs e)
        {
            if (IsModifying)
                return;
            try
            {
                float max_pitch = float.Parse(textBoxMaxPitch.Text, CultureInfo.InvariantCulture);
                if (max_pitch < 0)
                    return;
                Sample sample = CurrentSample;
                sample.m_MaxPitch = max_pitch;

                UpdateSampleMinMax(ESampleMinMaxMask.Min);
                UpdatePropsSoon();
            }
            catch (Exception)
            {
            }
        }

        private void comboBoxLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index_layer = comboBoxLayer.SelectedIndex;
            this.IndexLayer = index_layer;

            Layer layer = CurrentLayer;
            if (IndexSample >= layer.m_Samples.Count)
                IndexSample = layer.m_Samples.Count - 1;

            UpdateSamples();
            UpdatePlot();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (m_PendingUpdateProps)
                UpdateProps();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            EventHandler? handler = OnSave;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
