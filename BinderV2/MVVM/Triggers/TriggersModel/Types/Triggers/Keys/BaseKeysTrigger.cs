using InterpreterScripts.InterpretationScriptData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Trigger.Events;
using static Hooks.Keyboard.KeysEngine;

namespace Trigger.Types
{
    public abstract class BaseKeysTrigger : BaseTrigger
    {
        private HashSet<Key> keys;
        public bool Exclusive { get; set; }
        public HashSet<Key> Keys
        {
            get { return keys; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                keys = value;
            }
        }

        public void Invoke(PressedKeysEventArgs key)
        {
            var data = new InterpretationData();
            data.Vars["Key"] = key.Key.ToString();
            base.Invoke(new TriggeredEventArgs(Name, Script, data));
            if (Exclusive && EnableTrigger && EnableAllTriggers)//блочим только в том случае, если сработали
                key.Handled = true;
        }

        public BaseKeysTrigger(string name) : base(name)
        { }
    }
}
