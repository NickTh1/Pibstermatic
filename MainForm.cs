using System.Globalization;
using System.Diagnostics;
using NAudio.Wave;
using NAudio.MediaFoundation;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Annotations;
using NAudio.Dsp;

namespace WaveMix
{
    public partial class MainForm : Form
    {
        static readonly int c_FFTLogBufferSize = 16;
        static readonly int c_FFTBufferSize = 1 << c_FFTLogBufferSize;
        static readonly float c_SliderQuantScale = 1e6f;
        static readonly int c_WaveBufferSize = 1024;

        enum EColumnIndex
        {
            WavName,
            Enabled,
            ExtendedRange,
            AutoMinPitch,
            RecommendedMinPitch,
            CurrentVolume,
        }

        struct SCheckboxStates
        {
            public SCheckboxStates() { }

            public int m_NumChecked = 0;
            public int m_NumUnchecked = 0;
        }

        class GridSampleModel
        {
            public bool m_Enabled = true;
            public bool m_ExtendedRange = false;
            public bool m_AutoMinPitch = false;
            public bool m_CanAutoMinPitch = true;
        }

        EnginePlayer m_EnginePlayer = new EnginePlayer();
        EngineSim m_EngineSim = new EngineSim();
        Settings m_Settings = new Settings();
        string m_EnginePath = "";

        PlotModel m_PlotModelFFT = new PlotModel();
        LineSeries m_FFTLineSeries = new LineSeries();
        LineAnnotation m_FFTAnnotationCycle = new LineAnnotation();
        LineAnnotation m_FFTAnnotationRevolution = new LineAnnotation();
        LineAnnotation m_FFTAnnotationCylinder = new LineAnnotation();
        LineAnnotation m_FFTAnnotationCombustion = new LineAnnotation();

        PlotModel m_PlotModelWave = new PlotModel();
        LineSeries m_WaveLineSeries = new LineSeries();

        WavPlayer.Reader? m_EngineOutputReader = null;

        float[] m_FFTBuffer = new float[c_FFTBufferSize];
        Complex[] m_FFTComplexBuffer = new Complex[c_FFTBufferSize];

        float[] m_WaveBuffer = new float[c_WaveBufferSize];

        Stopwatch m_TimerStopwatch = new Stopwatch();
        double m_ElapsedLast = 0;
        string m_JustSavedText = "";
        double m_TimeJustSaved = 0;

        DialogResult m_StoredReloadDlgResult = DialogResult.None;
        bool m_PromptingReload = false;

        Dictionary<Tuple<int, string>, GridSampleModel> m_SampleModels = new Dictionary<Tuple<int, string>, GridSampleModel>();

        FileSystemWatcher m_EngineFileWatcher;

        TextEditor? m_LiveEditor = null;
        SCLEditor? m_SCLEditor = null;

        int m_Stroke = 4;
        int m_Cylinders = 4;

        int m_Modifying = 1;

