using InterpreterScripts.Exceptions;
using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.ScriptCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InterpreterScripts.SyntacticConstructions.Constructions
{
    class IfThenConstruction : ISyntacticConstruction
    {
        public Task<object> Execute(CommandModel cmd, InterpretationData data)
        {
            return Task.Run(new Func<object>(()=> {
                List<ConditionAction> conditions = GetConditionsAndActions(cmd);
                foreach (ConditionAction ca in conditions)
                {
                    if (ca.Condition.Length == 0)//если это "else" без условия
                    {
                        Interpreter.ExecuteScript(ca.Action, data);
                        break;
                    }

                    object value = Interpreter.ExecuteCommand(ca.Condition);
                    if (value is bool)
                    {
                        if ((bool)value)
                        {
                            Interpreter.ExecuteScript(ca.Action, data);
                            break;
                        }
                    }
                    else
                    {
                        throw new ConversionFailedException("Can not convert " + value.GetType().Name + " to Bool.");
                    }
                }
                return null;
            }));
        }

        private List<ConditionAction> GetConditionsAndActions(CommandModel cmd)
        {
            List<ConditionAction> answer = new List<ConditionAction>();
            string script = cmd.Command+"\0";
            int countRoundBrackets = 0;
            int countBraces = 0;

            StringBuilder ConditionBuffer = new StringBuilder();
            StringBuilder ActionBuffer = new StringBuilder();
            bool actionWrited = false;
            foreach(char ch in script)
            {
                if (countRoundBrackets >= 1 && countBraces == 0)
                {
                    ConditionBuffer.Append(ch);
                }
                else if (countBraces != 0)
                {
                    ActionBuffer.Append(ch);
                    actionWrited = true;
                }
                else 
                {
                    if (actionWrited) 
                    {
                        answer.Add(new ConditionAction(RemoveLastSimbol(ConditionBuffer.ToString()).Trim(), RemoveLastSimbol(ActionBuffer.ToString()).Trim()));
                        ConditionBuffer.Clear();
                        ActionBuffer.Clear();
                        actionWrited = false;
                    }
                    
                }

                if (ch == '(')
                    countRoundBrackets++;
                else if (ch == ')')
                    countRoundBrackets--;
                else if (ch == '{')
                    countBraces++;
                else if (ch == '}')
                    countBraces--;
            }

            return answer;
        }

        private string RemoveLastSimbol(string s)
        {
            return s.Length >= 1 ? s.Remove(s.Length - 1, 1) : s;
        }

        public bool IsValidConstruction(CommandModel cmd, InterpretationData data)
        {
            return cmd.KeyWord == "if" && cmd.GetParameters().Length == 1;
        }
    }

    class ConditionAction
    {
        public string Condition { get; private set; }
        public string Action { get; private set; }

        public ConditionAction(string condition, string action)
        {
            Condition = condition;
            Action = action;
        }

        public override string ToString()
        {
            return Condition + "\n" + Action;
        }
    }
}
