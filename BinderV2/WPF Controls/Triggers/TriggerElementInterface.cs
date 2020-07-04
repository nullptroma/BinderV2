using BinderV2.Trigger.Types;

namespace BinderV2.WpfControls.Triggers
{
    interface ITriggerElement
    {
        BaseTrigger GetTrigger();
        bool Selected { get; set; }
    }
}
