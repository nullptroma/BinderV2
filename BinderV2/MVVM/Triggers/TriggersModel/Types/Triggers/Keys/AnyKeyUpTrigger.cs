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
    public class AnyKeyUpTrigger : BaseKeysTrigger
    {
        public override string TypeName { get { return "Любая кнопка поднята"; } }

        public AnyKeyUpTrigger(string name) : base(name)
        {
            KeysEngine.KeyUp += (sender, e) =>
            {
                Invoke(e);
            };
        }

        public AnyKeyUpTrigger() : this("Новый триггер")
        { }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
