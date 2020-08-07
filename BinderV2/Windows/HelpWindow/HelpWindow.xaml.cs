using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace BinderV2.Windows.Help
{
    /// <summary>
    /// Логика взаимодействия для HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
            DataContext = new HelpViewModel();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int count = FunctionsHelp.Items.Count;
            Thickness windowBorder = (Thickness)App.Current.Resources["WindowBorderThickness"];
            double widthForOne = (this.Width / count)-((windowBorder.Left + windowBorder.Right + 5)*1.0 /count);
            for (int i = 0; i < count; i++)
            {
                var buf = ((Control)FunctionsHelp.Items[i]);
                buf.Width = widthForOne;
            }
        }
    }
}
