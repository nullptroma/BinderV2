using Hooks.Keyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Trigger.Types;

namespace Hooks.Keyboard
{
    public static class KeysEngine
    {
        public class PressedKeysEventArgs : EventArgs
        {
            public HashSet<Key> PressedKeys { get; private set; }
            public Key Key { get; private set; }
            public bool Handled { get; set; }
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

        public static HashSet<Key> PressedKeys { get; private set; }
        static KeysEngine()
        {
            PressedKeys = new HashSet<Key>();
            KeyboardHook.KeyDown += KeysDown;
            KeyboardHook.KeyUp += KeysUp;
        }

        private static void KeysDown(object sender, KeyEventArgsCustom e)
        {
            var eventArgs = new PressedKeysEventArgs(PressedKeys, e.Key);
            if (PressedKeys.Contains(e.Key))
            {
                KeysHolding?.Invoke(null, eventArgs);
                if (eventArgs.Handled)
                    e.Handled = true;
                return;
            }   
            PressedKeys.Add(e.Key);
            KeyDown?.Invoke(null, eventArgs);
            if (eventArgs.Handled)
                e.Handled = true;
        }
        private static void KeysUp(object sender, KeyEventArgsCustom e)
        {
            var eventArgs = new PressedKeysEventArgs(PressedKeys, e.Key);
            PressedKeys.Remove(e.Key);
            KeyUp?.Invoke(null, eventArgs);
            if (eventArgs.Handled)
                e.Handled = true;
        }
    }
}
