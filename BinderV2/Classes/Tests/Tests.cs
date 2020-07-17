using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BinderV2.Tests
{
    public static class Tests
    {
        public static void RunTest()
        {
            Stopwatch sw = new Stopwatch();
            string str = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+@"\testing.txt");
            sw.Start();
            Interpreter.Script.ScriptTools.ReplaceParams(str, new string[] { "30" }, new string[] { "100500" });
            sw.Stop();
            MessageBox.Show("Тесты пройдены за "+ sw.ElapsedMilliseconds + " мс");
        }
    }
}
