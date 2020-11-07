﻿using BinderV2.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinderV2.MVVM.Models.MainModels
{
    class WindowsManager
    {
        private SettingsWindow SettingsWindow;
        private HelpWindow HelpWindow = new HelpWindow();
        private RecordWindow RecordWindow;

        public void OpenSettingsWindow()
        {
            if (SettingsWindow != null)
                SettingsWindow.Close();
            SettingsWindow = new SettingsWindow();
            SettingsWindow.Show();
        }

        public void OpenHelpWindow()
        {
            HelpWindow.Show();
        }

        public void OpenRecordWindow()
        {
            if (RecordWindow != null)
                RecordWindow.Close();
            RecordWindow = new RecordWindow();
            RecordWindow.Show();
        }

        public WindowsManager()
        {
            HelpWindow.Closing += (sender, e) => { e.Cancel = true; HelpWindow.Hide(); };
        }
    }
}
