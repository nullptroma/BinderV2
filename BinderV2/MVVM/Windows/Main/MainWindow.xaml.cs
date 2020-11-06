using System;
using System.Windows;
using BinderV2.Settings;
using BinderV2.MVVM.ViewModels;
using System.Windows.Threading;

namespace BinderV2.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml 
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer updateCursorTimer;

        public MainWindow()
        {
            InitializeComponent();
            StateChanged += CustomizedWindow.WindowStyle.Window_StateChanged;
            DataContext = new MainViewModel();
            
            if (ProgramSettings.RuntimeSettings.SaveMainWindowSize)
            {
                System.Windows.Application.Current.MainWindow.Width = ProgramSettings.RuntimeSettings.MainWindowSize.Width;
                System.Windows.Application.Current.MainWindow.Height = ProgramSettings.RuntimeSettings.MainWindowSize.Height;
            }

            updateCursorTimer = new DispatcherTimer() { Interval = new TimeSpan(0,0,0,0,20) };
            updateCursorTimer.Tick += (sender, e) => { CursorPosition.Content = "Позиция курсора " + System.Windows.Forms.Cursor.Position; };
            updateCursorTimer.Start();
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
            updateCursorTimer.Stop();
        }

        private void ShowWindow()
        {
            WindowState = WindowState.Normal;
            this.Visibility = Visibility.Visible;
            ShowInTaskbar = true;
            updateCursorTimer.Start();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Hooks.Mouse.MouseHook.UnInstallHook();
            ProgramSettings.RuntimeSettings.MainWindowSize = new Size(Width, Height);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Test.RunTest();
        }

        private void ItemsControl_Loaded(object sender, RoutedEventArgs e)
        {
            BindsScrollViewer.Tag = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (ProgramSettings.RuntimeSettings.HideOnStart)
                HideWindow();
        }
    }
}
