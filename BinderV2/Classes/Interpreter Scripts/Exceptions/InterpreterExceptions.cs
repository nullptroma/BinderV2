using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinderV2.Interpreter.Script
{
    public class IncorrectScriptDesign :Exception
    {
        public IncorrectScriptDesign(string msg) : base(msg)
        { }
    }
}
