using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Triggers.Types.KeysEngine;
using static Triggers.Types.KeysEngine.KeysTriggersEngine;

namespace Trigger.Types
{
    class KeysHoldingOnce : BaseKeysTrigger
    {
        public override string TypeName { get { return "Кнопки задержаны (один раз)"; } }
        private bool NeedKeysWasUp = true;

        public KeysHoldingOnce(string name, ICollection<Key> keys) : base(name)
        {
            Keys = new HashSet<Key>();
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));
            foreach (Key k in keys)
                this.Keys.Add(k);
            KeysTriggersEngine.KeysHolding += (sender, e) =>
            {
                if (Keys.Count == 0)//если у нас не настроены кнопки, чтобы не срабатывало
                    return;
                if (NeedKeysWasUp)
                    InvokeIfHaveNeedKeys(e.PressedKeys, e);
            };
            KeysTriggersEngine.KeyUp += (sender, e) => CheckUpKeys(e.PressedKeys);
        }

        public KeysHoldingOnce() : this("Новый триггер", new HashSet<Key>())
        { }

        private void CheckUpKeys(HashSet<Key> pressedKeys)
        {
            NeedKeysWasUp = true;
            if (Keys.All(k => pressedKeys.Contains(k)))
                NeedKeysWasUp = false;
        }

        private void InvokeIfHaveNeedKeys(HashSet<Key> pressedKeys, PressedKeysEventArgs lastKey)
        {
            if (HaveNeedKeys(pressedKeys))
            {
                NeedKeysWasUp = false;
                Invoke(lastKey);
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
