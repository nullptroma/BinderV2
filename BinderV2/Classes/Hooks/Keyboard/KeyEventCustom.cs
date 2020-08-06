using System.Windows.Input;

namespace Hooks.Keyboard
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
