using InterpreterScripts.Exceptions;
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
    class Repeat : ISyntacticConstruction
    {
        public string Name { get { return "Repeat"; } }
        public string Description { get { return "repeat(quant)\n{\n  <скрипт>\n} - выполняет скрипт quant раз."; } }

        public Task<object> TryExecute(CommandModel cmd, InterpretationData data)
        {
            if (IsValidConstruction(cmd, data))
                return Execute(cmd, data);
            return null;
        }

        private Task<object> Execute(CommandModel cmd, InterpretationData data)
        {
            return Task.Run(new Func<object>(() => 
            {
                var parameters = cmd.GetParameters();
                if (parameters.Length != 1)
                    throw new WrongNumberOfParametersException(parameters.Length, 1);
                object num = Interpreter.ExecuteCommand(parameters[0], data);
                if (!(num is int))
                    throw new ConversionFailedException("Не удаётся конвертировать " + parameters[0] + " в int.");
                int numberRepetitions = (int)num;
                string script = cmd.Command;
                int bracerIndex = script.IndexOf('{');
                script = script.Substring(bracerIndex + 1, (script.Length - 1) - bracerIndex - 1).Trim();
                while (numberRepetitions-- > 0)
                {
                    try { Interpreter.ExecuteScript(script, data); }
                    catch (BreakException) { break; }
                    catch (Exception e) { throw e; }//прокидываем выше
                }
                return null;
            }));
        }

        private bool IsValidConstruction(CommandModel cmd, InterpretationData data)
        {
            return cmd.Command.StartsWith("repeat");
        }
    }
}
