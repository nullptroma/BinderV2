using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.ScriptCommand;
using InterpreterScripts.SyntacticConstructions.Constructions.Exceptions;
using InterpreterScripts.SyntacticConstructions.Constructions.InterpreterOperators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;

namespace InterpreterScripts.SyntacticConstructions.Constructions
{
    class MathOperators : ISyntacticConstruction
    {
        public string Description { get { return "*, /, -, + - стандартные математические операторы" + Environment.NewLine + "%, ^ - остатот от деления, возведение в степень" + Environment.NewLine + "||, &&, ! - логические или, и, не" + Environment.NewLine + "==, !=, <, <=, >=, > - операторы сравнения."; } }

        public Task<object> TryExecute(CommandModel cmd, InterpretationData data)
        {
            var generalOperator = GetGeneralOperator(cmd.Command);
            if (generalOperator.PriorityLevel < 7)
                return Execute(cmd, data, generalOperator);
            return null;
        }
        private Task<object> Execute(CommandModel cmd, InterpretationData data, Operator generalOperator)
        {
            return Task.Run(new Func<object>(() =>
            {
                string expression = cmd.Command;
                object leftValue = Interpreter.ExecuteCommand(generalOperator.StringLeft, data);
                object rightValue = Interpreter.ExecuteCommand(generalOperator.StringRight, data);
                switch (generalOperator.StringOperator)
                {
                    case "+":
                        return Operators.UniversalSum(leftValue, rightValue);
                    case "-":
                        return Operators.UniversalMinus(leftValue, rightValue);
                    case "*":
                        return Operators.UniversalMult(leftValue, rightValue);
                    case "^":
                        return Operators.UniversalPow(leftValue, rightValue);
                    case "/":
                        return Operators.UniversalDiv(leftValue, rightValue);
                    case "%":
                        return Operators.UniversalRemainder(leftValue, rightValue);
                    case "==":
                        return Operators.UniversalEqual(leftValue, rightValue);
                    case "!=":
                        return Operators.UniversalUnEqual(leftValue, rightValue);

                    case "<":
                        return Operators.UniversalLess(leftValue, rightValue);
                    case "<=":
                        return Operators.UniversalLess(leftValue, rightValue) || Operators.UniversalEqual(leftValue, rightValue);
                    case ">=":
                        return Operators.UniversalMore(leftValue, rightValue) || Operators.UniversalEqual(leftValue, rightValue);
                    case ">":
                        return Operators.UniversalMore(leftValue, rightValue);
                    case "||":
                        return (bool)rightValue || (bool)leftValue;
                    case "&&":

                        return (bool)rightValue && (bool)leftValue;
                    case "!":
                        return !((bool)rightValue);
                    default:
                        throw new InvalidCastException();
                }
            }));
        }


        private Operator GetGeneralOperator(string expression)
        {
            int countBrackets = 0;//текущий уровень скобки
            int countMarks = 0;//текущий уровень скобки
            Operator resultOperator = new Operator();
            int operatorIndex = -1;
            for (int i = expression.Length - 1; i >= 0; i--)
            {
                if (expression[i] == '(')
                    countBrackets--;
                else if (expression[i] == ')')
                    countBrackets++;
                else if (expression[i] == '\"')
                    countMarks++;

                if (countBrackets == 0 && countMarks % 2 == 0)//если мы не в скобках и не в кавычках
                {
                    if (i > 0)
                    {
                        if (resultOperator.ChangeToLessPriorityOperator(expression[i - 1].ToString() + expression[i].ToString()))
                        {
                            i--;
                            operatorIndex = i;
                            continue;
                        }
                    }
                    if (resultOperator.ChangeToLessPriorityOperator(expression[i].ToString()))
                        operatorIndex = i;
                }
            }
            if (resultOperator.PriorityLevel < 7)
            {
                resultOperator.StringLeft = expression.Substring(0, operatorIndex);
                resultOperator.StringRight = expression.Substring(operatorIndex + resultOperator.StringOperator.Length);
            }
            return resultOperator;
        }


    }
}
