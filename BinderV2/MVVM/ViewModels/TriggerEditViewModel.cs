using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Trigger.Types;
using Trigger.Tools;
using BindModel;
using BinderV2.Commands;
using BinderV2.MVVM.Views;
using BinderV2.MVVM.ViewModels.Triggers;
using BinderV2.MVVM.Models;

namespace BinderV2.MVVM.ViewModels
{
    class TriggerEditViewModel : BaseViewModel
    {
        #region TriggersManager
        TriggersManager triggersManager;
        public ObservableCollection<BaseTriggerViewModel> Triggers { get { return triggersManager.Triggers; } }
        public string SelectedTriggerScript
        {
            get { return triggersManager.SelectedTriggerScript; }
            set
            {
                triggersManager.SelectedTriggerScript = value;
                OnPropertyChanged("SelectedTriggerScript");
            }
        }

        private RelayCommand createTriggerCommand;
        public RelayCommand CreateTriggerCommand
        {
            get
            {
                return createTriggerCommand ??
                  (createTriggerCommand = new RelayCommand(obj =>
                  {
                      triggersManager.CreateNewTrigger();
                  }));
            }
        }

        private RelayCommand removeTriggerCommand;
        public RelayCommand RemoveTriggerCommand
        {
            get
            {
                return removeTriggerCommand ??
                  (removeTriggerCommand = new RelayCommand(obj =>
                  {
                      if (!(obj is BaseTriggerViewModel))
                          return;
                      BaseTriggerViewModel bindElement = (BaseTriggerViewModel)obj;
                      if (MessageBox.Show("Удалить триггер \"" + bindElement.Trigger.Name + "\"?", "Вы уверены?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                      {
                          if (!triggersManager.RemoveTrigger((BaseTriggerViewModel)obj))
                              MessageBox.Show("Ошибка удаления");
                      }
                  }));
            }
        }

        private RelayCommand selectTriggerCommand;
        public RelayCommand SelectTriggerCommand
        {
            get
            {
                return selectTriggerCommand ??
                  (selectTriggerCommand = new RelayCommand(obj =>
                  {
                      if (obj is BaseTriggerViewModel)
                          triggersManager.SelectedTrigger = (BaseTriggerViewModel)obj;
                  }));
            }
        }

        private RelayCommand saveTriggerScriptCommand;
        public RelayCommand SaveTriggerScriptCommand
        {
            get
            {
                return saveTriggerScriptCommand ??
                  (saveTriggerScriptCommand = new RelayCommand(obj =>
                  {
                      if (obj != null)
                      {
                          triggersManager.SaveScriptToSelectedBind(obj.ToString());
                          MessageBox.Show("Сохранено", "Успех");
                      }
                  }));
            }
        }

        public void OnTriggersManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }
        #endregion

        public TriggerEditViewModel(Bind bind)
        {
            triggersManager = new TriggersManager(bind);
            triggersManager.PropertyChanged += OnTriggersManagerPropertyChanged;
        }

        public override event PropertyChangedEventHandler PropertyChanged;
        public override void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
