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
using Trigger.Types;
using System.Windows.Media;
using BinderV2.Settings;
using InterpreterScripts;
using BinderV2.Classes;
using BinderV2.MVVM.ViewModels;

namespace BinderV2.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml 
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            if (ProgramSettings.RuntimeSettings.HideOnStart)
                HideWindow();
            if (ProgramSettings.RuntimeSettings.SaveMainWindowSize)
            {
                Application.Current.MainWindow.Width = ProgramSettings.RuntimeSettings.MainWindowSize.Width;
                Application.Current.MainWindow.Height = ProgramSettings.RuntimeSettings.MainWindowSize.Height;
            }
        }

        private void ShowWindowButton_Click(object sender, RoutedEventArgs e)
        {
            ShowWindow();
        }

        private void HideWindowButton_Click(object sender, RoutedEventArgs e)
        {
            HideWindow();
        }
 

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //e.Cancel = true;
            //HideWindow();
        }

        private void TaskBarIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                ShowWindow();
            else
                HideWindow();
        }

        private void HideWindow()
        {
            WindowState = WindowState.Minimized;
            ShowInTaskbar = false;
        }

        private void ShowWindow()
        {
            WindowState = WindowState.Normal;
            ShowInTaskbar = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ProgramSettings.RuntimeSettings.MainWindowSize = new Size(Width, Height);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Test.RunTest();
        }

        
    }
}
