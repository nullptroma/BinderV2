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
        public string Description { get { return "Name - получает значение переменной в пределах данного скрипта"; } }

        public Task<object> TryExecute(CommandModel cmd, InterpretationData data)
        {
            if (IsValidConstruction(cmd, data))
                return Execute(cmd, data);
            return null;
        }

        private Task<object> Execute(CommandModel cmd, InterpretationData data)
        {
            return Task.Run(new Func<object>(() =>
            {
                return data.Vars[cmd.Command];
            }));
        }

        private bool IsValidConstruction(CommandModel cmd, InterpretationData data)
        {
            return data.Vars.HasVar(cmd.Command.Trim());
        }
    }
}
