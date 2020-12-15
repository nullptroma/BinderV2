using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Trigger.Types;
using Hooks.Mouse;

namespace Triggers.Types
{
    public class MouseMoveTrigger : BaseMouseTrigger
    {
        public override string TypeName { get { return "Перемещение курсора"; } }

        public MouseMoveTrigger()
        {
            MouseHook.MouseMove += MouseMove;
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            Invoke(e);
        }

        public override void Dispose()
        {
            MouseHook.MouseMove -= MouseMove;
            GC.SuppressFinalize(this);
        }
    }
}
