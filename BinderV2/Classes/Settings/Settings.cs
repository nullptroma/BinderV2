using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BinderV2.Settings.Visuals;
using BinderV2.Utilities;
using Newtonsoft.Json;
using System.IO;

namespace BinderV2.Settings
{
    public class ProgramSettings
    {
        private bool startWithWindows = false;
        private bool hideOnStart = false;
        private bool autoLoadBinds = false;
        private string autoLoadBindsPath = "";
        private string lastBindsPath = "";
        public VisualsSettings VisualSettings { get; set; }
        public bool HideOnStart { get { return hideOnStart; } set { hideOnStart = value; } }
        public bool AutoLoadBinds { get { return autoLoadBinds; } set { autoLoadBinds = value; } }
        public string AutoLoadBindsPath { get { return autoLoadBindsPath; } set { autoLoadBindsPath = value; } }
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
            VisualsSettings.ApplyVisuals(runtimeSettings.VisualSettings);
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
            catch { SaveSettings(); }
        }

        public static void Initialize()
        {
            
        }

        public static void Reset()
        {
            runtimeSettings = new ProgramSettings();
            SaveSettings();
        }
    }
}
