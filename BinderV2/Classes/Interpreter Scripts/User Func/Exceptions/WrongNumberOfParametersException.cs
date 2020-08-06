using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterScripts.UserFunc.Exceptions
{
    public class WrongNumberOfParametersException : Exception
    {
        public int CurrentNumOfParams { get; private set; }
        public int ExpectedNumOfParams { get; private set; }

        public WrongNumberOfParametersException(int currentNum, int expectedNum)
        {
            CurrentNumOfParams = currentNum;
            ExpectedNumOfParams = expectedNum;
        }
    }
}
