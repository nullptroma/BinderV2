using Trigger.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BindModel;
using BinderV2.MVVM.ViewModels;

namespace BinderV2.MVVM.Views
{
    public partial class TriggersEditWindow : Window
    {
        public TriggersEditWindow(Bind bind)
        {
            InitializeComponent();
            StateChanged += CustomizedWindow.WindowStyle.Window_StateChanged;
            DataContext = new TriggerEditViewModel(bind);//В TriggerEditViewModel мне нужны и bind, и его закрытое поле triggers
            Title = "Редактирование " + bind.Name;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TriggerScrollViewer.Tag = true;
        }
    }
}
