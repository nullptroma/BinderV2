using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterScripts.Exceptions
{
    class NotFoundScriptConstructException : Exception
    {
        public NotFoundScriptConstructException(string msg) : base(msg)
        {
        }
    }

    class ConversionFailedException : Exception
    {
        public ConversionFailedException(string msg) : base(msg)
        {
        }
    }

    class NotFoundDescriptionAttributeException : Exception
    {
    }

    class MethodIsNotStaticException : Exception
    {
    }

    public class IncorrectScriptDesignException : Exception
    {
        public IncorrectScriptDesignException(string msg) : base(msg)
        { }
    }

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
