using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Windows.Controls;
using InterpreterScripts.UserFunc.Exceptions;
using InterpreterScripts.Script;

namespace InterpreterScripts.UserFunc
{
    public class UserFunc
    {
        public string Name { get; private set; }
        private string Script { get; set; }
        private string[] funcsParamsNames;
        public string[] FuncsParamsNames
        {
            get { return funcsParamsNames; }
            set
            {
                if (value == null)
                    throw new NullReferenceException();
                funcsParamsNames = new string[value.Length];
                for (int i = 0; i < funcsParamsNames.Length; i++)
                    funcsParamsNames[i] = value[i];
            }
        }

        public UserFunc(string name, string script, string[] parameters)
        {
            Name = name;
            Script = script;
            FuncsParamsNames = parameters;
        }

        public string GetScriptWithParameters(params string[] parameters)
        {
            if (parameters.Length != FuncsParamsNames.Length)
                throw new WrongNumberOfParametersException(parameters.Length, FuncsParamsNames.Length);
            return ScriptTools.ReplaceParams(Script, FuncsParamsNames, parameters);
        }
    }
}
