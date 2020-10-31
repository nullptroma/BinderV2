using BinderV2.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trigger.Types;

namespace BinderV2.MVVM.ViewModels.Triggers
{
    public abstract class BaseTriggerViewModel : BaseViewModel
    {
        private bool isSelected = false;
        public bool IsSelected { get { return isSelected; } set { isSelected = value; OnPropertyChanged("IsSelected"); } }
        public bool IsEnabled { get { return Trigger.EnableTrigger; } set { Trigger.EnableTrigger = value; OnPropertyChanged("IsEnabled"); } }
        public string Name { get { return Trigger.Name; } set { Trigger.Name = value; OnPropertyChanged("Name"); } }
        public string TypeDescription { get { return Trigger.TypeDescription; } }
        public BaseTrigger Trigger { get; private set; }

        private RelayCommand changeEnableCommand;
        public RelayCommand ChangeEnableCommand
        {
            get
            {
                return changeEnableCommand ??
                  (changeEnableCommand = new RelayCommand(obj =>
                  {
                      IsEnabled = !IsEnabled;
                  }));
            }
        }

        public BaseTriggerViewModel(BaseTrigger trigger)
        {
            Trigger = trigger;
        }

        public override event PropertyChangedEventHandler PropertyChanged;
        public override void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
