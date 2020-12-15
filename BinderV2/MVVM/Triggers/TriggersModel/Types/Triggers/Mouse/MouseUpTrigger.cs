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
    public class MouseUpTrigger : BaseMouseTrigger
    {
        public override string TypeName { get { return "Кнопка мыши поднята"; } }

        public MouseUpTrigger()
        {
            MouseHook.MouseUp += MouseUp;
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            Invoke(e);
        }

        public override void Dispose()
        {
            MouseHook.MouseUp -= MouseUp;
            GC.SuppressFinalize(this);
        }
    }
}
