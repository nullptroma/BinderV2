using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.ScriptCommand;
using InterpreterScripts.InterpretationFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InterpreterScripts.Exceptions;
using System.ComponentModel;
using InterpreterScripts.InterpretationFunctions.Standart;

namespace InterpreterScripts.SyntacticConstructions.Constructions
{
    class FunctionDefinition : ISyntacticConstruction
    {
        public string Description { get { return "func Name(par1, par2...)\n{\n  <скрипт>\n} - объявляет функцию с именем Name, параметрами в скобках. Параметры можно использовать в теле функции."; } }

        public Task<object> TryExecute(CommandModel cmd, InterpretationData data)
        {
            if (IsValidConstruction(cmd, data))
                return Execute(cmd, data);
            return null;
        }

        private Task<object> Execute(CommandModel cmd, InterpretationData data)
        {
            return Task.Factory.StartNew(new Func<object>(() =>
            {
                CommandModel newModel = new CommandModel(cmd.Command.Remove(0, "func".Length).Trim());
                string name = newModel.KeyWord;
                string script = cmd.Command;
                int bracerIndex = script.IndexOf('{');
                script = script.Substring(bracerIndex + 1, (script.Length-1 ) - bracerIndex - 1).Trim();
                var buf = new UserFunc(name, newModel.GetParameters(), script);
                data.InterpretationFuncs.RemoveAll(func=>func.Name == buf.Name);
                data.InterpretationFuncs.Add(buf);
                return null;
            }), TaskCreationOptions.AttachedToParent);
        }

        private bool IsValidConstruction(CommandModel cmd, InterpretationData data)
        {
            return cmd.Command.StartsWith("func");
        }
    }
}
