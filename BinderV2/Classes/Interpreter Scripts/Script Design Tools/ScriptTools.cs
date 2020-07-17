using BinderV2.Interpreter.Script.UserFunc.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Annotations;

namespace BinderV2.Interpreter.Script
{
    public static class ScriptTools
    {
        static string[] GetCommands(string sc)//получить список команд в скрипте
        {
            List<string> commands = new List<string>();//ответ
            int countBrakets = 0;//счётчик скобок
            for (int i = 0; i < sc.Length; i++)
            {
                if (sc[i] == '{')
                    countBrakets++;
                else if (sc[i] == '}')
                    countBrakets--;
                if (sc[i] == ';' && countBrakets == 0)//если мы не внутри скобки, и нашли ;, то берём команду
                {
                    commands.Add(sc.Substring(0, i).Trim());
                    sc = sc.Remove(0, i + 1);
                    i = 0;
                }
            }
            commands.AsParallel().ForAll(c => c = c.Trim());
            return commands.Where(c => c != "").ToArray();
        }

        public static string ReplaceParams(string script, string[] oldPars, string[] newPars)//заменяет параметры всех функций в скрипте 
        {
            if (oldPars.Length != newPars.Length)
                throw new WrongNumberOfParameters(newPars.Length, oldPars.Length);
            StringBuilder answer = new StringBuilder();
            int countMarks = 0;
            int countBrackets = 0;
            int lastEndIndex = 0;
            for (int i = 0; i < script.Length; i++)
            {
                if (script[i] == '\"')
                    countMarks++;
                if (script[i] == '(')
                    countBrackets++;
                if (script[i] == ')')
                    countBrackets--;

                if (countMarks % 2 != 0 || countBrackets % 2 == 0)//если мы в кавычке или не в скобке
                    continue;

                int startIndex = i + 1;
                int parsLength;
                while (script[i] != ')')
                {
                    i++;
                    if (i >= script.Length)
                        throw new IncorrectScriptDesign("Неверный формат скрипта");

                    if (script[i] == '\"')
                        countMarks++;
                    if (script[i] == '(')
                        countBrackets++;
                    if (script[i] == ')')
                        countBrackets--;
                }
                parsLength = i- startIndex;
                string[] currentParameters = GetParameters(script.Substring(startIndex, parsLength));//получаем параметры
                answer.Append(script.Substring(lastEndIndex, startIndex-lastEndIndex));
                for (int pasrIndex = 0; pasrIndex < currentParameters.Length; pasrIndex++)//меняем параметры
                {
                    int newParIndex = oldPars.ToList().FindIndex(par => par == currentParameters[pasrIndex]);
                    if (newParIndex != -1)
                        currentParameters[pasrIndex] = newPars[newParIndex];
                }
                answer.Append(string.Join(", ", currentParameters));
                lastEndIndex = startIndex + parsLength;
            }
            answer.Append(");");
            return answer.ToString();
        }

        static string[] GetParameters(string str)//получить массив параметров из строки вида "x, y, abc" без кавычек
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
                pars.Add(str);
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

                if (parameter is object[])
                {
                    GetParametersFromArrays(ref outputParameters, (object[])parameter);
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
    }
}
