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
            string script = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+ @"\Новый текстовый документ2.txt") ;
            int count = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.ElapsedMilliseconds < 1000)
            {
                Interpreter.ExecuteScript(script);
                count++;
            }
            sw.Stop();
            MessageBox.Show("Результат: " + count, "Тест скорости за "+ sw.ElapsedMilliseconds);
        }

        private static long CountTicks(Action action)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            action();
            sw.Stop();
            return sw.ElapsedTicks;
        }

        private static object TestAdditional(params object[] strs)
        {
            MessageBox.Show(string.Join("", strs), "Успех");
            return strs;
        }

        private static List<Type> expectedTypes = new List<Type>() { typeof(int), typeof(double), typeof(bool) };
        private static object GetSpecificTypeValue(string strValue)
        {
            foreach (var t in expectedTypes)
            {
                var converter = TypeDescriptor.GetConverter(t);

                if (!(converter.CanConvertFrom(typeof(string))
                    && converter.IsValid(strValue))) continue;

                var answer = converter.ConvertFromInvariantString(strValue);

                if (answer != null)
                    return answer;
            }
            return null;
        }
    }
}
