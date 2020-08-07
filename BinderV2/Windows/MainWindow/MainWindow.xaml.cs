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

namespace BinderV2.Windows.Main
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
            if (ProgramSettings.runtimeSettings.HideOnStart)
                HideWindow();
            if (ProgramSettings.runtimeSettings.SaveMainWindowSize)
            {
                Application.Current.MainWindow.Width = ProgramSettings.runtimeSettings.MainWindowSize.Width;
                Application.Current.MainWindow.Height = ProgramSettings.runtimeSettings.MainWindowSize.Height;
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


        private async Task ScrollDown(ScrollViewer sv)
        {
            for (var i = sv.ContentVerticalOffset; i < sv.ScrollableHeight; i += ((sv.ScrollableHeight - i) / 10)+1)
            {
                sv.ScrollToVerticalOffset(i);
                await Task.Delay(1).ConfigureAwait(true);
            }
        }

        private async void BindsScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if(e.ExtentHeightChange!=0)
                await ScrollDown(BindsScrollViewer).ConfigureAwait(true);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //e.Cancel = true;
            //HideWindow();
        }

        private void TaskBarIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ShowWindow();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            BinderV2.Settings.ProgramSettings.runtimeSettings.MainWindowSize = new Size(Width, Height);
            
        }
    }
}
