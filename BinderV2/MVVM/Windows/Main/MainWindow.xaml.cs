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
        readonly DispatcherTimer updateCursorTimer;

        public MainWindow()
        {
            InitializeComponent();
            StateChanged += CustomizedWindow.WindowStyle.Window_StateChanged;
            DataContext = new MainViewModel();
            
            if (ProgramSettings.RuntimeSettings.SaveMainWindowSize)
            {
                Width = ProgramSettings.RuntimeSettings.MainWindowSize.Width;
                Height = ProgramSettings.RuntimeSettings.MainWindowSize.Height;
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
            if (ProgramSettings.RuntimeSettings.CloseEqualsHide)
            {
                e.Cancel = true;
                HideWindow();
            }
            else
            {
                App.Current.Shutdown();
            }
        }

        private void TaskBarIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
                HideWindow();
            else
                ShowWindow();
        }

        private void HideWindow()
        {
            updateCursorTimer.Stop();
            this.Hide();
        }

        private void ShowWindow()
        {
            updateCursorTimer.Start();
            this.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Hooks.Mouse.MouseHook.UnInstallHook();
            ProgramSettings.RuntimeSettings.MainWindowSize = new Size(Width, Height);
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
