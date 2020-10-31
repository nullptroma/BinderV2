using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Trigger.Types.KeysEngine;

namespace Trigger.Types
{
    class KeysHolding : BaseKeysTrigger
    {
        public override string TypeDescription { get { return "Кнопки задержаны"; } }

        public KeysHolding(string name, ICollection<Key> keys) : base(name)
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
                InvokeIfHaveNeedKeys(e.PressedKeys);
            };
        }

        public KeysHolding() : this("Новый триггер", new HashSet<Key>())
        { }


        private void InvokeIfHaveNeedKeys(HashSet<Key> pressedKeys)
        {
            if (HaveNeedKeys(pressedKeys))
                Invoke(new Events.TriggeredEventArgs(Name, Script));
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
