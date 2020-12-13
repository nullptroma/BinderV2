using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Annotations;
using InterpreterScripts.Exceptions;
using Trigger.Events;

namespace InterpreterScripts.Script
{
    public static class ScriptTools
    {
        public static string[] GetCommands(string sc)//получить список команд в скрипте
        {
            List<string> commands = new List<string>();//ответ
            int countBrakets = 0;//счётчик скобок
            int countMarks = 0;//счётчик кавычек
            for (int i = 0; i < sc.Length; i++)
            {
                if (sc[i] == '{')
                    countBrakets++;
                else if (sc[i] == '}')
                    countBrakets--;
                else if (sc[i] == '\"')
                    countMarks++;
                if (sc[i] == ';' && countBrakets == 0 && countMarks % 2 == 0)//если мы не внутри скобки, и нашли ;, то берём команду
                {
                    commands.Add(sc.Substring(0, i).Trim());
                    sc = sc.Remove(0, i + 1);
                    i = 0;
                }
            }
            return commands.Where(c => c.Trim() != "").ToArray();
        }

        public static string[] GetParameters(string str)//получить массив параметров из строки вида "x, y, abc" без кавычек
        {
            int countBrackets = 0;//счётчик скобок
            int countMarks = 0;//счётчик кавычек
            List<string> pars = new List<string>();
            str = str.Trim(',');
            str += ',';
            for (int i = 0; i < str.Length; i++)
            {
                switch (str[i])
                {
                    case '(':
                        countBrackets++;
                        break;
                    case ')':
                        countBrackets--;
                        break;
                    case '\"':
                        if (i > 0)
                            if (str[i - 1] == '\\')
                                break;
                        countMarks++;
                        break;
                    case ',':
                        if (countBrackets == 0 && countMarks % 2 == 0)
                        {
                            pars.Add(str.Substring(0, i));
                            str = str.Remove(0, i + 1);
                            i = -1;
                            countBrackets = 0;
                            countMarks = 0;

                            str = str.Trim(' ');
                        }
                        break;
                }
            }
            if (pars.Count == 0)
                pars.Add(str.Trim(','));
            return pars.ToArray();
        }

        public static object[] GetParametersFromArray(object[] sourceParams)//извлекает все параметры из массивов
        {
            object[] answer = new object[0];
            GetParametersFromArrays(ref answer, sourceParams);
            return answer;
        }

        private static void GetParametersFromArrays(ref object[] outputParameters, object[] parameters)
        {
            foreach (object parameter in parameters)
            {
                if (parameter is object[] paramsArray)
                {
                    GetParametersFromArrays(ref outputParameters, paramsArray);
                }
                else
                {
                    AddToArray(ref outputParameters, parameter);
                }
            }
        }
        private static void AddToArray<T>(ref T[] array, T value)
        {
            int indexToAdd = array.Length;
            Array.Resize(ref array, array.Length + 1);
            array[indexToAdd] = value;
        }


        public static int GetCharIndexOutsideBrackets(string str, char ch)
        {
            int countMarks = 0;
            int countBraces = 0;
            int countBrackets = 0;
            bool findSpace = false;
            for (int assignmentIndex = 0; assignmentIndex < str.Length; assignmentIndex++)
            {
                if (str[assignmentIndex] == '\"')
                    countMarks++;
                else if (str[assignmentIndex] == '(')
                    countBrackets++;
                else if (str[assignmentIndex] == ')')
                    countBrackets--;
                else if (str[assignmentIndex] == '{')
                    countBraces++;
                else if (str[assignmentIndex] == '}')
                    countBraces--;

                if (countMarks % 2 == 0 && countBraces == 0 && countBrackets == 0)
                    if (str[assignmentIndex] == ch)
                        return assignmentIndex;

                if (str[assignmentIndex] == ' ')
                    findSpace = true;
                else if (findSpace)//если сейчас мы не на пробеле, но до этого он был
                    return -1;


            }
            return -1;
        }

        public static string FormateScript(string sc)
        {
            int count = 0;
            var strs = sc.Split('\n');
            for (int i = 0; i < strs.Length; i++)
            {
                if (strs[i].Length == 0)
                    continue;
                strs[i] = strs[i].Trim(' ');
                if (strs[i][0] == '{')
                {
                    if (count > 0)
                        strs[i] = string.Join("", Enumerable.Repeat("    ", count)) + strs[i];
                    count++;
                    continue;
                }
                else if (strs[i][0] == '}')
                {
                    count--;
                }
                if (count > 0)
                {
                    strs[i] = string.Join("", Enumerable.Repeat("    ", count)) + strs[i];
                }
            }
            return string.Join("\n", strs);
        }
    }
}
