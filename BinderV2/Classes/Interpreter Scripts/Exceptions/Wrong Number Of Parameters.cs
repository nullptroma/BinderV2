using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinderV2.Interpreter.Script.UserFunc.Exceptions
{
    public class WrongNumberOfParameters : Exception
    {
        public int CurrentNumOfParams { get; private set; }
        public int ExpectedNumOfParams { get; private set; }

        public WrongNumberOfParameters(int cnop, int enop)
        {
            CurrentNumOfParams = cnop;
            ExpectedNumOfParams = enop;
        }
    }
}
