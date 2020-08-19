using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.InterpretationScriptData.CustomFunctions;
using InterpreterScripts.ScriptCommand;
using InterpreterScripts.InterpretationScriptData.StandartFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterpreterScripts.Exceptions;
using System.ComponentModel;
using InterpreterScripts.FuncAttributes;

namespace InterpreterScripts.SyntacticConstructions.Constructions
{
    class FunctionDefinition : ISyntacticConstruction
    {
        public Task<object> Execute(CommandModel cmd, InterpretationData data)
        {
            return Task.Factory.StartNew(new Func<object>(() =>
            {
                CommandModel newModel = new CommandModel(cmd.Command.Remove(0, "func".Length).Trim());
                string name = newModel.KeyWord;
                string script = cmd.Command;
                int bracerIndex = script.IndexOf('{');
                script = script.Substring(bracerIndex + 1, (script.Length-1 ) - bracerIndex - 1).Trim();
                var buf = new UserFunc(name, newModel.GetParameters(), script);
                data.CustomFunctions.Add(buf);
                return null;
            }), TaskCreationOptions.AttachedToParent);
        }

        public bool IsValidConstruction(CommandModel cmd, InterpretationData data)
        {
            return cmd.Command.StartsWith("func");
        }
    }
}
