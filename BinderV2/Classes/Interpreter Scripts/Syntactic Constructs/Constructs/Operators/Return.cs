using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.InterpretationScriptData.CustomFunctions;
using InterpreterScripts.ScriptCommand;
using InterpreterScripts.SyntacticConstructions.Constructions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterpreterScripts.SyntacticConstructions.Constructions
{
    class Return : ISyntacticConstruction
    {
        public string Description { get { return "return value - возвращает значение value из функции."; } }

        public Task<object> Execute(CommandModel cmd, InterpretationData data)
        {
            return Task.Run(new Func<object>(() => throw new ReturnException(Interpreter.ExecuteCommand(cmd.Command.Remove(0, "return".Length).Trim()))));
        }

        public bool IsValidConstruction(CommandModel cmd, InterpretationData data)
        {
            return cmd.Command.StartsWith("return");
        }
    }
}
