using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterScripts.SyntacticConstructions.Constructions.Exceptions
{
    public class ReturnException : Exception
    {
        public object ReturnValue { get; private set; }

        public ReturnException(object value) : base()
        {
            ReturnValue = value;
        }
    }

    public class BreakException : Exception
    {
        public BreakException() : base()
        { }
    }
}
