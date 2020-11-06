using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterScripts
{
    class StopException : Exception
    {
        public StopException() : base()
        { }
    }
}
