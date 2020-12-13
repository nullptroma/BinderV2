using BinderV2.Commands;
using Hooks.Keyboard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Trigger.Types;

namespace BinderV2.MVVM.ViewModels.Triggers
{
    public class KeysTriggerViewModel : BaseTriggerViewModel
    {
        public string triggerKeysString;
        public string TriggerKeysString { get { return triggerKeysString; } private set { triggerKeysString = value; OnPropertyChanged("TriggerKeysString"); } }
        public bool Exclusive { get { return Trigger.Exclusive; } set { Trigger.Exclusive = value; OnPropertyChanged("Exclusive"); } }
        private new BaseKeysTrigger Trigger;

        public KeysTriggerViewModel(BaseKeysTrigger keysTrigger) : base(keysTrigger) 
        {
            Trigger = keysTrigger;
            TriggerKeysString = string.Join(" + ", Trigger.Keys);
            if (TriggerKeysString.Length == 0)
                TriggerKeysString = "<пусто>";
        }

        private RelayCommand changeKeysCommand;
        public RelayCommand ChangeKeysCommand
        {
            get
            {
                return changeKeysCommand ??
                  (changeKeysCommand = new RelayCommand(obj =>
                  {
                      BaseTrigger.EnableAllTriggers = false;//отключаем все триггеры, а соотвественно бинды, чтобы записать новые кнопки.
                      Trigger.Keys.Clear();

                      KeyEventHandlerCustom keyDown = null;
                      keyDown = (s, keyArgs) =>
                      {
                          Trigger.Keys.Add(keyArgs.Key);
                          TriggerKeysString = string.Join(" + ", Trigger.Keys);
                      };

                      KeyEventHandlerCustom keyUp = null;
                      keyUp = (s, keyArgs) =>
                      {
                          KeyboardHook.KeyDown -= keyDown;
                          KeyboardHook.KeyUp -= keyUp;
                          BaseTrigger.EnableAllTriggers = true;//когда записали, включаем обратно
                      };

                      KeyboardHook.KeyDown += keyDown;
                      KeyboardHook.KeyUp += keyUp;

                      TriggerKeysString = "<нажмите сочетание>";
                  }));
            }
        }
    }
}
