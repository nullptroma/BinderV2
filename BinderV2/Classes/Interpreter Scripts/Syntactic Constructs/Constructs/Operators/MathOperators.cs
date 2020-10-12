using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.ScriptCommand;
using InterpreterScripts.SyntacticConstructions.Constructions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterScripts.SyntacticConstructions.Constructions
{
    class MathOperators : ISyntacticConstruction
    {
        public string Description { get { return "*, /, -, + - стандартные математические операторы."; } }

        public Task<object> Execute(CommandModel cmd, InterpretationData data)
        {
            return Task.Run(new Func<object>(() =>
            {
                string expression = SyncTrimBrackets(cmd.Command.Replace(" ", ""));
                int generalIndex = GetGeneralOperatorIndex(expression);
                char operatorSim = expression[generalIndex];
                object leftValue = Interpreter.ExecuteCommand(expression.Substring(0, generalIndex).Trim(), data);
                object rightValue = Interpreter.ExecuteCommand(expression.Substring(generalIndex + 1, expression.Length - 1 - generalIndex).Trim(), data);
                switch (operatorSim)
                {
                    case '+':
                        return UniversalSum(leftValue, rightValue);
                    case '-':
                        return UniversalMinus(leftValue, rightValue);
                    case '*':
                        return UniversalMult(leftValue, rightValue);
                    case '/':
                        return UniversalDiv(leftValue, rightValue);
                    default:
                        throw new InvalidCastException();
                }
            }));
        }

        //все типы данных
        //string
        //double
        //int
        //bool
        private object UniversalSum(object left, object right)
        {
            if (left is int && right is int)
                return (int)left + (int)right;
            else if (left is double && right is int)
                return (double)left + (int)right;
            else if (left is int && right is double)
                return (int)left + (double)right;
            else if (left is double && right is double)
                return (double)left + (double)right;
            else if (left is bool && right is bool)
                return (bool)left || (bool)right;
            else
                return left.ToString() + right.ToString();
        }

        private object UniversalMinus(object left, object right)
        {
            if (left is int && right is int)
                return (int)left - (int)right;
            else if (left is double && right is int)
                return (double)left - (int)right;
            else if (left is int && right is double)
                return (int)left - (double)right;
            else if (left is double && right is double)
                return (double)left - (double)right;
            else if (left is bool && right is bool)
                return !((bool)left || (bool)right);
            throw new InvalidCastException();
        }

        private object UniversalMult(object left, object right)
        {
            if (left is int && right is int)
                return (int)left * (int)right;
            else if (left is double && right is int)
                return (double)left * (int)right;
            else if (left is int && right is double)
                return (int)left * (double)right;
            else if (left is double && right is double)
                return (double)left * (double)right;
            else if (left is bool && right is bool)
                return (bool)left ^ (bool)right;
            throw new InvalidCastException();
        }

        private object UniversalDiv(object left, object right)
        {
            if (left is int && right is int)
                return (int)left / (int)right;
            else if (left is double && right is int)
                return (double)left / (int)right;
            else if (left is int && right is double)
                return (int)left / (double)right;
            else if (left is double && right is double)
                return (double)left / (double)right;
            else if (left is bool && right is bool)
                return !((bool)left ^ (bool)right);
            throw new InvalidCastException();
        }

        public bool IsValidConstruction(CommandModel cmd, InterpretationData data)
        {
            string expression = SyncTrimBrackets(cmd.Command.Replace(" ", ""));
            return GetGeneralOperatorIndex(expression) != -1;
        }

        private int GetGeneralOperatorIndex(string expression)
        {
            int generalOperatorIndex = -1;
            int countBrackets = 0;//текущий уровень скобки
            bool divOrMultFinded = false;
            for (int i = expression.Length-1; i >= 0; i--)
            {
                if (expression[i] == '(')
                    countBrackets--;
                else if (expression[i] == ')')
                    countBrackets++;

                if (countBrackets == 0)
                {
                    if (expression[i] == '+' || expression[i] == '-')
                    {
                        generalOperatorIndex = i;
                        break;
                    }
                    else if ((expression[i] == '*' || expression[i] == '/') && !divOrMultFinded)
                    {
                        generalOperatorIndex = i;
                        divOrMultFinded = true;
                    }
                }
            }

            return generalOperatorIndex;
        }

        private string SyncTrimBrackets(string str)
        {
            if (str.Length < 2 || str[0] != '(' || str[str.Length - 1] != ')')
                return str;

            int countBrackets = 0;//текущий уровень скобки
            for (int i = 1; i < str.Length - 1; i++)
            {
                if (str[i] == '(')
                    countBrackets++;
                else if (str[i] == ')')
                    countBrackets--;

                if (countBrackets == -1)
                    return str;
            }
            if (countBrackets == 0)
            {
                string answer = str.Remove(str.Length - 1, 1).Remove(0, 1);
                string nextAnswer = SyncTrimBrackets(answer);
                return answer == nextAnswer ? answer : nextAnswer;
            }
            return str;
        }
    }
}
