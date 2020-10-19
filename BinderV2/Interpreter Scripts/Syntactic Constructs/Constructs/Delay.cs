using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.ScriptCommand;
using InterpreterScripts.SyntacticConstructions;

namespace InterpreterScripts.SyntacticConstructions.Constructions
{
    public class Delay : ISyntacticConstruction
    {
        public string Description { get { return "Delay(int milliseconds) - приостанавливает выполнение скрипна на milliseconds."; } }

        public bool IsValidConstruction(CommandModel cmd, InterpretationData data)
        {
            return cmd.KeyWord.StartsWith("Delay") && cmd.GetParameters().Length == 1;
        }

        public Task<object> Execute(CommandModel cmd, InterpretationData data)
        {
            return Task.Run<object>(()=> 
            {
                int milliseconds = (int)Interpreter.ExecuteCommand(cmd.GetParameters()[0], data);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.ElapsedMilliseconds < milliseconds)
                {
                    if (milliseconds - sw.ElapsedMilliseconds > 50)
                        Task.Delay(1).Wait();
                }
                sw.Stop();
                return milliseconds;
            });
        }
    }
}
