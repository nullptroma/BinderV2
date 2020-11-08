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
using InterpreterScripts.InterpretationFunctions.Standart;
using System.ComponentModel;

namespace InterpreterScripts.InterpretationFunctions.Standart.Library
{
    static public class DoubleScripts
    {
        [Description("ConvertStringToDouble(string str) - пытается конвернитровать string в double.")]
        public static object ConvertStringToDouble(params object[] ps)
        {
            try
            {
                return double.Parse(ps[0].ToString().Replace(".", ","));
            }
            catch (IndexOutOfRangeException) { MessageBox.Show("В ConvertStringToDouble не переданы агрументы"); return 0; }
            catch (FormatException) { MessageBox.Show("В ConvertStringToDouble передан неподходящий аргумент"); return 0; }
        }
    }
}
