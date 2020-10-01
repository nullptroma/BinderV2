using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Trigger.Types;
using BinderV2.MVVM.ViewModels.Triggers;

namespace Trigger.Tools
{
    static class TriggerUtility
    {
        public static BaseTriggerViewModel GetViewModelForTrigger(BaseTrigger trig)
        {
            BaseTriggerViewModel vm = null;
            switch(trig.GetType().Name)
            {
                case "KeysDownTrigger":
                    {
                        vm = new KeysDownTriggerViewModel((KeysDownTrigger)trig);
                        break; 
                    }
                default:
                    {
                        throw new Trigger.TriggersExeptions.UnknownTriggerTypeExeption(trig.GetType());
                    }
            }
            return vm;
            
        }


        public static BaseTrigger GetTriggerFromTriggerType(string name, TriggerType type)
        {
            switch (type)
            {
                case TriggerType.KeysDown:
                    return new KeysDownTrigger(name, new HashSet<Key>());
                default:
                    {
                        throw new Trigger.TriggersExeptions.UnknownTriggerTypeExeption(type.GetType());
                    }
            }
        }
    }
}
