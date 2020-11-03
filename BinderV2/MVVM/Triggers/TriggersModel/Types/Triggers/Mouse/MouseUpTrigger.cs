using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Trigger.Types;
using Triggers.Types.MouseEngine;

namespace Triggers.Types
{
    public class MouseUpTrigger : BaseMouseTrigger
    {
        public override string TypeName { get { return "Кнопка мыши поднята"; } }

        public MouseUpTrigger()
        {
            MouseTriggersEngine.MouseUp += MouseUp;
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            Invoke(e);
        }

        public override void Dispose()
        {
            MouseTriggersEngine.MouseUp -= MouseUp;
            GC.SuppressFinalize(this);
        }
    }
}
