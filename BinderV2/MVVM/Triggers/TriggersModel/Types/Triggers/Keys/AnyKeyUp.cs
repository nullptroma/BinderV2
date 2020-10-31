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
    public class AnyKeyUp : BaseTrigger
    {
        public override string TypeDescription { get { return "Любая кнопка поднята"; } }

        public AnyKeyUp(string name) : base(name)
        {
            KeysTriggersEngine.KeyUp += (sender, e) =>
            {
                var data = new InterpreterScripts.InterpretationScriptData.InterpretationData();
                data.Vars["Key"] = e.Key.ToString();
                Invoke(new Events.TriggeredEventArgs(Name, Script, data));
            };
        }

        public AnyKeyUp() : this("Новый триггер")
        { }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
