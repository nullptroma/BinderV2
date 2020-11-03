using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trigger.Types;
using Trigger.Events;
using System.Windows.Threading;
using System.Windows;

namespace Triggers.Types
{
    public class TimerTrigger : BaseTrigger
    {
        private static List<TimerForTrigger> timers = new List<TimerForTrigger>();

        public override string TypeName { get { return "Таймер"; } }
        private TimerForTrigger currentTimer;
        public int milliseconds = 1000;
        public int Milliseconds
        {
            get { return milliseconds; } 
            set { milliseconds = value;
                UpdateTimer();
            }
        }

        private void UpdateTimer()
        {
            if (currentTimer != null)
            {
                currentTimer.timer.Tick -= Tick;
                currentTimer.Count--;
                if (currentTimer.Count == 0)
                    currentTimer.timer.Stop();
            }

            currentTimer = timers.Find(t => t.timer.Interval == new TimeSpan(0, 0, 0, 0, milliseconds));
            if (currentTimer == null)
            {
                currentTimer = new TimerForTrigger()
                {
                    timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, milliseconds) },
                    Count = 1
                };
                timers.Add(currentTimer);
            }
            if(!currentTimer.timer.IsEnabled)
                currentTimer.timer.Start();
            currentTimer.timer.Tick += Tick;
        }

        public void Tick(object sender, EventArgs e)
        {
            Invoke(new TriggeredEventArgs(Name, Script));
        }

        public TimerTrigger(string name) : base(name)
        {
            UpdateTimer();
        }

        public TimerTrigger() : this("Новый триггер")
        { }

        public override void Dispose()
        {

        }
    }

    class TimerForTrigger
    {
        public DispatcherTimer timer;
        public int Count;
    }
}
