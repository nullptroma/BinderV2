using BinderV2.MVVM.ViewModels.Triggers;
using BinderV2.MVVM.Views;
using BindModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trigger.Types;
using Trigger.Tools;
using System.Security.Permissions;

namespace BinderV2.MVVM.Models
{
    class TriggersManager : INotifyPropertyChanged
    {
        private Bind Bind;
        public ObservableCollection<BaseTriggerViewModel> Triggers { get; private set; }

        private BaseTriggerViewModel selectedTrigger;
        public BaseTriggerViewModel SelectedTrigger 
        {
            get { return selectedTrigger; }
            set
            {
                UnselectAll();
                if (value == null)
                    return;
                selectedTrigger = value;
                selectedTrigger.IsSelected = true;
                if (!Triggers.Contains(selectedTrigger))
                {
                    Triggers.Add(selectedTrigger);
                    OnPropertyChanged("Triggers");
                }
                OnPropertyChanged("SelectedTriggerScript");
            }
        }

        public string SelectedTriggerScript 
        { 
            get { return SelectedTrigger != null ? SelectedTrigger.Trigger.Script : ""; } 
            set 
            {
                if (SelectedTrigger != null)
                    SelectedTrigger.Trigger.Script = value;
                OnPropertyChanged("SelectedTriggerScript");
            }
        }

        public TriggersManager(Bind bind)
        {
            Bind = bind;
            Triggers = new ObservableCollection<BaseTriggerViewModel>();
            GetTriggersViewModelsFromBind();
        }

        public void ClearTriggers()
        {
            Bind.Triggers.Clear();
            Triggers.Clear();
            OnPropertyChanged("Triggers");
        }
        public void CreateNewTrigger()
        {
            ChooseTriggerTypeWindow ctw = new ChooseTriggerTypeWindow();
            ctw.ShowDialog();
            if (ctw.SelectedType == TriggerType.None)
                return;
            BaseTrigger newTrigger = TriggerUtility.GetTriggerFromTriggerType("Новый триггер", ctw.SelectedType);

            Bind.Triggers.Add(newTrigger);
            Triggers.Add(TriggerUtility.GetViewModelForTrigger(newTrigger));
            OnPropertyChanged("Triggers");
        }
        public bool RemoveTrigger(BaseTriggerViewModel toRemove)
        {
            bool result = Triggers.Remove(toRemove) && Bind.Triggers.Remove(toRemove.Trigger);
            OnPropertyChanged("Triggers");
            return result;
        }
        public void UnselectAll()
        {
            foreach (BaseTriggerViewModel trig in Triggers)
                trig.IsSelected = false;
        }
        public void SaveScriptToSelectedBind(string script)
        {
            SelectedTriggerScript = script;
        }

        private void GetTriggersViewModelsFromBind()
        {
            Triggers.Clear();
            foreach (BaseTrigger trigger in Bind.Triggers)
                Triggers.Add(TriggerUtility.GetViewModelForTrigger(trigger));
            OnPropertyChanged("Triggers");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
