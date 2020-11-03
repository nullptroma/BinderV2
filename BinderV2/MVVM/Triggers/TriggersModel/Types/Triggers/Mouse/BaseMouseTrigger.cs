using InterpreterScripts.InterpretationScriptData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Trigger.Events;
using Trigger.Types;

namespace Triggers.Types
{
    public abstract class BaseMouseTrigger : BaseTrigger
    {
        public void Invoke(MouseEventArgs e)
        {
            var data = new InterpretationData();
            data.Vars["X"] = e.X;
            data.Vars["Y"] = e.Y;
            data.Vars["Button"] = e.Button;
            base.Invoke(new TriggeredEventArgs(Name, Script, data));
        }
    }
}
