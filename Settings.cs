using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WaveMix
{
    public class Settings
    {
        public string PathEngine = "";
        public int Stroke = 4;
        public int Cylinders = 4;
        public float IdleRPM = 1000;
        public float OverallVolume = 1;
    }

    internal class SettingsIO
    {
        public static string PathSettings
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Pibstermatic/SoundSettings.ini");
            }
        }

        public static Settings? Read()
        {
            try
            {
                string path = PathSettings;

                string json_text = File.ReadAllText(path);
                Settings? settings = JsonConvert.DeserializeObject<Settings>(json_text);
                return settings;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void Write(Settings settings)
        {
            try
            {
                string path = PathSettings;

                string json_text = JsonConvert.SerializeObject(settings, Formatting.Indented);

                string? folder = Path.GetDirectoryName(path);
                if (folder != null)
                    Directory.CreateDirectory(folder);

                File.WriteAllText(path, json_text);
            }
            catch (Exception)
            {
            }
        }
    };
}
