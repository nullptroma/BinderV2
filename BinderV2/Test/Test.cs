using InterpreterScripts;
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
                int count = 0;
                var data = new InterpretationData();
                sw.Start();
                while (sw.ElapsedMilliseconds <= 1000)
                {
                    Interpreter.ExecuteScript("0<1000000;", data);
                    count++;
                }
                
                sw.Stop();
                MessageBox.Show("Время в мс: " + sw.ElapsedMilliseconds, "Тест пройден " + count);
            });
        }

    }
}
