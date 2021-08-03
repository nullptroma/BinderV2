using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using Trigger.Types;
using Trigger.Events;

namespace Triggers.Types
{
    public class OnAddCallbackTrigger : BaseTrigger
    {
        public override string TypeName { get { return "При запуске"; } }
        public OnAddCallbackTrigger(string name) : base(name) 
        {
            CallbackAdded += (sender, e) => { Invoke(new TriggeredEventArgs(Name, Script)); };
        }

        public OnAddCallbackTrigger() : this("Новый триггер")
        { }

        public override void Dispose()
        {
            
        }
    }
}
