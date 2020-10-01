using InterpreterScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinderV2.Classes
{
    static class Initializer
    {
        public static void Initialize()//Инициализация необходимых компонентов программы
        {
            DependencyResolver.Resolver.RegisterDependencyResolver();//прежде всего подключаем все зависимости
            Interpreter.ExecuteCommand("Delay(1)");//Прогоняем команду в интерпретаторе
            Settings.ProgramSettings.RuntimeSettings.ToString();//обращаемся к настройкам, чтобы они подгрузились
        }
    }
}
