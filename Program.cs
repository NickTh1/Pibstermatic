
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

            Settings? settings = SettingsIO.Read();
            if (settings == null)
                settings = new Settings();

            if (args.Length > 0)
                settings.PathEngine = args[0];
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
                settings.PathEngine = dlg.FileName;
            }
            SettingsIO.Write(settings);

            Application.Run(new MainForm(settings));
        }
    }
}