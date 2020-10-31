using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Hooks.Keyboard;
using System.Threading;
using System.Windows.Forms;
using Trigger.Types.KeysEngine;
using System.Threading.Tasks;

namespace Trigger.Types
{
    public class KeysDownTrigger : BaseKeysTrigger
    {
        public override string TypeDescription { get { return "Кнопки нажаты"; } }
        private bool NeedKeysWasUp = true;

        public KeysDownTrigger(string name, ICollection<Key> keys) : base(name)
        {
            Keys = new HashSet<Key>();
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));
            foreach (Key k in keys)
            {
                this.Keys.Add(k);
            }
            KeysTriggersEngine.KeyDown += (sender, e)=> 
            {
                if (Keys.Count == 0)//если у нас не настроены кнопки, чтобы не срабатывало
                    return;
                if(NeedKeysWasUp)
                    InvokeIfHaveNeedKeys(e.PressedKeys);
            };
            KeysTriggersEngine.KeyUp += (sender, e) => CheckUpKeys(e.PressedKeys);
        }

        public KeysDownTrigger() : this("Новый триггер", new HashSet<Key>())
        {}

        private void CheckUpKeys(HashSet<Key> pressedKeys)
        {
            NeedKeysWasUp = true;
            if (Keys.All(k => pressedKeys.Contains(k)))
                NeedKeysWasUp = false;
        }

        private void InvokeIfHaveNeedKeys(HashSet<Key> pressedKeys)
        {
            if (HaveNeedKeys(pressedKeys))
            {
                NeedKeysWasUp = false;
                Invoke(new Events.TriggeredEventArgs(Name, Script));
            }
        }

        private bool HaveNeedKeys(HashSet<Key> pressedKeys)
        {
            foreach (Key k in Keys)
                if (!pressedKeys.Contains(k))
                    return false;
            return true;
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
