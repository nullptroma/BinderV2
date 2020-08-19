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
            if (cmd.StartsWith("async "))
            {
                IsAsync = true;
                cmd = cmd.Remove(0, "async".Length).Trim();
            }
            Command = cmd;//берём команду, когда в ней нет async
            try { ParseFuncData(cmd); }//пытаемся получить данные для функции
            catch //в случае неудачи пихаем заглушки
            {
                int spaceIndex = Command.IndexOf(' ');
                KeyWord = Command.Substring(0, spaceIndex!=-1 ? spaceIndex : Command.Length).Trim();
                parameters = new string[0];
            }
        }

        private void ParseFuncData(string cmd)
        {
            KeyWord = cmd.Substring(0, cmd.IndexOf('('));//берём ключевое слово: всё до параметров
            if (KeyWord.Contains(' '))//если ключевое слово состоит более чем из 1 слова
                throw new FormatException();
            int startParsIndex = cmd.IndexOf('(') + 1;//Начинаем с символа после первой скобки
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
            parameters = ScriptTools.GetParameters(cmd.Substring(startParsIndex, lastParsIndex - startParsIndex));
        }

        public string[] GetParameters()
        {
            return (string[])parameters.Clone();        
        }
    }
}
