using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trigger.Types;
using Trigger.Events;
using System.Windows.Threading;

namespace Triggers.Types
{
    class TimerTrigger : BaseTrigger
    {
        public override string TypeDescription { get { return "Таймер"; } }
        private DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(1000) };
        public long milliseconds = 1000;
        public long Milliseconds
        {
            get { return milliseconds; } 
            set { milliseconds = value;
                timer.Stop();
                timer.Interval = TimeSpan.FromMilliseconds(milliseconds);
                timer.Start();
            }
        }

        public TimerTrigger(string name) : base(name)
        {
            timer.Tick += (sender, e) => { Invoke(new TriggeredEventArgs(Name, Script)); };
            timer.Start();
        }

        public TimerTrigger() : this("Новый триггер")
        { }

        public override void Dispose()
        {

        }
    }
}
