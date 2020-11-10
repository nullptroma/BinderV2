using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterScripts.InterpretationScriptData.Stopper
{
    public class StopSender
    {
        public event EventHandler StopInvoked;
        public void Stop()
        {
            StopInvoked?.Invoke(this, new EventArgs());
        }
    }
}
