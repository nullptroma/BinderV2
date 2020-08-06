using System;
using BinderV2.Settings.Visuals;
using BinderV2.Utilities;
using Newtonsoft.Json;
using System.IO;
using System.Windows;
using System.Windows.Documents;

namespace BinderV2.Settings
{
    public class ProgramSettings
    {
        private bool startWithWindows = false;
        public VisualsSettings VisualSettings { get; private set; }
        public bool HideOnStart { get; set; }
        public bool AutoLoadBinds { get; set; }
        public string AutoLoadBindsPath { get; set; }
        public bool SaveMainWindowSize { get; set; }
        public Size MainWindowSize { get; set; }
        [JsonIgnore] private string lastBindsPath = "";
        [JsonIgnore] public string LastBindsPath { get { return lastBindsPath; } set { lastBindsPath = value; } }
        public bool StartWithWindows
        {
            get { return startWithWindows; }
            set
            {
                startWithWindows = value;
                if (startWithWindows)
                    AutoRun.RegisterAutoRun();
                else
                    AutoRun.UnRegisterAutoRun();
            }
        }

        private ProgramSettings()
        {
            VisualSettings = new VisualsSettings();
        }

        public static readonly string SaveSettingsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BinderV2";
        public static ProgramSettings runtimeSettings { get; private set; }

        static ProgramSettings()//начинаем тут
        {
            LoadSettings();
        }

        ~ProgramSettings()
        {
            SaveSettings();
        }

        private static void SaveSettings()
        {
            JsonUtilities.SerializeToFile(runtimeSettings, ProgramSettings.SaveSettingsDirectory + @"\settings.txt");
        }

        private static void LoadSettings()
        {
            try
            {
                runtimeSettings = JsonUtilities.Deserialize<ProgramSettings>(File.ReadAllText(ProgramSettings.SaveSettingsDirectory + @"\settings.txt"));
            }
            catch{ runtimeSettings = new ProgramSettings(); }
            try
            {
                VisualsSettings.ApplyVisuals(runtimeSettings.VisualSettings);
            }
            catch { Reset(); }
        }


        public static void Reset()
        {
            runtimeSettings = new ProgramSettings();
            VisualsSettings.ApplyVisuals(runtimeSettings.VisualSettings);
            SaveSettings();
        }
    }
}
