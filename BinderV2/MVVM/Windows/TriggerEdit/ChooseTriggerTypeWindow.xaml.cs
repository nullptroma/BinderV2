using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Trigger.Types;
using BinderV2.Commands;

namespace BinderV2.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для ChooseTriggerTypeWindow.xaml
    /// </summary>
    public partial class ChooseTriggerTypeWindow : Window
    {
        public TriggerType SelectedType { get; private set; }

        public RelayCommand TriggerTypeSelectCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    SelectedType = (TriggerType)obj;
                    this.Close();
                });
            }
        }

        public ChooseTriggerTypeWindow()
        {
            InitializeComponent();
            SelectedType = TriggerType.None;
            DataContext = this;
        }
    }
}
