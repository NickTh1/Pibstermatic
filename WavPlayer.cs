using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using NAudio.Wave;
using NAudio.MediaFoundation;

namespace WaveMix
{
    internal class WavPlayer : IWaveProvider
    {
        static readonly float c_SampleRate = 88200;
        static readonly int c_CircularBufferSize = 1 << 17;
        static readonly int c_CircularBufferMask = c_CircularBufferSize - 1;

        public class Reader
        {
            internal int m_Index;

            internal Reader(int index)
            {
                m_Index = index;
            }
        };

        private class Wav
        {
            public string m_Path = "";
            public float m_SampleRate = 44100;

            public float[] m_Data = new float[1];
        };

        private class WavState
        {
            public float m_Amplitude = 0;
            public float m_Frequency = 44100;

            public float m_Position = 0;
        };

        [StructLayout(LayoutKind.Explicit)]
        struct SFloatCastToBytes
        {
            [FieldOffset(0)] public float m_Float;
            [FieldOffset(0)] public byte m_Byte0;
            [FieldOffset(1)] public byte m_Byte1;
            [FieldOffset(2)] public byte m_Byte2;
            [FieldOffset(3)] public byte m_Byte3;
        };

        private object m_Mutex = new object();
        List<Wav> m_Wavs = new List<Wav>();
        List<WavState> m_WavStates = new List<WavState>();
        List<int> m_ActiveWavs = new List<int>();
        float[] m_CircularBuffer = new float[c_CircularBufferSize];
        int m_CircularHead = 0;
        float m_OverallVolume = 1.0f;

        float m_TimeConstantRMS = 0;
        double m_RMSExp = 1;
        double m_MeanSquareSum = 0;

        public WavPlayer()
        {
            TimeConstantRMS = 0.5f;
        }

        public float OverallVolume
        {
            set { m_OverallVolume = value; }
            get { return m_OverallVolume; }
        }

        public int NumWavs
        {
            get {  return m_Wavs.Count; }
        }

        public float TimeConstantRMS
        {
            get { return m_TimeConstantRMS; }
            set
            {
                m_TimeConstantRMS = value;
                m_RMSExp = Math.Exp(-1.0f / (c_SampleRate * m_TimeConstantRMS));
            }
        }

        public float GetRMS()
        {
            return (float)Math.Sqrt(m_MeanSquareSum);
        }

        private void UpdateActiveWavs()
        {
            lock (m_Mutex)
            {
                m_ActiveWavs.Clear();
                for (int i = 0; i < m_Wavs.Count; i++)
                {
                    if (m_WavStates[i].m_Amplitude > 0)
                        m_ActiveWavs.Add(i);
                }
            }
        }

        public int AddWav(string filepath)
        {
            for(int i = 0; i < m_Wavs.Count; i++)
            {
                if (m_Wavs[i].m_Path.Equals(filepath))
                    return i;
            }

            int index;
            using (WaveFileReader reader = new WaveFileReader(filepath))
            {
                long sample_count = reader.SampleCount;
                float total_time = (float)reader.TotalTime.TotalSeconds;
                float sample_rate = sample_count / total_time;

                List<float> all_data = new List<float>();
                while (true)
                {
                    float[] data = reader.ReadNextSampleFrame();
                    if (data == null)
                        break;
                    all_data.AddRange(data);
                }

                lock (m_Mutex)
                {
                    Wav wav = new Wav();
                    WavState wav_state = new WavState();

                    wav.m_Path = filepath;
                    wav.m_SampleRate = sample_rate;
                    wav.m_Data = all_data.ToArray();

                    wav_state.m_Amplitude = 0;
                    wav_state.m_Frequency = sample_rate;

                    index = m_Wavs.Count;
                    m_Wavs.Add(wav);
                    m_WavStates.Add(wav_state);
                }
            }
            UpdateActiveWavs();
            return index;
        }

        public void SetWavPlayback(int handle, float amplitude, float pitch)
        {
            Wav wav = m_Wavs[handle];
            if (pitch <= 0 || pitch > 1000.0f)
            {
                amplitude = 0.0f;
                pitch = 1.0f;
            }

            WavState wav_state = m_WavStates[handle];
            wav_state.m_Amplitude = amplitude;
            wav_state.m_Frequency = pitch * wav.m_SampleRate;
            UpdateActiveWavs();
        }

        public float GetWavPlaybackAmplitude(int handle)
        {
            return m_WavStates[handle].m_Amplitude;
        }

        public WaveFormat WaveFormat 
        {  
            get {
                return WaveFormat.CreateIeeeFloatWaveFormat((int)c_SampleRate, 1);
            }
        }

        public Reader ReaderCreate()
        {
            return new Reader(m_CircularHead);
        }

        public void ReaderAdvance(Reader reader, float dt)
        {
            int step = (int)(dt * c_SampleRate);
            reader.m_Index = Math.Min(reader.m_Index + step, m_CircularHead);
        }

        public void ReaderRead(Reader reader, float[] out_data)
        {
            int src_index = reader.m_Index - out_data.Length;
            for (int i = 0; i < out_data.Length; i++)
                out_data[i] = m_CircularBuffer[(src_index + i) & c_CircularBufferMask];
        }

        private static float SampleWav(Wav wav, float position)
        {
            float floor_pos = (float)Math.Floor(position);
            float residual = position - floor_pos;
            int index = (int)floor_pos;
            float v_from = wav.m_Data[index];
            float v_to = wav.m_Data[index + 1 < wav.m_Data.Length ? index + 1 : 0];

            return v_from + residual * (v_to - v_from);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            lock (m_Mutex)
            {
                float dt = 1.0f / c_SampleRate;

                SFloatCastToBytes float_to_bytes = new SFloatCastToBytes();

                int num_floats = count / sizeof(float);
                int buffer_dst_index = offset;
                for (int i = 0; i < num_floats; i++)
                {
                    float v = 0.0f;
                    for (int j = 0; j < m_ActiveWavs.Count; j++)
                    {
                        int index = m_ActiveWavs[j];
                        Wav wav = m_Wavs[index];
                        WavState wav_state = m_WavStates[index];

                        wav_state.m_Position += dt * wav_state.m_Frequency;
                        while (wav_state.m_Position >= wav.m_Data.Length)
                            wav_state.m_Position -= wav.m_Data.Length;
                        float v_wav = SampleWav(wav, wav_state.m_Position);
                        v += v_wav * wav_state.m_Amplitude;
                    }
                    float scaled_value = v * m_OverallVolume;

                    int dst_index = m_CircularHead++;
                    m_CircularBuffer[dst_index & c_CircularBufferMask] = scaled_value;

                    float sq_v = scaled_value * scaled_value;
                    m_MeanSquareSum = m_MeanSquareSum * m_RMSExp + sq_v * (1.0 - m_RMSExp);

                    float_to_bytes.m_Float = scaled_value;
                    buffer[buffer_dst_index++] = float_to_bytes.m_Byte0;
                    buffer[buffer_dst_index++] = float_to_bytes.m_Byte1;
                    buffer[buffer_dst_index++] = float_to_bytes.m_Byte2;
                    buffer[buffer_dst_index++] = float_to_bytes.m_Byte3;
                }
                return count;
            }
        }
    }
}
