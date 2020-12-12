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
        public string Name { get { return "FuncDefinition"; } }
        public string Description { get { return "[description]\nfunc Name(par1, par2...)\n{\n  <скрипт>\n} - объявляет функцию с именем Name, параметрами в скобках. Параметры можно использовать в теле функции. Можно добавить описание(необязательно)."; } }

        public Task<object> TryExecute(CommandModel cmd, InterpretationData data)
        {
            if (IsValidConstruction(cmd))
                return Execute(cmd, data);
            return null;
        }

        private Task<object> Execute(CommandModel cmd, InterpretationData data)
        {
            return Task.Factory.StartNew(new Func<object>(() =>
            {
                string description = "";
                if (cmd.Command.StartsWith("["))
                {
                    description = cmd.Command.Substring(0, cmd.Command.IndexOf("func")-1).Trim();
                    description = description.Remove(description.Length-1,1).Remove(0,1);
                }

                CommandModel newModel = new CommandModel(cmd.Command.Substring(cmd.Command.IndexOf("func") + 4).Trim());
                string name = newModel.KeyWord;
                string script = cmd.Command;
                int bracerIndex = script.IndexOf('{');
                script = script.Substring(bracerIndex + 1, (script.Length-1 ) - bracerIndex - 1).Trim();
                var buf = new UserFunc(name, newModel.GetParameters(), script, description);
                data.InterpretationFuncs.RemoveAll(func=>func.Name == buf.Name);
                data.InterpretationFuncs.Add(buf);
                return null;
            }), TaskCreationOptions.AttachedToParent);
        }

        private bool IsValidConstruction(CommandModel cmd)
        {
            return cmd.Command.StartsWith("func") || cmd.Command.StartsWith("[");
        }
    }
}
