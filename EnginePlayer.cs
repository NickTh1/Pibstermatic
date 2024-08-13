using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace WaveMix
{
    enum ESampleType
    {
        On,
        Off,
        Idle,
        Other,
    };

    struct SEngineState
    {
        public SEngineState() { }

        public float m_RPM = 1000;

        public float m_On = 0;
        public float m_Off = 1;
    };

    class SSampleState
    {
        public bool m_Enabled = true;
        public bool m_ExtendedRange = false;
        public bool m_AutoMinPitch = false;
    };

    internal class EnginePlayer
    {
        static Tuple<string, ESampleType>[] s_SampleTypes = new Tuple<string, ESampleType>[0];

        public class SamplePoint
        {
            public float m_Value;
            public float m_Volume;

            public SamplePoint(float value, float volume)
            {
                m_Value = value;
                m_Volume = volume;
            }
        }

        public class Sample
        {
            public int m_IndexLayer;
            public int m_IndexSampleInLayer;
            public string m_WavName = "";
            public float m_MinPitch = 0;
            public float m_MaxPitch = 1;

            public ESampleType m_Type;
            public int m_HandleWav;

            public SamplePoint[] m_Points = new SamplePoint[0];
        };

        class SampleState
        {
            public bool m_Enabled = true;
            public float m_OverrideVolume = 0.0f;
            public bool m_AutoMinPitch = false;
        };

        WaveOut m_WaveOut = new WaveOut();
        WavPlayer m_WavPlayer = new WavPlayer();

        string m_Folder = ".";

        float m_MaxRPM = 19000;
        int m_AutoTune = 0;

        List<Sample> m_Samples = new List<Sample>();
        List<SampleState> m_SampleStates = new List<SampleState>();

        public EnginePlayer()
        {
            m_WaveOut.DesiredLatency = 100;//60;
            m_WaveOut.Init(m_WavPlayer);
        }

        static EnginePlayer()
        {
            var sample_types = new (string, ESampleType)[]
            {
            ("on", ESampleType.On),
            ("off", ESampleType.Off),
            ("idle", ESampleType.Idle),
            };
            Tuple<string, ESampleType>[] sample_type_tuples = new Tuple<string, ESampleType>[sample_types.Length];
            for (int i = 0; i < sample_types.Length; i++)
                sample_type_tuples[i] = new Tuple<string, ESampleType>(sample_types[i].Item1, sample_types[i].Item2);
            s_SampleTypes = sample_type_tuples;
        }

        public WaveOut WaveOut
        {
            get { return m_WaveOut; }
        }

        public WavPlayer WavPlayer
        {
            get { return m_WavPlayer; }
        }

        public string Folder
        {
            set { m_Folder = value; }
            get { return m_Folder; }
        }

        public float MaxRPM
        {
            get { return m_MaxRPM; }
        }

        public int NumSamples
        {
            get { return m_Samples.Count; }
        }

        public Sample GetSample(int index)
        {
            return m_Samples[index];
        }

        public void SetSampleState(int index, SSampleState new_sample_state)
        {
            SampleState sample_state = m_SampleStates[index];
            sample_state.m_Enabled = new_sample_state.m_Enabled;
            sample_state.m_OverrideVolume = new_sample_state.m_ExtendedRange ? 1.0f : 0.0f;
            sample_state.m_AutoMinPitch = new_sample_state.m_AutoMinPitch;
        }

        static ESampleType ParseSampleType(int layer, string filename)
        {
            string filename_no_ext = Path.GetFileNameWithoutExtension(filename);
            for (int i = 0; i < s_SampleTypes.Length; i++)
            {
                Tuple<string, ESampleType> tup = s_SampleTypes[i];
                if (filename_no_ext.StartsWith(tup.Item1))
                    return tup.Item2;
                if (filename_no_ext.EndsWith("_" + tup.Item1))
                    return tup.Item2;
                if (filename_no_ext.IndexOf("_" + tup.Item1 + "_") >= 0)
                    return tup.Item2;
            }
            return ESampleType.Other;
        }

        int FindSample(int index_layer, string wav_name)
        {
            for(int i = 0; i < m_Samples.Count; i++)
            {
                Sample sample = m_Samples[i];
                if (sample.m_IndexLayer == index_layer && sample.m_WavName.Equals(wav_name))
                    return i;
            }
            return -1;
        }

        public void UpdateEngineProperties(StructuredProperties struct_props)
        {
            m_MaxRPM = struct_props.GetFloat("MaxValue");
            m_AutoTune = struct_props.GetInteger("AutoTune", 0);

            List<Sample> samples = new List<Sample>();
            List<SampleState> sample_states = new List<SampleState>();
            for(int index_layer = 0; index_layer < 2; index_layer++)
            {
                StructuredProperties layer = struct_props.GetChild("Layer" + index_layer);

                int num_samples = layer.GetInteger("NumSamples");
                for (int j = 0; j < num_samples; j++)
                {
                    try
                    {
                        StructuredProperties src_sample = layer.GetChild("Sample" + j);
                        string wav_name = src_sample.GetString("Wav");

                        Sample sample = new Sample();
                        SampleState sample_state = new SampleState();

                        int index_curr_sample = FindSample(index_layer, wav_name);
                        if (index_curr_sample >= 0)
                        {
                            sample = m_Samples[index_curr_sample];
                            sample_state = m_SampleStates[index_curr_sample];
                        }

                        sample.m_IndexLayer = index_layer;
                        sample.m_IndexSampleInLayer = j;
                        sample.m_WavName = wav_name;
                        sample.m_Type = ParseSampleType(index_layer, wav_name);
                        sample.m_MinPitch = src_sample.GetFloat("MinPitch");
                        sample.m_MaxPitch = src_sample.GetFloat("MaxPitch");

                        string path = Path.Combine(m_Folder, wav_name);
                        sample.m_HandleWav = m_WavPlayer.AddWav(index_layer, path);

                        int num_points = src_sample.GetInteger("NumPoints");
                        if (num_points < 2)
                            continue;
                        sample.m_Points = new SamplePoint[num_points];
                        for (int k = 0; k < num_points; k++)
                        {
                            StructuredProperties pt = src_sample.GetChild("Point" + k);
                            float val = pt.GetFloat("Value");
                            float vol = pt.GetFloat("Volume");

                            sample.m_Points[k] = new SamplePoint(val, vol);
                        }
                        samples.Add(sample);
                        sample_states.Add(sample_state);
                    } catch (Exception)
                    {

                    }
                }
            }
            m_Samples = samples;
            m_SampleStates = sample_states;
        }

        public void UpdateEngineProperties(string engine_props_text)
        {
            try
            {
                StructuredProperties struct_props = PropsFile.ParsePropsFile(engine_props_text);
                UpdateEngineProperties(struct_props);
            }
            catch (Exception)
            {
            }
        }


        float EvaluateEngineState(int index_sample, in SEngineState engine_state)
        {
            Sample sample = m_Samples[index_sample];
            SampleState sample_state = m_SampleStates[index_sample];
            if (sample_state.m_OverrideVolume > 0)
                return 1.0f;
            return (sample.m_IndexLayer == 0) ? engine_state.m_On : engine_state.m_Off;
        }

        float? EvaluateSecant(int index_sample, float normalized_rpm)
        {
            Sample sample = m_Samples[index_sample];
            if (normalized_rpm < sample.m_Points[0].m_Value)
                return null;
            for (int i = 0; i < sample.m_Points.Length - 1; i++)
            {
                SamplePoint to = sample.m_Points[i + 1];
                if (to.m_Value >= normalized_rpm)
                {
                    SamplePoint from = sample.m_Points[i];
                    return i + (normalized_rpm - from.m_Value) / (to.m_Value - from.m_Value);
                }
            }
            return null;
        }

        float EvaluateVolume(int index_sample, float normalized_rpm, float weight)
        {
            Sample sample = m_Samples[index_sample];
            SampleState sample_state = m_SampleStates[index_sample];
            if (!sample_state.m_Enabled)
                return 0.0f;

            float? secant = EvaluateSecant(index_sample, normalized_rpm);

            float volume = 0;
            if (secant.HasValue)
            {
                float floor_secant = (float)Math.Floor(secant.Value);
                float residual = secant.Value - floor_secant;
                SamplePoint from = sample.m_Points[(int)floor_secant];
                if (residual > 0)
                {
                    SamplePoint to = sample.m_Points[(int)floor_secant + 1];
                    volume = from.m_Volume + residual * (to.m_Volume - from.m_Volume);
                } else
                    volume = from.m_Volume;
            }
            float weighted_volume = Math.Max(volume * weight, sample_state.m_OverrideVolume);
            return weighted_volume;
        }

        float EvaluatePitch(int index_sample, float normalized_rpm)
        {
            Sample sample = m_Samples[index_sample];
            SampleState sample_state = m_SampleStates[index_sample];

            float min_normed_rpm = sample.m_Points[0].m_Value;
            float max_normed_rpm = sample.m_Points[sample.m_Points.Length - 1].m_Value;

            if (sample_state.m_AutoMinPitch)
                return sample.m_MaxPitch * (normalized_rpm / max_normed_rpm);

            float t = (normalized_rpm - min_normed_rpm)/ (max_normed_rpm - min_normed_rpm);
            float t_clamped = Math.Clamp(t, 0, 1);
            return sample.m_MinPitch + t * (sample.m_MaxPitch - sample.m_MinPitch);
        }

        struct SWavSetting
        {
            public float m_Volume;
            public float m_Pitch;

            public SWavSetting(float volume, float pitch)
            {
                m_Volume = volume;
                m_Pitch = pitch;
            }
        }

        static SWavSetting MixSettings(SWavSetting op1, SWavSetting op2)
        {
            if (op1.m_Volume == 0)
                return op2;
            if (op2.m_Volume == 0)
                return op1;

            float pitch = (op1.m_Pitch * op1.m_Volume + op2.m_Pitch * op2.m_Volume)/ (op1.m_Volume + op2.m_Volume);
            return new SWavSetting(op1.m_Volume + op2.m_Volume, pitch);
        }

        public void UpdateAudio(in SEngineState engine_state)
        {
            float rpm = engine_state.m_RPM;
            float normalized_rpm = rpm / m_MaxRPM;

            SWavSetting[] wav_settings = new SWavSetting[m_WavPlayer.NumWavs];

            for( int index_sample = 0; index_sample < m_Samples.Count; index_sample++)
            {
                Sample sample = m_Samples[index_sample];

                float weight = EvaluateEngineState(index_sample, engine_state);
                if (weight == 0)
                    continue;

                float volume = EvaluateVolume(index_sample, normalized_rpm, weight);
                if (volume == 0)
                    continue;

                float pitch = EvaluatePitch(index_sample, normalized_rpm);
                if (pitch == 0)
                    continue;

                int handle_wav = sample.m_HandleWav;
                wav_settings[handle_wav] = MixSettings(wav_settings[handle_wav], new SWavSetting(volume, pitch));
            }

            for(int i = 0; i < wav_settings.Length; i++)
            {
                ref SWavSetting wav_setting = ref wav_settings[i];
                m_WavPlayer.SetWavPlayback(i, wav_setting.m_Volume, wav_setting.m_Pitch);
            }
            m_WavPlayer.CommitScape();
        }
    }
}
