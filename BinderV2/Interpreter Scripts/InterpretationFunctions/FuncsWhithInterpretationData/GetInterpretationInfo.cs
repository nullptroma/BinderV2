using InterpreterScripts.InterpretationFunctions.Standart;
using InterpreterScripts.InterpretationScriptData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InterpreterScripts.InterpretationFunctions
{
    class GetInterpretationInfo : IInterpreterFunction
    {
        public string Name { get { return "GetInterpretationInfo"; } }
        public string Description { get { return "GetInterpretationInfo() - возвращает информацию о текущей интерпретации."; } }
        public string GroupName { get { return "ScriptRuntimeControl"; } }
        public FuncType ReturnType { get { return FuncType.String; } }
        

        public Task<object> GetResult(string[] parameters, InterpretationData data)
        {
            return Task.Run(new Func<object>(() =>
            {
                if (parameters.Length > 0)
                {
                    object parameterData = Interpreter.ExecuteCommand(parameters[0], data);
                    if (parameterData is InterpretationData newData)
                        return newData.ToString();
                }
                return data.ToString();
            }));
        }
    }
}
