using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Triggers.Types.MouseEngine
{
    static class MouseTriggersEngine//класс-обёртка на события
    {
        public static event MouseEventHandler MouseDown;
        public static event MouseEventHandler MouseUp;
        public static event MouseEventHandler MouseMove;

        static MouseTriggersEngine()
        {
            Hooks.Mouse.MouseHook.MouseDown += (sender, e) => MouseDown?.Invoke(sender, e);
            Hooks.Mouse.MouseHook.MouseUp += (sender, e) => MouseUp?.Invoke(sender, e);
            Hooks.Mouse.MouseHook.MouseMove += (sender, e) => MouseMove?.Invoke(sender, e);
        }
    }
}
