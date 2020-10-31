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
    public class AnyKeyDown : BaseTrigger
    {
        public override string TypeDescription { get { return "Любая кнопка нажата"; } }

        public AnyKeyDown(string name) : base(name)
        {
            KeysTriggersEngine.KeyDown += (sender, e) =>
            {
                var data = new InterpreterScripts.InterpretationScriptData.InterpretationData();
                data.Vars["Key"] = e.Key.ToString();
                Invoke(new Events.TriggeredEventArgs(Name, Script, data));
            };
        }

        public AnyKeyDown() : this("Новый триггер")
        { }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
