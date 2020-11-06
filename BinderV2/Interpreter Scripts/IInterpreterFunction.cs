using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterScripts
{
    public interface IInterpreterFunction
    {
        string Name { get; }

        Task<object> GetResult(string[] parameters, InterpretationScriptData.InterpretationData data);
    }
}
