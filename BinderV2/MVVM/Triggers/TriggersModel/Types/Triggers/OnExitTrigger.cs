using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trigger.Types;
using Trigger.Events;
using BinderV2;
using System.Threading;

namespace Triggers.Types
{
    public class OnExitTrigger : BaseTrigger
    {
        public override string TypeName { get { return "При выходе"; } }
        public OnExitTrigger(string name) : base(name)
        {
            App.Current.MainWindow.Closed += (sender, e) =>
            {
                Invoke(new TriggeredEventArgs(Name, Script)).Wait();
            };
        }

        public OnExitTrigger() : this("Новый триггер")
        { }

        public override void Dispose()
        {

        }
    }
}
