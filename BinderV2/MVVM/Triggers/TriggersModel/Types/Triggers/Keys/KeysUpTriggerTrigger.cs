using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Hooks.Keyboard;
using System.Threading;
using System.Windows.Forms;
using Triggers.Types.KeysEngine;

namespace Trigger.Types
{
    public class KeysUpTriggerTrigger : BaseKeysTrigger
    {
        public override string TypeName { get { return "Кнопки подняты"; } }
        private bool NeedKeysWasDown = false;

        public KeysUpTriggerTrigger(string name, ICollection<Key> keys) : base(name)
        {
            Keys = new HashSet<Key>();
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));
            foreach (Key k in keys)
            {
                this.Keys.Add(k);
            }
            KeysTriggersEngine.KeyUp += (sender, e) => 
            {
                if (Keys.Count == 0)
                    return;
                if (NeedKeysWasDown)
                    if (NeedKeysAreUp(e.PressedKeys))
                        Invoke(e.Key);
            };
            KeysTriggersEngine.KeyDown+=(sender, e)=> CheckPressedKeys(e.PressedKeys);
        }

        public KeysUpTriggerTrigger() : this("Новый триггер", new HashSet<Key>())
        { }

        private void CheckPressedKeys(HashSet<Key> pressedKeys)
        {
            NeedKeysWasDown = false;
            if (Keys.All(k=> pressedKeys.Contains(k)))
                NeedKeysWasDown = true;
        }

        private bool NeedKeysAreUp(HashSet<Key> pressedKeys)
        {
            foreach (Key k in Keys)
                if (pressedKeys.Contains(k))
                    return false;
            return true;
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
