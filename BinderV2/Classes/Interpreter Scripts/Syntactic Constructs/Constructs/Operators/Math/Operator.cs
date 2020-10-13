using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterScripts.SyntacticConstructions.Constructions.InterpreterOperators
{
    class Operator
    {
        public string StringOperator { get; private set; }
        public int PriorityLevel { get; private set; }
        public string StringLeft { get; set; }
        public string StringRight { get; set; }

        public Operator()
        {
            StringOperator = "";
            PriorityLevel = 7;
        }

        public bool ChangeToLessPriorityOperator(string op)
        {
            int newOperatorPriority = GetOperatorPriority(op);
            if (newOperatorPriority < PriorityLevel)
            {
                StringOperator = op;
                PriorityLevel = newOperatorPriority;
                return true;
            }
            return false;
        }

        public static int GetOperatorPriority(string op)
        {
            switch (op)
            {
                case "&&":
                case "||":
                    return 1;
                case "==":
                case "!=":
                    return 2;
                case "<":
                case "<=":
                case ">=":
                case ">":
                    return 3;
                case "-":
                case "+":
                    return 4;
                case "*":
                case "%":
                case "^":
                case "/":
                    return 5;
                case "!":
                    return 6;

                default:
                    return 7;
            }
        }
    }
}
