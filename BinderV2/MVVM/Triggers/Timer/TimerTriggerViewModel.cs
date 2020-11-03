using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Triggers.Types;

namespace BinderV2.MVVM.ViewModels.Triggers
{
    public class TimerTriggerViewModel : BaseTriggerViewModel
    {
        private TimerTrigger trigger;
        public string TimerMilliseconds 
        {
            get { return trigger.Milliseconds.ToString(); }
            set { 
                if (int.TryParse(value, out int res))
                    trigger.Milliseconds = res;
                OnPropertyChanged("TimerMilliseconds");
            } 
        }

        public TimerTriggerViewModel(TimerTrigger _trigger) : base(_trigger)
        {
            trigger = _trigger;
        }
    }
}
