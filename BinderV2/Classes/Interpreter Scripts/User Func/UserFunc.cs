using BinderV2.Interpreter.Script.UserFunc.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Windows.Controls;

namespace BinderV2.Interpreter.Script.UserFunc
{
    public class UserFunc
    {
        public string Name { get; private set; }
        private string Script { get; set; }
        private string[] paramsNames;
        public string[] ParamsNames 
        {
            get { return paramsNames; }
            set
            {
                if (value == null)
                    throw new NullReferenceException();
                paramsNames = new string[value.Length];
                for (int i = 0; i < paramsNames.Length; i++)
                    paramsNames[i] = value[i];
            }
        }

        public UserFunc(string name, string script, string[] parameters)
        {
            Name = name;
            Script = script;
            ParamsNames = parameters;
        }

        public string GetScriptWithParameters(params string[] parameters)
        {
            if (parameters.Length != ParamsNames.Length)
                throw new WrongNumberOfParameters(parameters.Length, ParamsNames.Length);
            return ScriptTools.ReplaceParams(Script, ParamsNames, parameters);
        }
    }
}
