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

namespace InterpreterScripts.InterpretationScriptData.StandartFunctions.Library
{
    static public class BoolScripts
    {
        [Description("HideDesktopBackground() - скрывает фон рабочего стола. Возвращает результат операции.")]
        public static object HideDesktopBackground(params object[] ps)
        {
            bool show = true;
            IntPtr hWin = Meths.FindWindow("Progman", null);
            if (hWin != IntPtr.Zero)
                return Meths.ShowWindow(hWin, show ? 0 : 5);

            return false;
        }

        [Description("ShowDesktopBackground() - показывает фон рабочего стола. Возвращает результат операции.")]
        public static object ShowDesktopBackground(params object[] ps)
        {
            bool show = false;
            IntPtr hWin = Meths.FindWindow("Progman", null);
            if (hWin != IntPtr.Zero)
                return Meths.ShowWindow(hWin, show ? 0 : 5);

            return false;
        }

        
    }
}