        public MainForm(Settings settings)
        {
            m_Settings = settings;
            m_EnginePath = settings.PathEngine ?? throw new Exception("Engine path must not be null");

            InitializeComponent();
            Focus();

            string bike_folder = Path.GetDirectoryName(m_EnginePath) ?? ".";

            m_EngineFileWatcher = new FileSystemWatcher(bike_folder);
            m_EngineFileWatcher.Filter = "*.scl";
            m_EngineFileWatcher.EnableRaisingEvents = true;
            m_EngineFileWatcher.Changed += EngineFileWatcher_Changed;
            m_EngineFileWatcher.Created += EngineFileWatcher_Changed;
            m_EngineFileWatcher.Renamed += EngineFileWatcher_Changed;

            m_EnginePlayer.Folder = bike_folder;

            UpdateEngineFromFile();
            m_EnginePlayer.WaveOut.Play();

            m_EngineOutputReader = m_EnginePlayer.WavPlayer.ReaderCreate();

            trackBarOverallVolume.Value = (int)Math.Round(settings.OverallVolume * c_SliderQuantScale);
            comboBoxStroke.Text = settings.Stroke.ToString();
            comboBoxCylinders.Text = settings.Cylinders.ToString();
            textBoxIdleRPM.Text = ((int)settings.IdleRPM).ToString();
            UpdateStrokeCylinders();
            m_EngineSim.IdleRPM = settings.IdleRPM;

            SetRawRPM(settings.IdleRPM);

            UpdateRaw();
            UpdateOverallVolume();

            {
                // Init FFT plot
                m_PlotModelFFT.Title = "FFT";
                m_PlotModelFFT.TextColor = OxyColors.White;
                m_PlotModelFFT.TitleColor = OxyColors.White;
                m_PlotModelFFT.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, AxislineColor = OxyColors.White });
                m_PlotModelFFT.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Left, AxislineColor = OxyColors.White });
                m_PlotModelFFT.Axes[1].IsAxisVisible = false;

                m_FFTAnnotationCycle.Type = LineAnnotationType.Vertical;
                m_FFTAnnotationRevolution.Type = LineAnnotationType.Vertical;
                m_FFTAnnotationCombustion.Type = LineAnnotationType.Vertical;
                m_FFTAnnotationCylinder.Type = LineAnnotationType.Vertical;

                m_FFTAnnotationCycle.Text = "Cycle";
                m_FFTAnnotationRevolution.Text = "Revolution";
                m_FFTAnnotationCombustion.Text = "Combustion";
                m_FFTAnnotationCylinder.Text = "Cylinder";

                m_PlotModelFFT.Series.Add(m_FFTLineSeries);

                plotViewFFT.Model = m_PlotModelFFT;
            }

            {
                // Init Wave plot
                m_PlotModelWave.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
                m_PlotModelWave.Axes.Add(new LinearAxis { Position = AxisPosition.Left, AxislineColor = OxyColors.White });
                m_PlotModelWave.Axes[0].Minimum = 0;
                m_PlotModelWave.Axes[0].Maximum = 1;
                m_PlotModelWave.Axes[1].Minimum = -1.1f;
                m_PlotModelWave.Axes[1].Maximum = 1.1f;
                m_PlotModelWave.Axes[0].IsAxisVisible = false;

                m_PlotModelWave.Series.Add(m_WaveLineSeries);
                plotViewWave.Model = m_PlotModelWave;
            }


            timer.Interval = 16;
            timer.Tick += Timer_Tick;
            timer.Start();

            m_Modifying = 0;       // Enable OnValueChanged
        }

        private void M_EngineFileWatcher_Renamed(object sender, RenamedEventArgs e)
        {
        }

        void EngineFileWatcher_Changed_MainThread(object sender, FileSystemEventArgs e)
        {
            if (IsModifying)
                return;
            AskThenReloadEngineFromFile();
        }

        private void EngineFileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            BeginInvoke(new FileSystemEventHandler(EngineFileWatcher_Changed_MainThread), sender, e);
        }

        bool IsModifying
        {
            get { return m_Modifying > 0; }
        }

        void UpdateSpectrum()
        {
            if (m_EngineOutputReader == null)       // Get rid of warning
                return;

            int len = c_FFTBufferSize;

            float[] data = m_FFTBuffer;
            m_EnginePlayer.WavPlayer.ReaderRead(m_EngineOutputReader, data);

            Complex[] fft_complex_buffer = m_FFTComplexBuffer;
            for (int i = 0; i < len; i++)
            {
                ref Complex c = ref fft_complex_buffer[i];
                c.X = data[i] * (float)FastFourierTransform.HammingWindow(i, len);
                c.Y = 0;
            }

            FastFourierTransform.FFT(true, c_FFTLogBufferSize, fft_complex_buffer);

            m_FFTLineSeries.Points.Clear();
            float min_mag = 100000;
            float max_mag = -10000;
            int half_len = len / 2;
            for (int i = 0; i < half_len; i++)
            {
                Complex v = fft_complex_buffer[i];
                float magnitude = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);
                min_mag = Math.Min(min_mag, magnitude);
                max_mag = Math.Max(max_mag, magnitude);

                float freq = (float)i * (88200.0f / c_FFTBufferSize);

                m_FFTLineSeries.Points.Add(new DataPoint(freq, magnitude));
            }

            float plot_max_freq = m_EnginePlayer.MaxRPM * (1.0f / 60.0f) * (float)m_Cylinders * (2.0f / m_Stroke);

            m_PlotModelFFT.Axes[0].Minimum = -10;       // So that the annotation doesn't disappear when at 0.
            m_PlotModelFFT.Axes[0].Maximum = plot_max_freq * 1.01f;
            m_PlotModelFFT.Axes[1].Minimum = Math.Max(min_mag, 1e-9f);
            m_PlotModelFFT.Axes[1].Maximum = max_mag;
            m_PlotModelFFT.ResetAllAxes();

            plotViewFFT.InvalidatePlot(true);
        }

        void UpdateWave()
        {
            if (m_EngineOutputReader == null)       // Get rid of warning
                return;
            float[] data = m_WaveBuffer;
            m_EnginePlayer.WavPlayer.ReaderRead(m_EngineOutputReader, data);

            m_WaveLineSeries.Points.Clear();
            for (int i = 0; i < c_WaveBufferSize; i++)
                m_WaveLineSeries.Points.Add(new DataPoint((float)i / (float)c_WaveBufferSize, Math.Clamp(data[i], -1, 1)));

            plotViewWave.InvalidatePlot(true);
        }

        void UpdateRMS()
        {
            float rms = m_EnginePlayer.WavPlayer.GetRMS();
            textBoxRMS.Text = rms.ToString("0.000", CultureInfo.InvariantCulture);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (m_EngineOutputReader == null)       // Get rid of warning
                return;

            float dt = 0;
            double elapsed_now = 0;
            if (!m_TimerStopwatch.IsRunning)
                m_TimerStopwatch.Start();
            else
            {
                elapsed_now = m_TimerStopwatch.Elapsed.TotalSeconds;
                dt = (float)(elapsed_now - m_ElapsedLast);
                m_ElapsedLast = elapsed_now;
            }

            // Note: Scale up dt a wee bit to ensure that we don't accumulate a lag. It's relatively harmless to step 'too much' - it's clamped.
            m_EnginePlayer.WavPlayer.ReaderAdvance(m_EngineOutputReader, dt * 1.1f);

            if (IsSimEnabled)
                UpdateSim(dt);

            UpdateSpectrum();
            UpdateWave();
            UpdateRMS();
            UpdateGridState();

            if (m_JustSavedText != null)
            {
                if (elapsed_now - m_TimeJustSaved > 1.0)
                    m_JustSavedText = "";       // Clear after a second. If it changes after that, we can prompt.
            }
        }

        static bool CanAutoPitchMin(EnginePlayer.Sample sample)
        {
            return (sample.m_Type == ESampleType.Off || sample.m_Type == ESampleType.On);
        }

        GridSampleModel GetSampleModel(EnginePlayer.Sample sample)
        {
            Tuple<int, string> key = new Tuple<int, string>(sample.m_IndexLayer, sample.m_WavName);

            GridSampleModel? model = null;
            if (m_SampleModels.TryGetValue(key, out model))
                return model;
            model = new GridSampleModel();
            m_SampleModels.Add(key, model);
            return model;
        }

        void UpdateGridRows()
        {
            DataGridViewRowCollection rows = dataGridViewWavs.Rows;
            DataGridViewColumnCollection columns = dataGridViewWavs.Columns;
            int num_samples = m_EnginePlayer.NumSamples;
            while (rows.Count < num_samples)
                rows.Add();
            while (rows.Count > Math.Max(num_samples, 1))
                rows.RemoveAt(rows.Count - 1);

            Color col_ok = Color.FromArgb(0xff, 0xe0, 0xff, 0xe0);
            Color col_not_ok = Color.FromArgb(0xff, 0xff, 0xe0, 0xe0);

            using (Modifying modifying = Modify())
            {
                for (int i = 0; i < num_samples; i++)
                {
                    EnginePlayer.Sample sample = m_EnginePlayer.GetSample(i);
                    GridSampleModel sample_model = GetSampleModel(sample);

                    DataGridViewCellCollection cells = rows[i].Cells;
                    cells[(int)EColumnIndex.WavName].Value = sample.m_WavName;

                    cells[(int)EColumnIndex.Enabled].Value = sample_model.m_Enabled;
                    cells[(int)EColumnIndex.AutoMinPitch].Value = sample_model.m_AutoMinPitch;
                    cells[(int)EColumnIndex.ExtendedRange].Value = sample_model.m_ExtendedRange;

                    bool can_auto_pitch_min = CanAutoPitchMin(sample);
                    sample_model.m_CanAutoMinPitch = can_auto_pitch_min;

                    DataGridViewCheckBoxCell? chk_auto_min_pitch = cells[(int)EColumnIndex.AutoMinPitch] as DataGridViewCheckBoxCell;
                    if (chk_auto_min_pitch != null)     // Warning
                    {
                        DataGridViewColumn column_auto_min_pitch = columns[(int)EColumnIndex.AutoMinPitch];
                        chk_auto_min_pitch.ReadOnly = !can_auto_pitch_min;
                        chk_auto_min_pitch.FlatStyle = can_auto_pitch_min ? FlatStyle.Standard : FlatStyle.Flat;
                        chk_auto_min_pitch.Style.ForeColor = can_auto_pitch_min ? column_auto_min_pitch.DefaultCellStyle.ForeColor : Color.White;
                    }

                    string recommended_min_pitch_str = "";
                    if (can_auto_pitch_min)
                    {
                        float min_value = sample.m_Points[0].m_Value;
                        float max_value = sample.m_Points[sample.m_Points.Length - 1].m_Value;
                        float recommended_min_pitch = sample.m_MaxPitch * min_value / max_value;
                        recommended_min_pitch_str = recommended_min_pitch.ToString("0.0000", CultureInfo.InvariantCulture);

                        bool is_ok = Math.Abs(recommended_min_pitch - sample.m_MinPitch) < 0.02f;
                        cells[(int)EColumnIndex.RecommendedMinPitch].Style.BackColor = is_ok ? col_ok : col_not_ok;
                    }
                    else
                    {
                        DataGridViewColumn column_min_pitch = columns[(int)EColumnIndex.RecommendedMinPitch];
                        cells[(int)EColumnIndex.RecommendedMinPitch].Style.BackColor = column_min_pitch.DefaultCellStyle.BackColor;
                    }
                    cells[(int)EColumnIndex.RecommendedMinPitch].Value = recommended_min_pitch_str;
                }
            }

            UpdateFromGrid();
            UpdateButtons();
            UpdateEngineState();
        }

        void UpdateGridState()
        {
            int index_scledit_layer = -1;
            int index_scledit_sample = -1;
            if (m_SCLEditor != null)
            {
                index_scledit_layer = m_SCLEditor.GetIndexLayer();
                index_scledit_sample = m_SCLEditor.GetIndexSample();
            }

            using (Modifying modifying = Modify())
            {
                DataGridViewRowCollection rows = dataGridViewWavs.Rows;
                int num_samples = m_EnginePlayer.NumSamples;
                Color color_active = Color.FromArgb(0xff, 0xff, 0xe0, 0xff);
                for (int i = 0; i < num_samples; i++)
                {
                    EnginePlayer.Sample sample = m_EnginePlayer.GetSample(i);
                    DataGridViewCellCollection cells = rows[i].Cells;

                    float amplitude = m_EnginePlayer.WavPlayer.GetWavPlaybackAmplitude(sample.m_HandleWav);
                    cells[(int)EColumnIndex.CurrentVolume].Value = (amplitude > 0.0f) ? amplitude.ToString("0.00", CultureInfo.InvariantCulture) : "";
                    cells[(int)EColumnIndex.CurrentVolume].Style.BackColor = (amplitude > 0.0f) ? color_active : Color.White;

                    if (sample.m_IndexLayer == index_scledit_layer && sample.m_IndexSampleInLayer == index_scledit_sample && m_SCLEditor != null)
                        m_SCLEditor.SetCurrentSampleState(amplitude);
                }
            }
        }

        void UpdateEngineFromFile()
        {
            try
            {
                string engine_props_text = File.ReadAllText(m_EnginePath);

                m_EnginePlayer.UpdateEngineProperties(engine_props_text);
                UpdateGridRows();
                UpdateEngineState();

                if (m_LiveEditor != null)
                    m_LiveEditor.EditorText = engine_props_text;

                if (m_SCLEditor != null)
                    m_SCLEditor.UpdateFromText(engine_props_text);
            }
            catch (Exception)
            {
            }
        }

        void AskThenReloadEngineFromFile()
        {
            try
            {
                string engine_props_text = File.ReadAllText(m_EnginePath);
                if (string.Equals(engine_props_text, m_JustSavedText))
                    return;
                if (m_PromptingReload)
                    return;                 // Can otherwise recurse :-/ 
                m_PromptingReload = true;

                DialogResult reload_dlg_result = m_StoredReloadDlgResult;
                if (reload_dlg_result == DialogResult.None)
                {
                    bool dont_ask_again = false;
                    reload_dlg_result = MessageBoxAskAgain.Show("File was modified on disk", "Reload?", out dont_ask_again);
                    if (dont_ask_again)
                        m_StoredReloadDlgResult = reload_dlg_result;
                }
                if (reload_dlg_result != DialogResult.Yes)
                    return;

                UpdateEngineFromFile();
            }
            catch (Exception)
            {
            }
            finally
            {
                m_PromptingReload = false;
            }
        }

        void SetSimRPM(float rpm)
        {
            m_EngineSim.CurrentRPM = rpm;
        }

        void SetRawRPM(float rpm)
        {
            TrackBarRPM = rpm;
            SetTextBoxRPM(rpm);
            SetSimRPM(rpm);
        }

        float TrackBarRPM
        {
            get { return trackBarRPM.Value * m_EnginePlayer.MaxRPM / c_SliderQuantScale; }
            set { trackBarRPM.Value = (int)Math.Round(value * c_SliderQuantScale / m_EnginePlayer.MaxRPM); }
        }

        bool IsSimEnabled
        {
            get { return tabControlControl.SelectedIndex != 0; }
        }

        void UpdateAnnotations()
        {
            m_PlotModelFFT.Annotations.Clear();
            m_PlotModelFFT.Annotations.Add(m_FFTAnnotationRevolution);

            if (m_Stroke != 2)
                m_PlotModelFFT.Annotations.Add(m_FFTAnnotationCycle);

            if (m_Cylinders != m_Stroke / 2)
                m_PlotModelFFT.Annotations.Add(m_FFTAnnotationCombustion);

            if (m_Cylinders != 1 && m_Stroke != 2)
                m_PlotModelFFT.Annotations.Add(m_FFTAnnotationCylinder);
        }

        void SetAnnotations(SEngineState engine_state)
        {
            float rotation_freq = engine_state.m_RPM * (1.0f / 60.0f);

            m_FFTAnnotationCycle.X = rotation_freq * 2.0f / (float)m_Stroke;
            m_FFTAnnotationRevolution.X = rotation_freq;
            m_FFTAnnotationCylinder.X = rotation_freq * (float)m_Cylinders;
            m_FFTAnnotationCombustion.X = rotation_freq * (float)m_Cylinders * 2.0f / (float)m_Stroke;

            m_FFTAnnotationCycle.Color = Utils.Lerp(engine_state.m_On, OxyColor.FromArgb(0xff, 0x40, 0x40, 0x30), OxyColor.FromArgb(0xff, 0xff, 0xa0, 0xff));
            m_FFTAnnotationRevolution.Color = OxyColor.FromArgb(0xff, 0xff, 0xff, 0xa0);
            m_FFTAnnotationCylinder.Color = OxyColor.FromArgb(0xff, 0x80, 0x80, 0x50);
            m_FFTAnnotationCombustion.Color = Utils.Lerp(engine_state.m_On, OxyColor.FromArgb(0xff, 0x30, 0x40, 0x40), OxyColor.FromArgb(0xff, 0xa0, 0xff, 0xff));

            m_FFTAnnotationCycle.TextColor = Utils.Lerp(engine_state.m_On, OxyColors.Gray, OxyColors.White);
            m_FFTAnnotationCombustion.TextColor = Utils.Lerp(engine_state.m_On, OxyColors.Gray, OxyColors.White);
            m_FFTAnnotationCylinder.TextColor = OxyColors.Gray;
        }

        void UpdateFromEngineState(SEngineState engine_state)
        {
            m_EnginePlayer.UpdateAudio(engine_state);

            SetAnnotations(engine_state);
        }

        void SetSCLEditorRPM(float rpm)
        {
            if (m_SCLEditor == null)
                return;
            m_SCLEditor.RPM = rpm;
        }

        void UpdateRaw()
        {
            float on = ((float)trackBarOn.Value / 1000000.0f);
            float rpm = TrackBarRPM;

            SEngineState engine_state = new SEngineState();
            engine_state.m_RPM = rpm;
            engine_state.m_Off = 1.0f - on;
            engine_state.m_On = on;

            UpdateFromEngineState(engine_state);

            SetSCLEditorRPM(rpm);
        }

        void UpdateSim(float dt)
        {
            float throttle = (float)trackBarThrottle.Value / c_SliderQuantScale;
            SEngineState engine_state = m_EngineSim.Update(dt, throttle);
            UpdateFromEngineState(engine_state);

            using (Modifying modifying = Modify())
            {
                SetTextBoxRPM(m_EngineSim.CurrentRPM);
                TrackBarRPM = m_EngineSim.CurrentRPM;
                textBoxOn.Text = engine_state.m_On.ToString("0.00", CultureInfo.InvariantCulture);
                SetSCLEditorRPM(m_EngineSim.CurrentRPM);
            }
        }

        void UpdateEngineState()
        {
            m_EngineSim.MaxRPM = m_EnginePlayer.MaxRPM;
            if (!IsSimEnabled)
                UpdateRaw();
        }

        Modifying Modify()
        {
            m_Modifying++;
            return new Modifying(this);
        }

        void SetTextBoxRPM(float rpm)
        {
            string rpm_str = ((int)Math.Round(rpm)).ToString();
            using (Modifying modifying = Modify())
            {
                textBoxRPM.Text = rpm_str;
            }
            textBoxCurrentRPM.Text = rpm_str;
        }

        private void trackBarRPM_Scroll(object sender, EventArgs e)
        {
            if (IsModifying)
                return;

            float rpm = TrackBarRPM;
            SetTextBoxRPM(rpm);

            UpdateRaw();
        }

        private void trackBarOn_Scroll(object sender, EventArgs e)
        {
            if (IsModifying)
                return;

            UpdateRaw();
        }

        private void textBoxRPM_TextChanged(object sender, EventArgs e)
        {
            if (IsModifying)
                return;

            try
            {
                float rpm = (float)Convert.ToDouble(textBoxRPM.Text);
                TrackBarRPM = rpm;

                UpdateRaw();
            }
            catch
            {
            }
        }

        void UpdateOverallVolume()
        {
            float volume = (float)trackBarOverallVolume.Value / c_SliderQuantScale;
            m_EnginePlayer.WavPlayer.OverallVolume = volume;
        }

        private void trackBarOverallVolume_Scroll(object sender, EventArgs e)
        {
            UpdateOverallVolume();
            WriteSettings();
        }

        void textEditor_EditorTextChanged(object? sender, EventArgs e)
        {
            m_JustSavedText = "";
            if (m_LiveEditor == null)       // Get rid of warning
                return;
            string engine_props_text = m_LiveEditor.EditorText;
            m_EnginePlayer.UpdateEngineProperties(engine_props_text);
            UpdateGridRows();
            UpdateEngineState();

            if (m_SCLEditor != null)
                m_SCLEditor.UpdateFromText(engine_props_text);
        }

        void Save(string engine_props_text)
        {
            try
            {
                m_JustSavedText = engine_props_text;
                File.WriteAllText(m_EnginePath, engine_props_text);
                m_TimeJustSaved = m_TimerStopwatch.Elapsed.Seconds;
            }
            catch (Exception)
            {
            }
        }

        void textEditor_Save(object? sender, EventArgs e)
        {
            using (Modifying modifying = Modify())
            {
                if (m_LiveEditor == null)       // Get rid of warning
                    return;

                string engine_props_text = m_LiveEditor.EditorText;
                Save(engine_props_text);
            }
        }

        void EnsureLiveEditorOpen()
        {
            if (m_LiveEditor != null && !m_LiveEditor.IsDisposed)
                return;
            m_LiveEditor = new TextEditor(m_EnginePath);
            m_LiveEditor.OnEditorTextChanged += new System.EventHandler(textEditor_EditorTextChanged);
            m_LiveEditor.OnSave += new System.EventHandler(textEditor_Save);
            m_LiveEditor.Show();
        }

        private void buttonLiveEdit_Click(object sender, EventArgs e)
        {
            EnsureLiveEditorOpen();
        }

        class Modifying : IDisposable
        {
            MainForm m_Owner;

            internal Modifying(MainForm owner)
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

        static bool CellValueToBool(object val)
        {
            return (val != null) ? (bool)val : false;
        }

        SCheckboxStates GetCheckboxStates(EColumnIndex index_column)
        {
            SCheckboxStates retval = new SCheckboxStates();
            DataGridViewRowCollection rows = dataGridViewWavs.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                DataGridViewCheckBoxCell? chk_cell = rows[i].Cells[(int)index_column] as DataGridViewCheckBoxCell;
                if (chk_cell == null)
                    continue;
                if (chk_cell.FlatStyle == FlatStyle.Flat)       // This will be the state when the checkbox is hidden. Ignore those
                    continue;
                if (CellValueToBool(chk_cell.Value))
                    retval.m_NumChecked++;
                else
                    retval.m_NumUnchecked++;
            }
            return retval;
        }

        void SetCheckboxColumn(EColumnIndex index_column, bool new_state)
        {
            DataGridViewRowCollection rows = dataGridViewWavs.Rows;
            using (Modifying modifying = Modify())      // Avoid an O(N^2)
            {
                SCheckboxStates curr_states = GetCheckboxStates(index_column);
                bool enabled = curr_states.m_NumChecked > curr_states.m_NumUnchecked;
                for (int i = 0; i < rows.Count; i++)
                {
                    DataGridViewCellCollection cells = rows[i].Cells;
                    cells[(int)index_column].Value = new_state;
                }
            }
            dataGridViewWavs.RefreshEdit();
            UpdateFromGrid();
            UpdateEngineState();
        }

        private void dataGridViewWavs_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewRowCollection rows = dataGridViewWavs.Rows;
            EColumnIndex index_column = (EColumnIndex)e.ColumnIndex;
            if (index_column == EColumnIndex.Enabled || index_column == EColumnIndex.AutoMinPitch)
            {
                bool enabled = CellValueToBool(rows[0].Cells[(int)index_column].Value);
                SetCheckboxColumn(index_column, !enabled);
            }
        }

        void UpdateFromGridRow(int index_row)
        {
            EnginePlayer.Sample sample = m_EnginePlayer.GetSample(index_row);
            DataGridViewCellCollection cells = dataGridViewWavs.Rows[index_row].Cells;

            bool enabled = CellValueToBool(cells[(int)EColumnIndex.Enabled].Value);
            bool extend_range = CellValueToBool(cells[(int)EColumnIndex.ExtendedRange].Value);
            bool auto_min_pitch = CellValueToBool(cells[(int)EColumnIndex.AutoMinPitch].Value);

            GridSampleModel sample_model = GetSampleModel(sample);
            sample_model.m_Enabled = enabled;
            sample_model.m_ExtendedRange = extend_range;
            sample_model.m_AutoMinPitch = auto_min_pitch;

            SSampleState sample_state = new SSampleState();
            sample_state.m_Enabled = enabled || extend_range;
            sample_state.m_ExtendedRange = extend_range;
            sample_state.m_AutoMinPitch = auto_min_pitch && CanAutoPitchMin(sample);

            m_EnginePlayer.SetSampleState(index_row, sample_state);

            if (m_SCLEditor != null)
                m_SCLEditor.SetSampleAutoMinPitch(sample.m_IndexLayer, sample.m_IndexSampleInLayer, sample_state.m_AutoMinPitch);
        }

        void UpdateButtons()
        {
            SCheckboxStates states_enabled = GetCheckboxStates(EColumnIndex.Enabled);
            buttonEnableAll.Enabled = states_enabled.m_NumUnchecked > 0;
            buttonDisableAll.Enabled = states_enabled.m_NumChecked > 0;

            SCheckboxStates states_automin = GetCheckboxStates(EColumnIndex.AutoMinPitch);
            buttonAutoMinPitchEnable.Enabled = states_automin.m_NumUnchecked > 0;
            buttonAutoMinPitchDisable.Enabled = states_automin.m_NumChecked > 0;
        }

        void UpdateFromGrid()
        {
            DataGridViewRowCollection rows = dataGridViewWavs.Rows;
            for (int i = 0; i < rows.Count; i++)
                UpdateFromGridRow(i);
            UpdateButtons();
            UpdateEngineState();
        }

        void CellChanged(int index_row, EColumnIndex index_column)
        {
            if (IsModifying)
                return;
            if (index_column == EColumnIndex.Enabled || index_column == EColumnIndex.ExtendedRange || index_column == EColumnIndex.AutoMinPitch)
            {
                UpdateFromGridRow(index_row);
                UpdateButtons();
                UpdateEngineState();
            }
        }

        private void dataGridViewWavs_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            CellChanged(e.RowIndex, (EColumnIndex)e.ColumnIndex);
        }

        private void dataGridViewWavs_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            CellChanged(e.Cell.RowIndex, (EColumnIndex)e.Cell.ColumnIndex);
        }

        private void dataGridViewWavs_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dataGridViewWavs.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dataGridViewWavs_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index_row = e.RowIndex;
            if (index_row > m_EnginePlayer.NumSamples)
                return;
            EnginePlayer.Sample sample = m_EnginePlayer.GetSample(index_row);
            string wav_name = sample.m_WavName;

            EnsureLiveEditorOpen();

            if (m_LiveEditor == null)       // Warning
                return;

            string text = m_LiveEditor.EditorText;
            int pos_wav = text.IndexOf(wav_name);
            if (pos_wav < 0)
                return;

            int pos_sample = text.Substring(0, pos_wav).LastIndexOf("Sample");
            if (pos_sample < 0)
                return;

            m_LiveEditor.SetCursor(pos_sample);
        }

        void UpdateStrokeCylinders()
        {
            m_Stroke = Int32.Parse(comboBoxStroke.Text);
            m_Cylinders = Int32.Parse(comboBoxCylinders.Text);
            UpdateAnnotations();
            UpdateEngineState();
        }

        void WriteSettings()
        {
            m_Settings.Stroke = m_Stroke;
            m_Settings.Cylinders = m_Cylinders;
            m_Settings.OverallVolume = m_EnginePlayer.WavPlayer.OverallVolume;
            m_Settings.IdleRPM = m_EngineSim.IdleRPM;

            SettingsIO.Write(m_Settings);
        }

        private void comboBoxStroke_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsModifying)
                return;
            UpdateStrokeCylinders();
            WriteSettings();
        }

        private void comboBoxCylinders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsModifying)
                return;
            UpdateStrokeCylinders();
            WriteSettings();
        }

        private void buttonEnableAll_Click(object sender, EventArgs e)
        {
            SetCheckboxColumn(EColumnIndex.Enabled, true);
        }

        private void buttonDisableAll_Click(object sender, EventArgs e)
        {
            SetCheckboxColumn(EColumnIndex.Enabled, false);
        }

        private void buttonAutoMinPitchEnable_Click(object sender, EventArgs e)
        {
            SetCheckboxColumn(EColumnIndex.AutoMinPitch, true);
        }

        private void buttonAutoMinPitchDisable_Click(object sender, EventArgs e)
        {
            SetCheckboxColumn(EColumnIndex.AutoMinPitch, false);
        }

        private void trackBarGearing_Scroll(object sender, EventArgs e)
        {
            float gearing = (float)trackBarGearing.Value / c_SliderQuantScale;
            m_EngineSim.Gearing = gearing;
        }

        private void checkBoxClutch_CheckedChanged(object sender, EventArgs e)
        {
            m_EngineSim.Neutral = checkBoxClutch.Checked;
        }

        private void buttonBrake_MouseDown(object sender, MouseEventArgs e)
        {
            m_EngineSim.Brake = 1.0f;
        }

        private void buttonBrake_MouseUp(object sender, MouseEventArgs e)
        {
            m_EngineSim.Brake = 0.0f;
        }

        private void textBoxIdleRPM_TextChanged(object sender, EventArgs e)
        {
            if (IsModifying)
                return;
            try
            {
                float idle_rpm = (float)Convert.ToDouble(textBoxIdleRPM.Text);
                m_EngineSim.IdleRPM = idle_rpm;
                WriteSettings();
            }
            catch (Exception)
            {
            }
        }

        void sclEditor_PropertyChanged(object? sender, EventArgs e)
        {
            if (m_SCLEditor == null)       // Get rid of warning
                return;
            m_JustSavedText = "";
            StructuredProperties engine_props = m_SCLEditor.EngineProperties;
            m_EnginePlayer.UpdateEngineProperties(engine_props);
            UpdateGridRows();
            UpdateEngineState();

            if (m_LiveEditor != null && !m_LiveEditor.IsDisposed)
            {
                string engine_props_text = PropsFile.SerializePropsFile(engine_props);
                m_LiveEditor.EditorText = engine_props_text;
            }
        }

        int FindSample(int index_layer, int index_sample_in_layer)
        {
            int num_samples = m_EnginePlayer.NumSamples;
            for (int i = 0; i < num_samples; i++)
            {
                EnginePlayer.Sample sample = m_EnginePlayer.GetSample(i);
                if (sample.m_IndexLayer == index_layer && sample.m_IndexSampleInLayer == index_sample_in_layer)
                    return i;
            }
            return -1;
        }

        void sclEditor_SelectedSampleChanged(object? sender, EventArgs e)
        {
            if (m_SCLEditor == null)       // Get rid of warning
                return;

            int index_layer = m_SCLEditor.GetIndexLayer();
            int index_sample_in_layer = m_SCLEditor.GetIndexSample();
            int index_sample = FindSample(index_layer, index_sample_in_layer);

            if (index_sample >= 0)
            {
                dataGridViewWavs.ClearSelection();
                dataGridViewWavs.Rows[index_sample].Selected = true;

                EnginePlayer.Sample sample = m_EnginePlayer.GetSample(index_sample);
                GridSampleModel sample_model = GetSampleModel(sample);
                m_SCLEditor.SetSampleAutoMinPitch(index_layer, index_sample_in_layer, sample_model.m_AutoMinPitch);
            }
        }

        void sclEditor_Save(object? sender, EventArgs e)
        {
            using (Modifying modifying = Modify())
            {
                if (m_SCLEditor == null)       // Get rid of warning
                    return;

                StructuredProperties props = m_SCLEditor.EngineProperties;
                string engine_props_text = PropsFile.SerializePropsFile(props);

                Save(engine_props_text);
            }
        }


        void EnsureSCLEditorOpen()
        {
            if (m_SCLEditor != null && !m_SCLEditor.IsDisposed)
                return;
            try
            {
                string engine_props_text;
                if (m_LiveEditor != null && !m_LiveEditor.IsDisposed)
                    engine_props_text = m_LiveEditor.EditorText;
                else
                    engine_props_text = File.ReadAllText(m_EnginePath);

                m_SCLEditor = new SCLEditor(m_EnginePath, engine_props_text);
                m_SCLEditor.OnPropertyChanged += new System.EventHandler(sclEditor_PropertyChanged);
                m_SCLEditor.OnSelectedSampleChanged += new System.EventHandler(sclEditor_SelectedSampleChanged);
                m_SCLEditor.OnSave += new System.EventHandler(sclEditor_Save);
                m_SCLEditor.Show();

                SetSCLEditorRPM(TrackBarRPM);
            } catch(Exception)
            {
            }
        }

        private void buttonEditEnvelopes_Click(object sender, EventArgs e)
        {
            EnsureSCLEditorOpen();
        }
    }
}