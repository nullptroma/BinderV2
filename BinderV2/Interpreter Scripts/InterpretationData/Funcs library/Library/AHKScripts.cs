using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Input;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.CodeDom;
using InterpreterScripts.FuncAttributes;
using System.ComponentModel;
using AutoHotkey.Interop;

namespace InterpreterScripts.InterpretationScriptData.StandartFunctions.Library
{
    static public class AHKScripts
    {
        private static AutoHotkeyEngine ahk = AutoHotkeyEngine.Instance;

        [FuncGroup("AutoHotKey")]
        [Description("AHKExecute(string code) - выполняет код ahk.")]
        public static object AHKExecute(params object[] ps)
        {
            try
            {
                if (ps.Length == 0)
                    return ps;
                ahk.ExecRaw(ps[0].ToString());
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
            return ps;
        }

        [FuncGroup("AutoHotKey")]
        [Description("AHKKeyDown(string key) - нажимает кнопку с помощью AHK.")]
        public static object AHKKeyDown(params object[] ps)
        {
            try
            {
                if (ps.Length == 0)
                    return ps;
                ahk.ExecRaw("Send {" + ps[0].ToString() + " down}");
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
            return ps;
        }

        [FuncGroup("AutoHotKey")]
        [Description("AHKKeyUp(string key) - поднимает кнопку с помощью AHK.")]
        public static object AHKKeyUp(params object[] ps)
        {
            try
            {
                if (ps.Length == 0)
                    return ps;
                ahk.ExecRaw("Send {" + ps[0].ToString() + " up}");
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
            return ps;
        }

        [FuncGroup("AutoHotKey")]
        [Description("AHKSend(string text/key) - отправляет текст/кнопку.")]
        public static object AHKSend(params object[] ps)
        {
            try
            {
                if (ps.Length == 0)
                    return ps;
                ahk.ExecRaw("Send "+ ps[0].ToString());
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
            return ps;
        }

        [FuncGroup("AutoHotKey")]
        [Description("AHKWriteText(string text, int delay) - пишет текст посимвольно через AHK.")]
        public static object AHKWriteText(params object[] ps)
        {
            try
            {
                int delay = (int)ps[1];
                foreach (var sim in ps[0].ToString())
                {
                    ahk.ExecRaw($"Send {{{sim}}}");
                    Interpreter.ExecuteCommand($"Delay({delay})");
                }
            }
            catch (IndexOutOfRangeException) { MessageBox.Show("В AHKWriteText не передан текст."); }
            catch (InvalidCastException) { MessageBox.Show("В AHKWriteText передана неподходящая задержка."); }
            return ps;
        }
    }
}
