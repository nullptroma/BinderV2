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
        public string Description { get { return "if(<условие1>)\n{\n  <скрипт1>\n}\nelse if(<условие2>)\n{\n  <скрипт1>\n}\nelse\n{\n  <скрипт3>\n} - конструкция if-else. Может иметь любую конфигурацию. После последнего блока кода нужно ставить ;."; } }

        public Task<object> TryExecute(CommandModel cmd, InterpretationData data)
        {
            if (IsValidConstruction(cmd))
                return Execute(cmd, data);
            return null;
        }

        private Task<object> Execute(CommandModel cmd, InterpretationData data)
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

                    object value = Interpreter.ExecuteCommand(ca.Condition, data);
                    if (value is bool boolValue)
                    {
                        if (boolValue)
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

        private bool IsValidConstruction(CommandModel cmd)
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
