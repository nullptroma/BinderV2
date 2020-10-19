using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trigger.Types;
using Trigger.Events;

namespace Triggers.Types
{
    class OnAddCallbackTrigger : BaseTrigger
    {
        public OnAddCallbackTrigger(string name) : base(name) 
        {
            CallbackAdded += (sender, e) => { e.meth.Invoke(this, new TriggeredEventArgs(Name, Script)); };
        }

        public OnAddCallbackTrigger() : this("Новый триггер")
        { }

        public override void Dispose()
        {
            
        }
    }
}
