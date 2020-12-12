using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.ScriptCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterScripts.SyntacticConstructions
{
    public interface ISyntacticConstruction
    {
        string Name { get; }
        string Description { get; }
        Task<object> TryExecute(CommandModel cmd, InterpretationData data);
    }
}
