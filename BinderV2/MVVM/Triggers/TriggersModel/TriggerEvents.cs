using InterpreterScripts.InterpretationScriptData;
using System;

namespace Trigger.Events
{
    public class TriggeredEventArgs : EventArgs
    {
        public string TriggerName { get; private set; }
        public string TriggerScript { get; private set; }
        public InterpretationData triggerData { get; private set; }

        public TriggeredEventArgs(string name, string script, InterpretationData _data = null)
        {
            TriggerName = name;
            TriggerScript = script;
            triggerData = _data != null ? _data : new InterpretationData();

            triggerData.Vars["TriggerName"] = name;
            triggerData.Vars["TriggerScript"] = script;
        }
    }
    public delegate void TriggeredEventHandler(object sendet, TriggeredEventArgs e);


    public class EnableTriggerChangedEventArgs : EventArgs
    {
        public bool enable;
        public EnableTriggerChangedEventArgs(bool e) : base()
        {
            enable = e;
        }
    }
    public delegate void EnableTriggerChangedEventHandler(object sender, EnableTriggerChangedEventArgs e);


    public class CallbackEditEventArgs : EventArgs
    {
        public TriggeredEventHandler meth;
        public CallbackEditEventArgs(TriggeredEventHandler _meth) : base()
        {
            meth = _meth;
        }
    }
    public delegate void CallbackEditEventHandler(object sender, CallbackEditEventArgs e);
}