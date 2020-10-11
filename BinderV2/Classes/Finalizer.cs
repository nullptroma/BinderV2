using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinderV2.Classes
{
    static class Finalizer
    {
        public static void FinalActions()
        {
            Hooks.Mouse.MouseHook.UnInstallHook();
        }
    }
}
