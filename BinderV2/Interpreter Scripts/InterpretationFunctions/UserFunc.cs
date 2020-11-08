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
using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.SyntacticConstructions.Constructions.Exceptions;

namespace InterpreterScripts.InterpretationFunctions
{
    public class UserFunc : IInterpreterFunction
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string GroupName { get { return "UserFuncs"; } }
        public FuncType ReturnType { get { return FuncType.Dynamic; } }

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

        public UserFunc(string name, string[] parameters, string script, string desc = "")
        {
            Name = name;
            Script = script;
            FuncsParamsNames = parameters;
            Description = Name + "(" + string.Join(", ", funcsParamsNames) + ")" + desc;
        }

        public Task<object> GetResult(string[] parameters, InterpretationData data)
        {
            return Task.Run(new Func<object>(() =>
            {
                for (int i = 0; i < funcsParamsNames.Length; i++)
                    data.Vars[funcsParamsNames[i]] = Interpreter.ExecuteCommand(parameters[i], data);
                try { Interpreter.ExecuteScript(Script, data); }
                catch (ReturnException e) { return e.ReturnValue; }
                catch (Exception) { return null; }
                return null;
            }));
        }

        public override string ToString()
        {
            return Name + $"({string.Join(", ", FuncsParamsNames)});" + "\n" + Script;
        }
    }


}
