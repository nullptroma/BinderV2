using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterScripts.Funcslibrary
{
    [AttributeUsage(AttributeTargets.Method)]
    class FuncDescriptionAttribute : Attribute
    {
        public string Description { get; private set; }
        public FuncDescriptionAttribute(string desc)
        {
            Description = desc;
        }
    }
}
