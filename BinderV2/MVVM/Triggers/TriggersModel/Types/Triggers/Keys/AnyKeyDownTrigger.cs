using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Hooks.Keyboard;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Trigger.Types
{
    public class AnyKeyDownTrigger : BaseKeysTrigger
    {
        public override string TypeName { get { return "Любая кнопка нажата"; } }

        public AnyKeyDownTrigger(string name) : base(name)
        {
            KeysEngine.KeyDown += (sender, e) =>
            {
                Invoke(e);
            };
        }

        public AnyKeyDownTrigger() : this("Новый триггер")
        { }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
