using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterScripts.InterpretationScriptData.Stopper
{
    public class StopReceiver
    {
        public bool IsStopped { get; set; }

        private void Stop(object sender, EventArgs e)
        {
            IsStopped = true;
        }

        public void RegisterStopper(StopSender ss)
        {
            ss.StopInvoked += Stop;
        }
        
        public void UnRegisterStopper(StopSender ss)
        {
            ss.StopInvoked -= Stop;
        }
    }
}
