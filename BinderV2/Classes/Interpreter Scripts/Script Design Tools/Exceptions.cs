using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterScripts.Script.Exceptions
{
    public class IncorrectScriptDesignException : Exception
    {
        public IncorrectScriptDesignException(string msg) : base(msg)
        { }
    }
}
