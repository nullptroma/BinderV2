using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.Script;
using InterpreterScripts.ScriptCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterpreterScripts.SyntacticConstructions.Constructions
{
    class SetVar : ISyntacticConstruction
    {
        public string Description { get { return "Name = value - задаёт переменную Name в пределах данного скрипта."; } }

        public Task<object> Execute(CommandModel cmd, InterpretationData data)
        {
            return Task.Run(new Func<object>(()=> 
            {
                string script = cmd.Command;
                int assignmentIndex = ScriptTools.GetCharIndexOutsideBrackets(script, '='); 
                string varName = script.Substring(0, assignmentIndex).Trim();
                string valueString = script.Substring(assignmentIndex+1, (script.Length-1) - assignmentIndex).Trim();
                object value = Interpreter.ExecuteCommand(valueString, data);
                data.Vars[varName] = value;
                return value;
            }));
        }

        public bool IsValidConstruction(CommandModel cmd, InterpretationData data)
        {
            int index = ScriptTools.GetCharIndexOutsideBrackets(cmd.Command, '=');
            if (index == 0 || index == cmd.Command.Length - 1 || index == -1)
                return false;
            if (cmd.Command[index - 1] == '=' || cmd.Command[index + 1] == '=' || cmd.Command[index - 1] == '!')
                return false;
            return ScriptTools.GetCharIndexOutsideBrackets(cmd.Command, '.') == -1;
        }
    }
}
