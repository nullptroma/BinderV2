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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BinderV2.MVVM.Views.Triggers
{
    /// <summary>
    /// Логика взаимодействия для TimerTriggerControl.xaml
    /// </summary>
    public partial class TimerTriggerControl : UserControl
    {
        public TimerTriggerControl()
        {
            InitializeComponent();
        }

        private void EditNameButton_Click(object sender, RoutedEventArgs e)
        {
            StartEditName();
        }

        private void StartEditName()
        {
            labelName.Visibility = Visibility.Collapsed;
            textBoxName.Visibility = Visibility.Visible;
            textBoxName.SelectionStart = textBoxName.Text.Length;
            textBoxName.Focus();
        }

        private void EndEditName()
        {
            labelName.Visibility = Visibility.Visible;
            textBoxName.Visibility = Visibility.Collapsed;
            labelName.Focus();
        }

        private void textBoxName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EndEditName();
            }
        }
    }
}
