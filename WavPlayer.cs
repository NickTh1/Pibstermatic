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
            public int m_IndexLayer = 0;
            public float m_SampleRate = 44100;

            public float[] m_Data = new float[1];
        };

        private struct SWavPlaybackParams
        {
            public float m_Amplitude = 0;
            public float m_Frequency = 44100;
        }

        private struct SScape
        {
            public SWavPlaybackParams[] m_WavPlaybackParams = new SWavPlaybackParams[0];
        }

        private class WavState
        {
            public double m_Position = 0;
        }

        private class InterpolatedWav
        {
            public SWavPlaybackParams m_From;
            public SWavPlaybackParams m_To;
            public int m_Index;
        }

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
        SScape m_PendingScape = new SScape();
        List<SScape> m_QueuedScapes = new List<SScape>();
        List<WavState> m_WavStates = new List<WavState>();
        List<InterpolatedWav> m_TmpInterpolatedWavs = new List<InterpolatedWav>();
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

        public int AddWav(int index_layer, string filepath)
        {
            for(int i = 0; i < m_Wavs.Count; i++)
            {
                Wav wav = m_Wavs[i];
                if (wav.m_IndexLayer == index_layer && wav.m_Path.Equals(filepath))
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
                    SWavPlaybackParams wav_playback_params = new SWavPlaybackParams();
                    wav_state.m_Position = 0;

                    wav.m_Path = filepath;
                    wav.m_IndexLayer = index_layer;
                    wav.m_SampleRate = sample_rate;
                    wav.m_Data = all_data.ToArray();

                    wav_playback_params.m_Amplitude = 0;
                    wav_playback_params.m_Frequency = sample_rate;

                    index = m_Wavs.Count;
                    m_Wavs.Add(wav);
                    m_WavStates.Add(wav_state);

                    SWavPlaybackParams[] new_params = new SWavPlaybackParams[index + 1];
                    Array.Copy(m_PendingScape.m_WavPlaybackParams, new_params, m_PendingScape.m_WavPlaybackParams.Length);
                    new_params[index] = wav_playback_params;
                    m_PendingScape.m_WavPlaybackParams = new_params;

                    m_TmpInterpolatedWavs.Add(new InterpolatedWav());

                    m_QueuedScapes.Clear();       // Wrong size, so just clear.
                }
            }
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

            ref SWavPlaybackParams playback_params = ref m_PendingScape.m_WavPlaybackParams[handle];
            playback_params.m_Amplitude = amplitude;
            playback_params.m_Frequency = pitch * wav.m_SampleRate;
        }

        public void CommitScape()
        {
            SScape scape_copy = new SScape();
            scape_copy.m_WavPlaybackParams = new SWavPlaybackParams[m_PendingScape.m_WavPlaybackParams.Length];
            Array.Copy(m_PendingScape.m_WavPlaybackParams, scape_copy.m_WavPlaybackParams, m_PendingScape.m_WavPlaybackParams.Length);

            lock (m_Mutex)
            {
                m_QueuedScapes.Add(scape_copy);
            }
        }

        public float GetWavPlaybackAmplitude(int handle)
        {
            return m_PendingScape.m_WavPlaybackParams[handle].m_Amplitude;
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
            if (index >= wav.m_Data.Length)
                index -= wav.m_Data.Length;
            float v_from = wav.m_Data[index];
            float v_to = wav.m_Data[index + 1 < wav.m_Data.Length ? index + 1 : 0];

            return v_from + residual * (v_to - v_from);
        }

        static float Lerp(float t, float op1, float op2)
        {
            return op2 * t + op1 * (1.0f - t);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            lock (m_Mutex)
            {
                float dt = 1.0f / c_SampleRate;

                SFloatCastToBytes float_to_bytes = new SFloatCastToBytes();

                int num_floats = count / sizeof(float);
                int buffer_dst_index = offset;
                int num_remaining_floats = num_floats;

                int num_queued = m_QueuedScapes.Count;
                int num_frames = Math.Max(num_queued, 1);

                for (int i = 0; i < num_frames; i++)
                {
                    int num_remaining_frames = num_frames - i;
                    int num_samples_this_frame = num_remaining_floats / num_remaining_frames;

                    SScape from = (num_queued > 0) ? m_QueuedScapes[Math.Min(i, num_queued - 1)] : m_PendingScape;
                    SScape to = (num_queued > 0) ? m_QueuedScapes[Math.Min(i + 1, num_queued-1)] : m_PendingScape;

                    int num_active_wavs = 0;
                    List<InterpolatedWav> interpolated_wavs = m_TmpInterpolatedWavs;
                    for( int j = 0; j < from.m_WavPlaybackParams.Length; j++)
                    {
                        SWavPlaybackParams wav_from = from.m_WavPlaybackParams[j];
                        SWavPlaybackParams wav_to = to.m_WavPlaybackParams[j];

                        if (wav_from.m_Amplitude > 0 || wav_to.m_Amplitude > 0)
                        {
                            InterpolatedWav interpolated_wav = interpolated_wavs[num_active_wavs++];
                            interpolated_wav.m_From = wav_from;
                            interpolated_wav.m_To = wav_to;
                            interpolated_wav.m_Index = j;
                            if (interpolated_wav.m_From.m_Amplitude <= 0)
                                interpolated_wav.m_From.m_Frequency = interpolated_wav.m_To.m_Frequency;
                            if (interpolated_wav.m_To.m_Amplitude <= 0)
                                interpolated_wav.m_To.m_Frequency = interpolated_wav.m_From.m_Frequency;
                        }
                    }

                    float oo_num_samples_this_frame = 1.0f / num_samples_this_frame;
                    for( int j = 0; j < num_samples_this_frame; j++)
                    {
                        float lerp = (float)j * oo_num_samples_this_frame;

                        float v = 0.0f;
                        for (int k = 0; k < num_active_wavs; k++)
                        {
                            InterpolatedWav interpolated_wav = interpolated_wavs[k];
                            int index = interpolated_wav.m_Index;
                            Wav wav = m_Wavs[index];
                            WavState wav_state = m_WavStates[index];
                            float frequency = Lerp(lerp, interpolated_wav.m_From.m_Frequency, interpolated_wav.m_To.m_Frequency);
                            float amplitude = Lerp(lerp, interpolated_wav.m_From.m_Amplitude, interpolated_wav.m_To.m_Amplitude);

                            wav_state.m_Position += dt * frequency;
                            while (wav_state.m_Position >= wav.m_Data.Length)
                                wav_state.m_Position -= wav.m_Data.Length;
                            float v_wav = SampleWav(wav, (float)wav_state.m_Position);
                            v += v_wav * amplitude;
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

                    num_remaining_floats -= num_samples_this_frame;
                    num_remaining_frames--;
                }

                if (num_queued > 1)
                {
                    m_QueuedScapes[0] = m_QueuedScapes[num_queued - 1];
                    m_QueuedScapes.RemoveRange(1, num_queued - 1);
                }
                return count;
            }
        }
    }
}
