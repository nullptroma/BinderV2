using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterScripts.Funcslibrary
{
    [AttributeUsage(AttributeTargets.Method)]
    class FuncGroupAttribute : Attribute
    {
        public string GroupName { get; private set; }
        public FuncGroupAttribute(string groupName)
        {
            GroupName = groupName;
        }
    }
}
