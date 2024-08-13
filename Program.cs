
using System.Diagnostics;

namespace WaveMix
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();

            using (Process p = Process.GetCurrentProcess())
                p.PriorityClass = ProcessPriorityClass.AboveNormal;

            Settings? settings = SettingsIO.Read();
            if (settings == null)
                settings = new Settings();

            string path_file = "";
            if (args.Length > 0)
                path_file = args[0];
            else
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "Engine files (*.scl)|*.scl";
                if (settings.PathEngine.Length > 0)
                {
                    string filename = Path.GetFileName(settings.PathEngine);
                    dlg.FileName = filename;
                    string? folder = Path.GetDirectoryName(settings.PathEngine);
                    if (folder != null)
                        dlg.InitialDirectory = folder;
                }

                DialogResult result = dlg.ShowDialog();
                if (result != DialogResult.OK)
                    return;
                path_file = dlg.FileName;
            }

            settings.PathEngine = path_file;
            SettingsIO.Write(settings);

            Application.Run(new MainForm(settings));
        }
    }
}