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
            Initializer.Initialize();
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
                await Task.Delay(1);
            }
        }

        private async void BindsScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange != 0)
                await ScrollDown(BindsScrollViewer);
        }

        private async void BindsScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            int length = e.Delta * -1;
            double step = length / 12;
            for (int i = 0; i < 12; i++)
            {
                BindsScrollViewer.Dispatcher.Invoke(() => BindsScrollViewer.ScrollToVerticalOffset(BindsScrollViewer.VerticalOffset + step));
                await Task.Delay(1);
            }
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
            ProgramSettings.RuntimeSettings.MainWindowSize = new Size(Width, Height);
            Hooks.Mouse.MouseHook.UnInstallHook();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Test.RunTest();
        }

        
    }
}
