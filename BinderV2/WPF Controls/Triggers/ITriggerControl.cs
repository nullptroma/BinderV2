using Trigger.Types;

namespace BinderV2.WpfControls.Triggers
{
    interface ITriggerControl
    {
        BaseTrigger GetTrigger();
        bool Selected { get; set; }
    }
}
