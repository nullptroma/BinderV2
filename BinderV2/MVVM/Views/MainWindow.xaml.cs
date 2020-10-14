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
using CustomScrollViewerLogic;
using System.Runtime.Remoting.Channels;

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
            ShowWindow();
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
            e.Cancel = true;
            HideWindow();
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
            this.Visibility = Visibility.Collapsed;
            ShowInTaskbar = false;
            Hooks.Mouse.MouseHook.MouseMove -= OnCursorPositonChanged;
        }

        private void ShowWindow()
        {
            WindowState = WindowState.Normal;
            this.Visibility = Visibility.Visible;
            ShowInTaskbar = true;
            Hooks.Mouse.MouseHook.MouseMove += OnCursorPositonChanged;
        }

        private void OnCursorPositonChanged(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            CursorPosition.Content = "Позиция курсора " + System.Windows.Forms.Cursor.Position;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ProgramSettings.RuntimeSettings.MainWindowSize = new Size(Width, Height);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Test.RunTest();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BindsScrollViewer.Tag = true;
        }
    }
}
