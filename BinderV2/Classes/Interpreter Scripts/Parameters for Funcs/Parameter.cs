using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinderV2.Interpreter.ParametersForFuncs
{
    public class Parameter
    {
        public string Name { get; private set; }

        public Parameter(string name)
        {
            Name = name;
        }
    }
}
