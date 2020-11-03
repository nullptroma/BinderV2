using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Triggers.Types.KeysEngine;

namespace Trigger.Types
{
    class KeysHoldingTrigger : BaseKeysTrigger
    {
        public override string TypeName { get { return "Кнопки задержаны"; } }

        public KeysHoldingTrigger(string name, ICollection<Key> keys) : base(name)
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
                InvokeIfHaveNeedKeys(e.PressedKeys, e.Key);
            };
        }

        public KeysHoldingTrigger() : this("Новый триггер", new HashSet<Key>())
        { }


        private void InvokeIfHaveNeedKeys(HashSet<Key> pressedKeys, Key lastKey)
        {
            if (HaveNeedKeys(pressedKeys))
                Invoke(lastKey);
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
