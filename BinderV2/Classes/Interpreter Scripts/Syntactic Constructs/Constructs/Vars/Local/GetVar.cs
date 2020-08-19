using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.ScriptCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterpreterScripts.SyntacticConstructions.Constructions
{
    class GetVar : ISyntacticConstruction
    {
        public Task<object> Execute(CommandModel cmd, InterpretationData data)
        {
            return Task.Run(new Func<object>(() =>
            {
                return data.Vars[cmd.Command];
            }));
        }

        public bool IsValidConstruction(CommandModel cmd, InterpretationData data)
        {
            return data.Vars[cmd.Command] !=null;
        }
    }
}
