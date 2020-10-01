using InterpreterScripts;
using InterpreterScripts.InterpretationScriptData.StandartFunctions;
using InterpreterScripts.Script;
using InterpreterScripts.ScriptCommand;
using InterpreterScripts.SyntacticConstructions.Constructions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using InterpreterScripts.TypeConverter;
using System.Threading;
using System.Net.Http.Headers;
using InterpreterScripts.InterpretationScriptData;
using System.Reflection;
using Utilities;

namespace BinderV2
{
    public static class Test
    {
        public static void RunTest()
        {
            Task.Run(()=> 
            {
                Interpreter.ExecuteCommand("Delay(1)");
                int sum = 0;
                for (int n = 1; n < 100; n+=10)
                {
                    sum += n;
                    Stopwatch sw = new Stopwatch();
                    string cmd = $"Delay({n})";
                    sw.Start();
                    Interpreter.ExecuteCommand(cmd);
                    sw.Stop();
                    if (sw.ElapsedMilliseconds != n)
                        MessageBox.Show("Ожидалось: " + n + "\nПолучено: " + sw.ElapsedMilliseconds);
                }
                MessageBox.Show("Тесты пройдены.", "sum = " + sum);
            });
        }

    }
}
