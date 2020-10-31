using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Trigger.Types;
using BinderV2.Commands;
using BinderV2.MVVM.ViewModels.Triggers;

namespace BinderV2.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для ChooseTriggerTypeWindow.xaml
    /// </summary>
    public partial class ChooseTriggerTypeWindow : Window
    {
        public BaseTriggerViewModel ResultBaseViewModel { get; private set; }

        public RelayCommand TriggerTypeSelectCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    ResultBaseViewModel = (BaseTriggerViewModel)obj;
                    this.Close();
                });
            }
        }

        public ChooseTriggerTypeWindow()
        {
            InitializeComponent();
            StateChanged += CustomizedWindow.WindowStyle.Window_StateChanged;
            DataContext = this;
        }
    }
}
