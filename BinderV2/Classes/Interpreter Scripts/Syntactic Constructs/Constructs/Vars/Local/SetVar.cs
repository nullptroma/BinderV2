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
    class SetVar : ISyntacticConstruction
    {
        public Task<object> Execute(CommandModel cmd, InterpretationData data)
        {
            return Task.Run(new Func<object>(()=> 
            {
                string script = cmd.Command;
                int assignmentIndex = GetCharIndexOutsideBrackets(script, '='); 
                string varName = script.Substring(0, assignmentIndex).Trim();
                string valueString = script.Substring(assignmentIndex+1, (script.Length-1) - assignmentIndex).Trim();
                object value = Interpreter.ExecuteCommand(valueString, data);
                data.Vars[varName] = value;
                return value;
            }));
        }

        public bool IsValidConstruction(CommandModel cmd, InterpretationData data)
        {
            return GetCharIndexOutsideBrackets(cmd.Command, '=')!=-1 && GetCharIndexOutsideBrackets(cmd.Command, '.') == -1;
        }

        private int GetCharIndexOutsideBrackets(string str, char ch)
        {
            int countMarks = 0;
            int countBraces = 0;
            int countBrackets = 0;
            bool findSpace = false;
            for (int assignmentIndex= 0; assignmentIndex < str.Length; assignmentIndex++)
            {
                if (str[assignmentIndex] == '\"')
                    countMarks++;
                else if (str[assignmentIndex] == '(')
                    countBrackets++;
                else if (str[assignmentIndex] == ')')
                    countBrackets--;
                else if (str[assignmentIndex] == '{')
                    countBraces++;
                else if (str[assignmentIndex] == '}')
                    countBraces--;

                if (countMarks % 2 == 0 && countBraces == 0 && countBrackets == 0)
                    if (str[assignmentIndex] == ch)
                        return assignmentIndex;

                if (str[assignmentIndex] == ' ')
                    findSpace = true;
                else if(findSpace)//если сейчас мы не на пробеле, но до этого он был
                    return -1;

                
            }
            return -1;
        }
    }
}
