using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BinderV2.Hooks.Keyboard
{
    public class KeyEventArgsCustom
    {
        public Key Key { get; set; }
        public bool Handled { get; set; }

        public KeyEventArgsCustom(Key key)
        {
            this.Key = key;
        }
    }

    public delegate void KeyEventHandlerCustom(object sender, KeyEventArgsCustom e);
}
