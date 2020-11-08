using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Media;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using InterpreterScripts.InterpretationFunctions.Standart;
using System.ComponentModel;

namespace InterpreterScripts.InterpretationFunctions.Standart.Library
{
    static public class IntScripts
    {
        [Description("ConvertFromStringToInt(string str) - пытается конвернитровать string в int.")]
        public static object ConvertFromStringToInt(params object[] ps)
        {
            try
            {
                return int.Parse(ps[0].ToString());
            }
            catch (IndexOutOfRangeException) { MessageBox.Show("В ConvertFromStringToInt не передан подходящий агрумент"); return 0; }
            catch (FormatException) { MessageBox.Show("В ConvertFromStringToInt передан неподходящий аргумент"); return 0; }
        }

        [Description("Compare(object l, object r) - производит компарирование со значениями l и r.")]
        public static object Compare(params object[] ps)
        {
            if (ps.Length < 2)
            {
                MessageBox.Show("В Compare передано недостаточно аргументов");
                throw new Exception("В Compare передано недостаточно аргументов");
            }
            return Meths.CompareUniversal(ps[0], ps[1]);
        }

        [Description("GetProcessID(string name) - возвращает ID процесса с именем name. Если процесс не найден вернёт -1.")]
        public static object GetProcessID(params object[] ps)
        {
            if (ps.Length == 0)
            {
                MessageBox.Show("В GetProcessID передано недостаточно аргументов");
                throw new Exception("В GetProcessID передано недостаточно аргументов");
            }
            return Process.GetProcessesByName(ps[0].ToString()).Length==0 ? -1 : Process.GetProcessesByName(ps[0].ToString()).First().Id;
        }
        
        [Description("GetProcessIDForegroundWindow() - возвращает ID процесса активного окна.")]
        public static object GetProcessIDForegroundWindow(params object[] ps)
        {
            int id = -1;
            Meths.GetWindowThreadProcessId(Meths.GetForegroundWindow(), ref id);
            return id;
        }
        
        [Description("GetForegroundWindow() - возвращает дескриптор активного окна.")]
        public static object GetForegroundWindow(params object[] ps)
        {
            return Meths.GetForegroundWindow();
        }

        [FuncGroup("Cursor")]
        [Description("GetCursorPosX() - возвращает позицию курсора по координате X.")]
        public static object GetCursorPosX(params object[] ps)
        {
            return Cursor.Position.X;
        }

        [FuncGroup("Cursor")]
        [Description("GetCursorPosY() - возвращает позицию курсора по координате Y.")]
        public static object GetCursorPosY(params object[] ps)
        {
            return Cursor.Position.Y;
        }
    }
}
