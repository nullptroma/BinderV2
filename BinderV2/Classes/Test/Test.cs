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
                string script = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Новый текстовый документ2.txt");
                int count = 0;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.ElapsedMilliseconds < 1000)
                {
                    Interpreter.ExecuteScript(script);
                    count++;
                }
                sw.Stop();
                MessageBox.Show("Результат: " + count, "Тест скорости за " + sw.ElapsedMilliseconds);
            });

            
        }

    }
}
