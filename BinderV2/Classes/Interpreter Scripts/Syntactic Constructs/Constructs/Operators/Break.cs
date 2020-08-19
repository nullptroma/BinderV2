using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.ScriptCommand;
using InterpreterScripts.SyntacticConstructions.Constructions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterScripts.SyntacticConstructions.Constructions
{
    class Break : ISyntacticConstruction
    {
        public Task<object> Execute(CommandModel cmd, InterpretationData data)
        {
            return Task.Run(new Func<object>(() => throw new BreakException()));
        }

        public bool IsValidConstruction(CommandModel cmd, InterpretationData data)
        {
            return cmd.Command.StartsWith("break");
        }
    }
}
