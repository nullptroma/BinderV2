using System;

namespace Trigger.Events
{
    public class TriggeredEventArgs : EventArgs
    {
        public string TriggerName { get; set; }
        public string TriggerScript { get; set; }

        public TriggeredEventArgs(string name, string script)
        {
            TriggerName = name;
            TriggerScript = script;
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