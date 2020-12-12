using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.ScriptCommand;
using InterpreterScripts.SyntacticConstructions;

namespace InterpreterScripts.SyntacticConstructions.Constructions
{
    public class Delay : ISyntacticConstruction
    {
        public string Name { get { return "Delay"; } }
        public string Description { get { return "Delay(int milliseconds) - приостанавливает выполнение скрипна на milliseconds."; } }

        public Task<object> TryExecute(CommandModel cmd, InterpretationData data)
        {
            if (IsValidConstruction(cmd, data))
                return Execute(cmd, data);
            return null;
        }

        private bool IsValidConstruction(CommandModel cmd, InterpretationData data)
        {
            return cmd.KeyWord.StartsWith("Delay") && cmd.GetParameters().Length == 1;
        }

        private Task<object> Execute(CommandModel cmd, InterpretationData data)
        {
            return Task.Factory.StartNew<object>(()=> 
            {
                int milliseconds = (int)Interpreter.ExecuteCommand(cmd.GetParameters()[0], data);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.ElapsedMilliseconds < milliseconds)
                {
                    if (data.Stopper.IsStopped)
                        throw new StopException();
                    if (milliseconds - sw.ElapsedMilliseconds > 1100)
                        Thread.Sleep(1000);
                    if (milliseconds - sw.ElapsedMilliseconds > 50)
                        Task.Delay(1).Wait();
                }
                sw.Stop();
                return milliseconds;
            }, TaskCreationOptions.LongRunning);
        }
    }
}
