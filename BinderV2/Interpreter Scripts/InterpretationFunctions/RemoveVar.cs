using InterpreterScripts.InterpretationFunctions.Standart;
using InterpreterScripts.InterpretationScriptData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterScripts.InterpretationFunctions
{
    class RemoveVar : IInterpreterFunction
    {
        public string Name { get { return "RemoveVar"; } }
        public string Description { get { return "RemoveVar(name) - удаляет переменную name."; } }
        public string GroupName { get { return "ScriptRuntimeControl"; } }
        public FuncType ReturnType { get { return FuncType.Boolean; } }


        public Task<object> GetResult(string[] parameters, InterpretationData data)
        {
            return Task.Run(new Func<object>(() =>
            {
                return data.Vars.RemoveVar((string)Interpreter.ExecuteCommand(string.Join("", parameters), data));
            }));
        }
    }
}
