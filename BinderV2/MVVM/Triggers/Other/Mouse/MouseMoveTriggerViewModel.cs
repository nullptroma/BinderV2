using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trigger.Types;
using Triggers.Types;

namespace BinderV2.MVVM.ViewModels.Triggers
{
    public class MouseMoveTriggerViewModel : BaseTriggerViewModel
    {
        public MouseMoveTriggerViewModel(MouseMoveTrigger trigger) : base(trigger)
        { }
    }
}
