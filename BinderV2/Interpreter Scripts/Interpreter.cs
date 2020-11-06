using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.InterpretationScriptData.StandartFunctions;
using InterpreterScripts.ScriptCommand;
using InterpreterScripts.Script;
using InterpreterScripts.SyntacticConstructions;
using InterpreterScripts.Exceptions;
using System.ComponentModel;
using InterpreterScripts.TypeConverter;
using System.Runtime.CompilerServices;
using Utilities;
using InterpreterScripts.InterpretationScriptData.CustomFunctions;
using System.Diagnostics;
using System.Linq;
using InterpreterScripts.SyntacticConstructions.Constructions.Exceptions;
using InterpreterScripts.FuncAttributes;

namespace InterpreterScripts
{
    public static class Interpreter
    {
        private static Function[] MainLibrary = FuncsLibManager.GetLibrary();
        private static HashSet<Function> AdditionalLibrary = new HashSet<Function>();

        private static IEnumerable<Function> FullLibrary;
        private static bool StopExecuting = false;
        private static int ExecutingCount = 0;

        private static void UpdateFullLibrary()
        {
            FullLibrary = MainLibrary.Concat(AdditionalLibrary);
        }

        public static void ExecuteScript(string script)
        {
            ExecuteScript(script, new InterpretationData());
        }
        public static void ExecuteScript(string script, Function[] additionalFunctions)
        {
            var data = new InterpretationData();
            data.InterpretationFuncs.AddRange(additionalFunctions);
            ExecuteScript(script, data);
        }
        public static void ExecuteScript(string script, InterpretationData data)
        {
            try
            {
                if (ExecutingCount == 0)
                    StopExecuting = false;
                ExecutingCount++;
                string[] commands = ScriptTools.GetCommands(script);
                foreach (var cmd in commands)
                    ExecuteCommand(cmd, data);
            }
            catch (Exception e) when (!(e is ReturnException) && !(e is BreakException))
            {
                if(!StopExecuting)
                    MessageBox.Show(e.ToString(), "Ошибка");
            }
            finally
            {
                ExecutingCount--;
                if (ExecutingCount == 0)
                    StopExecuting = false;
            }
        }

        public static object ExecuteCommand(string cmdString)
        {
            return ExecuteCommand(cmdString, new InterpretationData()); 
        }


        public static object ExecuteCommand(string cmdString, InterpretationData data)
        {
            if (StopExecuting)
                throw new StopException();

            CommandModel cmd = new CommandModel(cmdString.Trim());
            Task<object> commandTask = null;

            if (Converter.CanConvertToSimpleType(cmd.Command))//берём простой тип
                commandTask = Converter.ToSimpleType(cmd.Command);
            else if (SyntacticConstructionsManager.IsValidConstruction(cmd, data))//ищем конструкции
                commandTask = SyntacticConstructionsManager.ExecuteConstruction(cmd, data);
            else 
            {
                //ищем функцию по имени во всей библиотеке, начинаем с функций этой интерпретации
                IInterpreterFunction func = data.InterpretationFuncs.FirstOrDefault(Func => Func.Name == cmd.KeyWord);
                if (func == null)
                    func = FullLibrary.FirstOrDefault(Func => Func.Name == cmd.KeyWord);
                if (func != null)
                    commandTask = func.GetResult(cmd.GetParameters(), data);
            }
            

            if(commandTask == null)//если ничего не удалось, возвращаем входную команду, как строку
                commandTask = Task.FromResult<object>(cmdString);

            if (commandTask.Status == TaskStatus.Created)//если задача создана, но не запущена
                commandTask.Start();
            if (cmd.IsAsync)//если асинхронно, на всё пофиг
                return null;
            else
            {
                try { return commandTask.Result; }//если что то не так, прокидываем исключение выше
                catch { throw commandTask.Exception.InnerException; };
            }
        }

        public static void AddToLibrary(Function f)
        {
            AdditionalLibrary.Add(f);
            UpdateFullLibrary();
        }

        public static bool RemoveFromLibrary(Function f)
        {
            bool result = AdditionalLibrary.Remove(f);
            UpdateFullLibrary();
            return result;
        }

        static Interpreter()
        {
            AddToLibrary(new Function(new Func<object[], object>(StopAllScripts), FuncType.Other));
            UpdateFullLibrary();
        }

        [Description("StopAllScripts() - немедленно останавливает выполнение всех скриптов.")]
        public static object StopAllScripts(params object[] ps)
        {
            StopExecuting = true;
            return ps;
        }

        public static Function[] GetFullLibrary()
        {
            return Enumerable.Concat(MainLibrary, AdditionalLibrary).ToArray();
        }
    }
}
