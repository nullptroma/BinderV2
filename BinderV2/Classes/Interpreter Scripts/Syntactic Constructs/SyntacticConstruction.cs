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
        Task<object> Execute(CommandModel cmd,InterpretationData data);
        bool IsValidConstruction(CommandModel cmd, InterpretationData data);
    }
}
