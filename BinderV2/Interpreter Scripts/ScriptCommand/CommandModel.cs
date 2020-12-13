using InterpreterScripts.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InterpreterScripts.ScriptCommand
{
    public class CommandModel
    {
        public string KeyWord { get; private set; }
        public bool IsAsync { get; private set; }
        public string Command { get; private set; }
        private string[] parameters;

        public CommandModel(string cmd)
        {
            cmd = SyncTrimBrackets(cmd);
            if (cmd.StartsWith("async"))
            {
                IsAsync = true;
                cmd = cmd.Remove(0, "async".Length).Trim();
            }
            Command = cmd;//берём команду, когда в ней нет async
            if (!ParseFuncData(cmd))
            {
                int spaceIndex = Command.IndexOf(' ');
                KeyWord = Command.Substring(0, spaceIndex != -1 ? spaceIndex : Command.Length).Trim();
                parameters = new string[0];
            }
        }

        private string SyncTrimBrackets(string str)
        {
            if (str.Length < 2 || str[0] != '(' || str[str.Length - 1] != ')')
                return str;

            int countBrackets = 0;//текущий уровень скобки
            for (int i = 1; i < str.Length - 1; i++)
            {
                if (str[i] == '(')
                    countBrackets++;
                else if (str[i] == ')')
                    countBrackets--;

                if (countBrackets == -1)
                    return str;
            }
            if (countBrackets == 0)
            {
                string answer = str.Remove(str.Length - 1, 1).Remove(0, 1);
                string nextAnswer = SyncTrimBrackets(answer);
                return answer == nextAnswer ? answer : nextAnswer;
            }
            return str;
        }

        private bool ParseFuncData(string cmd)
        {
            int bracketIndex = cmd.IndexOf('(');
            if (bracketIndex == -1)
                return false;
            KeyWord = cmd.Substring(0, bracketIndex).Trim();//берём ключевое слово: всё до параметров
            if (KeyWord.Contains(' '))//если ключевое слово состоит более чем из 1 слова
                return false;
            int startParsIndex = bracketIndex + 1;//Начинаем с символа после первой скобки
            int lastParsIndex = startParsIndex;
            int bracketsCount = 1;
            int marksCount = 0;
            for (; lastParsIndex < cmd.Length; lastParsIndex++)
            {
                if (cmd[lastParsIndex] == '\"')
                    marksCount++;
                if (marksCount % 2 == 0)
                {
                    if (cmd[lastParsIndex] == '(')
                        bracketsCount++;
                    if (cmd[lastParsIndex] == ')')
                        bracketsCount--;
                }
                if (bracketsCount == 0)
                    break;
            }
            var stringPars = cmd.Substring(startParsIndex, lastParsIndex - startParsIndex);
            parameters = ScriptTools.GetParameters(stringPars);
            return true;
        }

        public string[] GetParameters()
        {
            return parameters;        
        }
    }
}
