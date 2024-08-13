using System;
using NAudio.Wave;
using NAudio.MediaFoundation;
using OxyPlot;

public class WavFileData
{
    public float m_SampleRate;
    public float[] m_Data = new float[0];
};

public class Utils
{
    static byte LerpColorComponent(float t, int op1, int op2)
    {
        return (byte)Math.Round((float)op1 * (1.0f - t) + (float)op2 * t);
    }

    public static OxyColor Lerp(float t, OxyColor op1, OxyColor op2)
    {
        return OxyColor.FromArgb(LerpColorComponent(t, op1.A, op2.A), LerpColorComponent(t, op1.R, op2.R), LerpColorComponent(t, op1.G, op2.G), LerpColorComponent(t, op1.B, op2.B));
    }


    public static WavFileData ReadWavFile(string filepath)
    {
        using (WaveFileReader reader = new WaveFileReader(filepath))
        {
            WavFileData wav_file_data = new WavFileData();

            long sample_count = reader.SampleCount;
            float total_time = (float)reader.TotalTime.TotalSeconds;
            wav_file_data.m_SampleRate = sample_count / total_time;

            List<float> all_data = new List<float>();
            while (true)
            {
                float[] data = reader.ReadNextSampleFrame();
                if (data == null)
                    break;
                all_data.AddRange(data);
            }
            wav_file_data.m_Data = all_data.ToArray();
            return wav_file_data;
        }
    }
}
