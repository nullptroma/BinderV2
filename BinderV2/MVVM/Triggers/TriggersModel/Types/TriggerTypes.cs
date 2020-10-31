
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

        public static KeysTriggerViewModel NewKeysHoldingOnce
        { get { return new KeysTriggerViewModel(new KeysHoldingOnce()); } }

        public static KeysTriggerViewModel NewKeysHolding
        { get { return new KeysTriggerViewModel(new KeysHolding()); } }
        
        public static BaseTriggerViewModel NewOnAddCallback
        { get { return new OnAddCallbackTriggerViewModel(new OnAddCallbackTrigger()); } }

        public static BaseTriggerViewModel NewTimerTrigger
        { get { return new TimerTriggerViewModel(new TimerTrigger()); } }
        
        public static BaseTriggerViewModel NewAnyKeyDown
        { get { return new AnyKeyDownViewModel(new AnyKeyDown()); } }
        
        public static BaseTriggerViewModel NewAnyKeyUp
        { get { return new AnyKeyUpViewModel(new AnyKeyUp()); } }
        
        public static BaseTriggerViewModel NewOnExitTrigger
        { get { return new OnExitTriggerViewModel(new OnExitTrigger()); } }

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
                        vm = new KeysTriggerViewModel((KeysHolding)trigger);
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
                        vm = new AnyKeyDownViewModel((AnyKeyDown)trigger);
                        break;
                    }
                case "AnyKeyUp":
                    {
                        vm = new AnyKeyUpViewModel((AnyKeyUp)trigger);
                        break;
                    }
                case "OnExitTrigger":
                    {
                        vm = new OnExitTriggerViewModel((OnExitTrigger)trigger);
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
