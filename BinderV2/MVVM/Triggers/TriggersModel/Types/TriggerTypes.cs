
using BinderV2.MVVM.ViewModels.Triggers;
using BinderV2.MVVM.Views.Triggers;
using Triggers.Types;

namespace Trigger.Types
{
    public enum TriggerType
    {
        None,
        KeysDown,
        KeysUp,
        OnAddCallback,
    }

    public static class TriggersUtilities
    {
        public static KeysTriggerViewModel NewKeysDownTrigger
        { get { return new KeysTriggerViewModel(new KeysDownTrigger()); } }

        public static KeysTriggerViewModel NewKeysUpTrigger
        { get { return new KeysTriggerViewModel(new KeysUpTrigger()); } }

        public static KeysTriggerViewModel NewKeysHoldingOnceTrigger
        { get { return new KeysTriggerViewModel(new KeysHoldingOnce()); } }

        public static KeysTriggerViewModel NewKeysHoldingTrigger
        { get { return new KeysTriggerViewModel(new KeysHoldingTrigger()); } }
        
        public static OnAddCallbackTriggerViewModel NewOnAddCallbackTrigger
        { get { return new OnAddCallbackTriggerViewModel(new OnAddCallbackTrigger()); } }

        public static TimerTriggerViewModel NewTimerTrigger
        { get { return new TimerTriggerViewModel(new TimerTrigger()); } }
        
        public static AnyKeyDownViewModel NewAnyKeyDownTrigger
        { get { return new AnyKeyDownViewModel(new AnyKeyDownTrigger()); } }
        
        public static AnyKeyUpViewModel NewAnyKeyUpTrigger
        { get { return new AnyKeyUpViewModel(new AnyKeyUpTrigger()); } }
        
        public static OnExitTriggerViewModel NewOnExitTrigger
        { get { return new OnExitTriggerViewModel(new OnExitTrigger()); } }

        public static MouseMoveTriggerViewModel NewMouseMoveTrigger
        { get { return new MouseMoveTriggerViewModel(new MouseMoveTrigger()); } }

        public static MouseDownTriggerViewModel NewMouseDownTrigger
        { get { return new MouseDownTriggerViewModel(new MouseDownTrigger()); } }

        public static MouseUpTriggerViewModel NewMouseUpTrigger
        { get { return new MouseUpTriggerViewModel(new MouseUpTrigger()); } }

        public static BaseTriggerViewModel GetViewModelForTrigger(BaseTrigger trigger)
        {
            BaseTriggerViewModel vm = null;
            switch (trigger.GetType().Name)
            {
                case "KeysDownTrigger":
                    {
                        vm = new KeysTriggerViewModel((KeysDownTrigger)trigger);
                        break;
                    }
                case "KeysUpTrigger":
                    {
                        vm = new KeysTriggerViewModel((KeysUpTrigger)trigger);
                        break;
                    }
                case "KeysHoldingOnce":
                    {
                        vm = new KeysTriggerViewModel((KeysHoldingOnce)trigger);
                        break;
                    }
                case "KeysHolding":
                    {
                        vm = new KeysTriggerViewModel((KeysHoldingTrigger)trigger);
                        break;
                    }
                case "OnAddCallbackTrigger":
                    {
                        vm = new OnAddCallbackTriggerViewModel((OnAddCallbackTrigger)trigger);
                        break;
                    }
                case "TimerTrigger":
                    {
                        vm = new TimerTriggerViewModel((TimerTrigger)trigger);
                        break;
                    }
                case "AnyKeyDown":
                    {
                        vm = new AnyKeyDownViewModel((AnyKeyDownTrigger)trigger);
                        break;
                    }
                case "AnyKeyUp":
                    {
                        vm = new AnyKeyUpViewModel((AnyKeyUpTrigger)trigger);
                        break;
                    }
                case "OnExitTrigger":
                    {
                        vm = new OnExitTriggerViewModel((OnExitTrigger)trigger);
                        break;
                    }
                case "MouseMoveTrigger":
                    {
                        vm = new MouseMoveTriggerViewModel((MouseMoveTrigger)trigger);
                        break;
                    }
                case "MouseDownTrigger":
                    {
                        vm = new MouseDownTriggerViewModel((MouseDownTrigger)trigger);
                        break;
                    }
                case "MouseUpTrigger":
                    {
                        vm = new MouseUpTriggerViewModel((MouseUpTrigger)trigger);
                        break;
                    }
                default:
                    {
                        throw new TriggersExeptions.UnknownTriggerTypeExeption(trigger.GetType());
                    }
            }
            return vm;
        }
    }
}
