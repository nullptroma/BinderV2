using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinderV2.Interpreter.ScriptHelp
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
            commands = commands.Where(c => c != "").ToList();
            commands.AsParallel().ForAll(c => c = c.Trim());
            return commands.ToArray();
        }
    }
}
