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
using System.Windows.Input;

namespace BinderV2
{
    public static class Test
    {
        public static void RunTest()
        {

            Task.Run(()=> 
            {
                Stopwatch sw = new Stopwatch();
                string script = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Новый текстовый документ.txt");
                sw.Start();
                Interpreter.ExecuteScript(script);
                sw.Stop();
                MessageBox.Show("Время в мс: " + sw.ElapsedMilliseconds, "Тест пройден ");
            });
        }

    }
}
