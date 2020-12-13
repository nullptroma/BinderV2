using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterScripts.InterpretationFunctions
{
    public interface IInterpreterFunction
    {
        string Name { get; }
        string Description { get; }
        string GroupName { get; }
        FuncType ReturnType { get; }

        Task<object> GetResult(object[] parameters, InterpretationScriptData.InterpretationData data);
    }
}
