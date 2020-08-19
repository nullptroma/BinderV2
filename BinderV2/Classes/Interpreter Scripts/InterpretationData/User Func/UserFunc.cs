using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using InterpreterScripts.Exceptions;
using InterpreterScripts.Script;
using InterpreterScripts.ScriptCommand;
using InterpreterScripts.SyntacticConstructions.Constructions.Exceptions;

namespace InterpreterScripts.InterpretationScriptData.CustomFunctions
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

        public UserFunc(string name, string[] parameters, string script)
        {
            Name = name;
            Script = script;
            FuncsParamsNames = parameters;
        }

        public Task<object> GetResult(string[] parameters, InterpretationData data)
        {
            return Task.Run(new Func<object>(() =>
            {
                try { Interpreter.ExecuteScript(GetScriptWithParameters(parameters), data); }
                catch (ReturnException e) { return e.ReturnValue; }
                catch (Exception) { return null; }
                return null;
            }));
        }

        public override string ToString()
        {
            return Name+$"({string.Join(", ", FuncsParamsNames)});" + "\n" + Script;
        }

        private string GetScriptWithParameters(params string[] parameters)
        {
            if (parameters.Length != FuncsParamsNames.Length)
                throw new WrongNumberOfParametersException(parameters.Length, FuncsParamsNames.Length);
            Dictionary<string, string> paramsToReplace = new Dictionary<string, string>();
            for (int i = 0; i < FuncsParamsNames.Length; i++)
                paramsToReplace.Add(FuncsParamsNames[i], parameters[i]);
            var answer = ScriptTools.ReplaceParams(Script, paramsToReplace);
            return answer;
        }
    }

    
}
