using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinderV2.Trigger.TriggersExeptions
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
