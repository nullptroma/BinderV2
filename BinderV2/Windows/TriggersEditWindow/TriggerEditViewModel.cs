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
using BinderV2.WpfControls.Triggers;


namespace BinderV2.Windows.TriggersEdit
{
    class TriggerEditViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<ITriggerControl> triggersControls { get; set; }
        private ITriggerControl selectedTrigger;
        private Bind bind;
        private string scriptTextBoxText = "";
        public string ScriptTextBoxText
        {
            get { return scriptTextBoxText; }
            set 
            { 
                scriptTextBoxText = value;
                OnPropertyChanged("ScriptTextBoxText");
            }
        }

        
        

        public TriggerEditViewModel(Bind bind)
        {
            this.bind = bind;
            triggersControls = new ObservableCollection<ITriggerControl>();//создаём коллекцию контролов
            AddTriggerControls();//добавляем контроллы из триггеров
        }

        private void AddTriggerControls()
        {
            foreach (BaseTrigger bt in bind.Triggers)
            {
                var control = TriggerUtility.GetControlFromTrigger(bt);
                triggersControls.Add((ITriggerControl)control);
            }
            OnPropertyChanged("triggersControls");
            OnPropertyChanged("triggersControls");
            
        }


        private RelayCommand createTriggerCommand;
        public RelayCommand CreateTriggerCommand
        {
            get
            {
                return createTriggerCommand ??
                  (createTriggerCommand = new RelayCommand(obj =>
                  {
                      
                      ChooseTriggerTypeWindow ctw = new ChooseTriggerTypeWindow();
                      ctw.ShowDialog();
                      if (ctw.SelectedType == TriggerType.None)
                          return;
                      BaseTrigger newTrigger = TriggerUtility.GetTriggerFromTriggerType("Новый триггер", ctw.SelectedType);
                      bind.Triggers.Add(newTrigger);

                      Control triggerControl = TriggerUtility.GetControlFromTrigger(newTrigger);

                      triggersControls.Add((ITriggerControl)triggerControl);
                      OnPropertyChanged("triggersControls");
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
                      ITriggerControl triggerElement = (ITriggerControl)obj;
                      if (MessageBox.Show("Удалить триггер \"" + triggerElement.GetTrigger().Name + "\"?", "Вы уверены?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                      {
                          if (!bind.Triggers.Remove(triggerElement.GetTrigger()))
                              MessageBox.Show("Ошибка удаления");

                          triggersControls.Remove(triggerElement);
                          OnPropertyChanged("triggersControls");

                          ScriptTextBoxText = "";
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
                      if (triggersControls.Contains(obj))
                      {
                          foreach (ITriggerControl trigEl in triggersControls)
                              trigEl.Selected = false;
                          var currentElement = (ITriggerControl)obj;
                          selectedTrigger = currentElement;
                          currentElement.Selected = true;

                          ScriptTextBoxText = selectedTrigger.GetTrigger().Script;
                      }
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
                      if (selectedTrigger != null)
                      {
                          selectedTrigger.GetTrigger().Script = obj.ToString();
                          MessageBox.Show("Сохранено");
                      }
                      else
                      {
                          MessageBox.Show("Выберите триггер", "Ошибка");
                      }
                  }));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
