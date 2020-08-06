using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Trigger.Types;

namespace BinderV2.Commands
{
    public static class EnableDisableTriggers
    {
        private static RelayCommand disableTriggersCommand;
        public static RelayCommand DisableTriggersCommand
        {
            get
            {
                return disableTriggersCommand ??
                  (disableTriggersCommand = new RelayCommand(obj =>
                  {
                      BaseTrigger.EnableAllTriggers = false;
                  }));
            }
        }

        private static RelayCommand enableTriggersCommand;
        public static RelayCommand EnableTriggersCommand
        {
            get
            {
                return enableTriggersCommand ??
                  (enableTriggersCommand = new RelayCommand(obj =>
                  {
                      BaseTrigger.EnableAllTriggers = true;
                  }));
            }
        }
    }
}
