using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Trigger.Types;
using BinderV2.WpfControls.Triggers;

namespace Trigger.Tools
{
    static class TriggerUtility
    {
        public static Control GetControlFromTrigger(BaseTrigger trig)
        {
            Control control = null;
            switch(trig.GetType().Name)
            {
                case "KeysDownTrigger":
                    {
                        control = new KeysDownTriggerControl((KeysDownTrigger)trig);
                        break; 
                    }
                default:
                    {
                        throw new Trigger.TriggersExeptions.UnknownTriggerTypeExeption(trig.GetType());
                    }
            }
            return control;
            
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
