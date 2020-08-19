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
    class CheckVar : ISyntacticConstruction
    {
        public Task<object> Execute(CommandModel cmd, InterpretationData data)
        {
            return Task.Run(new Func<object>(() =>
            {
                var pars = cmd.GetParameters();
                if (pars.Length == 1)
                    return data.Vars.HasVar(Interpreter.ExecuteCommand(pars[0], data).ToString());
                return false;
            }));
        }

        public bool IsValidConstruction(CommandModel cmd, InterpretationData data)
        {
            return cmd.KeyWord == "CheckVar" && cmd.GetParameters().Length == 1;
        }
    }
}
