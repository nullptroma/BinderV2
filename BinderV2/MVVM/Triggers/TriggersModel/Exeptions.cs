using System;

namespace Trigger.TriggersExeptions
{
    class UnknownTriggerTypeExeption : Exception
    {
        public Type type;

        public UnknownTriggerTypeExeption() : base()
        {
        }

        public UnknownTriggerTypeExeption(Type t) : base()
        {
            type = t;
        }

        public UnknownTriggerTypeExeption(string message, Type t) : base(message)
        {
            type = t;
        }
    }
}
