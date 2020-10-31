using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Trigger.Types
{
    public abstract class BaseKeysTrigger : BaseTrigger
    {
        private HashSet<Key> keys;
        public HashSet<Key> Keys
        {
            get { return keys; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                keys = value;
            }
        }

        public BaseKeysTrigger(string name) : base(name)
        { }
    }
}
