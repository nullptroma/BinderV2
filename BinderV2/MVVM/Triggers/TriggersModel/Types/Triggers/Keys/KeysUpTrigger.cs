using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Hooks.Keyboard;
using System.Threading;
using System.Windows.Forms;

namespace Trigger.Types
{
    public class KeysUpTrigger : BaseKeysTrigger
    {
        public override string TypeName { get { return "Кнопки подняты"; } }
        private bool NeedKeysWasDown = false;

        public KeysUpTrigger(string name, ICollection<Key> keys) : base(name)
        {
            Keys = new HashSet<Key>();
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));
            foreach (Key k in keys)
            {
                this.Keys.Add(k);
            }
            KeysEngine.KeyUp += (sender, e) => 
            {
                if (Keys.Count == 0)
                    return;
                if (NeedKeysWasDown)
                    if (NeedKeysAreUp(e.PressedKeys))
                        Invoke(e);
            };
            KeysEngine.KeyDown+=(sender, e)=> CheckPressedKeys(e.PressedKeys);
        }

        public KeysUpTrigger() : this("Новый триггер", new HashSet<Key>())
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
