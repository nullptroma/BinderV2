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
using InterpreterScripts.InterpretationFunctions.Standart;
using System.ComponentModel;
using Hooks.Keyboard;

namespace InterpreterScripts.InterpretationFunctions.Standart.Library
{
    static public class BoolScripts
    {
        [FuncGroup("Desktop")]
        [Description("HideDesktopBackground() - скрывает фон рабочего стола. Возвращает результат операции.")]
        public static object HideDesktopBackground(params object[] ps)
        {
            bool show = true;
            IntPtr hWin = Meths.FindWindow("Progman", null);
            if (hWin != IntPtr.Zero)
                return Meths.ShowWindow(hWin, show ? 0 : 5);

            return false;
        }

        [FuncGroup("Desktop")]
        [Description("ShowDesktopBackground() - показывает фон рабочего стола. Возвращает результат операции.")]
        public static object ShowDesktopBackground(params object[] ps)
        {
            bool show = false;
            IntPtr hWin = Meths.FindWindow("Progman", null);
            if (hWin != IntPtr.Zero)
                return Meths.ShowWindow(hWin, show ? 0 : 5);

            return false;
        }
        
        [FuncGroup("Desktop")]
        [Description("DesktopIsActive() - фокус на рабочем столе (true or false).")]
        public static object DesktopIsActive(params object[] ps)
        {
            const int maxChars = 256;
            IntPtr handle = IntPtr.Zero;
            StringBuilder className = new StringBuilder(maxChars);

            handle = Meths.GetForegroundWindow();

            if (Meths.GetClassName(handle, className, maxChars) > 0)
            {
                string cName = className.ToString();
                if (cName == "Progman" || cName == "WorkerW")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        
        [Description("IsBool(var parameter) - является parameter типом Boolean.")]
        public static object IsBool(params object[] ps)
        {
            if (ps.Length != 1)
                return false;

            return ps[0] is bool;
        }

        [Description("ButtonIsDown(string key) - нажата ли указанная клавиша на клавиатуре.")]
        public static object ButtonIsDown(params object[] ps)
        {
            return Hooks.Keyboard.KeysEngine.PressedKeys.Any(k=>k.ToString().ToLower() == ps[0].ToString().ToLower());
        }
    }
}
