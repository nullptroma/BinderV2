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
using BinderV2.MVVM.ViewModels;

namespace BinderV2.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для RecordWindow.xaml
    /// </summary>
    public partial class RecordWindow : Window
    {
        public RecordWindow()
        {
            InitializeComponent();
            DataContext = new RecordViewModel();
        }

        private void TextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            scriptBox.Focus();
        }
    }
}
