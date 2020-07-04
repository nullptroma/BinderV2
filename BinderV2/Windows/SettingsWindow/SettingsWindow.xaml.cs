﻿using BinderV2.Trigger.Types;
using BinderV2.Windows.SettingsWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BinderV2.Windows.Settings
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = new SettingsViewModel();

            
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            BaseTrigger.EnableAllTriggers = true;
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            BaseTrigger.EnableAllTriggers = false;
            
        }

        private void AutoLoadPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            AutoLoadPath.ScrollToHorizontalOffset(AutoLoadPath.Text.Length);
        }
    }
}
