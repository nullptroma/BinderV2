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
                var data = new InterpretationData();
                
                sw.Start();
                Interpreter.ExecuteScript("count = 0;\n while(count < 1000000){count = count + 1;};", data);

                sw.Stop();
                MessageBox.Show("Время в мс: " + sw.ElapsedMilliseconds, "Кол-во " + data.Vars["count"]);
            });
        }

    }
}
