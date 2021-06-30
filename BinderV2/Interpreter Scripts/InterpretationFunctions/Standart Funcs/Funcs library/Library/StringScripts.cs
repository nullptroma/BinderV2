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
using Microsoft.SqlServer.Server;
using InterpreterScripts.InterpretationFunctions.Standart;
using System.ComponentModel;
using Utilities;

namespace InterpreterScripts.InterpretationFunctions.Standart.Library
{
    static public class StringScripts
    {
        [Description("ToString(object o1, object o2...) - возвращает строковое представление объекта(ов)(можно использовать для объединения строк).")]
        public static object ToString(params object[] ps)
        {
            if(ps.Length!=0)
                return string.Join("", ps);
            return "";
        }


        [Description("GetDesktopPath() - возвращает путь до рабочего стола.")]
        public static object GetDesktopPath(params object[] ps)
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        [Description("ReplaceString(string str, string subStr, string replace) - возвращает строку с заменёнными subStr на replace.")]
        public static object ReplaceString(params object[] ps)
        {
            try
            {
                return ps[0].ToString().Replace(ps[1].ToString(), ps[2].ToString());
            }
            catch { MessageBox.Show("В ReplaceString недостаточно аргументов"); return ""; }
        }


        [Description("GetUserName() - возвращает имя текущего пользователя.")]
        public static object GetUserName(params object[] ps)
        {
            return Environment.UserName;
        }

        [FuncGroup("Keys")]
        [Description("GetKeysHelp() - возвращает помощь по кнопкам.")]
        public static object GetKeysHelp(params object[] ps)
        {
            string ans = "";
            foreach (var k in Enum.GetValues(typeof(Keys)))
            {
                ans += k.ToString() + ";     ";
            }
            return ans;
        }

        [FuncGroup("Keys")]
        [Description("GetSendKeysHelp() - возвращает помощь для скрипта SendKeysWait.")]
        public static object GetSendKeysHelp(params object[] ps)
        {
            return "SendKeysWait() посылает активному окну последовательность клавиш. Чтобы отправить обычный текст просто запишите его так: SendKeysWait(\"Hello\"). Чтобы имитировать нажатие например CTRL+C: SendKeysWait(^{C}).";
        }

        [FuncGroup("Mouse")]
        [Description("GetMouseEventsHelp() - возвращает помощь для скрипта MouseEvent.")]
        public static object GetMouseEventsHelp(params object[] ps)
        {
            string ans = "";
            foreach (var k in Enum.GetValues(typeof(Meths.MouseEventFlags)))
            {
                ans += k.ToString() + ";     ";
            }
            return ans;
        }

        [FuncGroup("Clipboard")]
        [Description("GetClipboardText() - возвращает текст из буфера обмена Windows.")]
        public static object GetClipboardText(params object[] ps)
        {
            string data = "";
            Thread thread;
            thread = new Thread(() => data = Clipboard.GetText());
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            return data;
        }
        
        
        [FuncGroup("Keys")]
        [Description("VKCodeToUnicode(uint vcKey) - представляет кнопку vkCode в виде строки с учётом языка.")]
        public static object VKCodeToUnicode(params object[] ps)
        {
            return KeyCodeToUnicode.VKCodeToUnicode((uint)(Keys)Enum.Parse(typeof(Keys), ps[0].ToString()));
        }


        [FuncGroup("Files")]
        [Description("GetTextFromFile(string path) - возвращает текст из указанного файла.")]
        public static object GetTextFromFile(params object[] ps)
        {
            return File.ReadAllText(ps[0].ToString());
        }

        [Description("StrIndex(string str, int index) - возвращает символ по указанному индексу в строке.")]
        public static object StrIndex(params object[] ps)
        {
            return ps[0].ToString()[(int)ps[1]];
        }

        [Description("LayoutSimbols(string text) - возвращает текст с изменённой раскладкой русский-английский по QWERTY.")]
        public static object LayoutSimbols(params object[] ps)
        {
            if (ps.Length == 0)
            {
                return "";
            }
            string str = ps[0].ToString();
            for (int i = 0; i < str.Length; i++)
            {
                foreach ((char, char) chrs in Constants.LayoutSimbols)
                {

                    if (str[i] == chrs.Item1)
                    {
                        str = str.Remove(i, 1).Insert(i, chrs.Item2.ToString());
                        break;
                    }
                    else if (str[i] == chrs.Item2)
                    {
                        str = str.Remove(i, 1).Insert(i, chrs.Item1.ToString());
                        break;
                    }

                }
            }

            return str;
        }
    }
}
