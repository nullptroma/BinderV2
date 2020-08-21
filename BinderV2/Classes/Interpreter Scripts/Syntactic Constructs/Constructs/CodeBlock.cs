using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.ScriptCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterScripts.SyntacticConstructions.Constructions
{
    class CodeBlock : ISyntacticConstruction
    {
        public string Description { get { return "{\n  <скрипт>\n} - простой блок кода, ни на что не влияет. Может использоваться с async."; } }

        public Task<object> Execute(CommandModel cmd, InterpretationData data)
        {
            return Task.Factory.StartNew(new Func<object>(() =>
            {
                Interpreter.ExecuteScript(cmd.Command.Substring(1, cmd.Command.Length-2), data);
                return null;
            }), TaskCreationOptions.AttachedToParent);
        }

        public bool IsValidConstruction(CommandModel cmd, InterpretationData data)
        {
            return cmd.Command.StartsWith("{") && cmd.Command.EndsWith("}");
        }
    }
}
