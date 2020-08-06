using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterpreterScripts.Funcslibrary;

namespace InterpreterScripts
{
    public static class Interpreter
    {
        public static void ExecuteScript(string script, Function[] additionalFunctions)
        {
            FuncsLibManager.GetLibrary();
        }
    }
}
