using InterpreterScripts.Exceptions;
using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.ScriptCommand;
using InterpreterScripts.SyntacticConstructions.Constructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterpreterScripts.SyntacticConstructions
{
    public static class SyntacticConstructionsManager
    {
        static readonly List<ISyntacticConstruction> syntacticConstructs = GetSyntacticConstructs().ToList();

        public static ISyntacticConstruction[] GetSyntacticConstructs()
        {
            //сюда добавлять конструкции
            return new ISyntacticConstruction[] 
            { 
                new Delay(),
                new Return(), 
                new Break(), 
                
                new IfThenConstruction(),

                new While(),
                new Repeat(),
                new CodeBlock(),
                new ParallelRepeat(),

                new FunctionDefinition(),

                new WithOtherData(),

                new GetVar(),
                new SetVar(),
                new CheckVar(),

                new MathOperators(), 
            };
        }

        public static bool TryExecute(CommandModel command, InterpretationData data, out Task<object> result)
        {
            foreach (var sc in syntacticConstructs)
            {
                result = sc.TryExecute(command, data);
                if (result != null)
                    return true;
            }
            result = null;
            return false;
        }
    }
}
