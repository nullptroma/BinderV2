using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinderV2.Interpreter.DescriptionAttribute
{
    [AttributeUsage(AttributeTargets.Method)]
    class ScriptDescriptionAttribute : Attribute
    {
        public string Description { get; private set; }
        public ScriptDescriptionAttribute(string desc)
        {
            Description = desc;
        }
    }
}
