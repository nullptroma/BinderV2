using Hooks.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Trigger.Types.KeysEngine
{
    static class KeysTriggersEngine
    {
        public class PressedKeysEventArgs : EventArgs
        {
            public HashSet<Key> PressedKeys { get; private set; }
            public Key Key { get; private set; }
            public PressedKeysEventArgs(HashSet<Key> _PressedKeys, Key _k) : base()
            { 
                PressedKeys = _PressedKeys;
                Key = _k;
            }
        }
        public delegate void PressedKeysEventHandler(object sender, PressedKeysEventArgs e);

        public static event PressedKeysEventHandler KeyDown;
        public static event PressedKeysEventHandler KeyUp;
        public static event PressedKeysEventHandler KeysHolding;

        public static HashSet<KeysDownTrigger> AllKeyDownTriggers { get; private set; }
        public static HashSet<Key> PressedKeys { get; private set; }
        static KeysTriggersEngine()
        {
            PressedKeys = new HashSet<Key>();
            AllKeyDownTriggers = new HashSet<KeysDownTrigger>();
            KeyboardHook.KeyDown += KeysDown;
            KeyboardHook.KeyUp += KeysUp;
        }

        private static void KeysDown(object sender, KeyEventArgsCustom e)
        {
            if (PressedKeys.Contains(e.Key))
            {
                KeysHolding?.Invoke(null, new PressedKeysEventArgs(PressedKeys, e.Key));
                return;
            }   
            PressedKeys.Add(e.Key);
            KeyDown?.Invoke(null, new PressedKeysEventArgs(PressedKeys, e.Key));
        }
        private static void KeysUp(object sender, KeyEventArgsCustom e)
        {
            PressedKeys.Remove(e.Key);
            KeyUp?.Invoke(null, new PressedKeysEventArgs(PressedKeys, e.Key));
        }
    }
}
