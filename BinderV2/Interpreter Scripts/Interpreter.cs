using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.InterpretationFunctions.Standart;
using InterpreterScripts.ScriptCommand;
using InterpreterScripts.Script;
using InterpreterScripts.SyntacticConstructions;
using InterpreterScripts.Exceptions;
using System.ComponentModel;
using InterpreterScripts.TypeConverter;
using System.Runtime.CompilerServices;
using Utilities;
using System.Diagnostics;
using InterpreterScripts.InterpretationFunctions;
using System.Linq;
using InterpreterScripts.SyntacticConstructions.Constructions.Exceptions;


namespace InterpreterScripts
{
    public static class Interpreter
    {
        private static IInterpreterFunction[] MainLibrary = FuncsLibManager.GetLibrary();
        private static HashSet<IInterpreterFunction> AdditionalLibrary = new HashSet<IInterpreterFunction>();
        private static Action StopInterpretations;

        public static void ExecuteScript(string script)
        {
            ExecuteScript(script, new InterpretationData());
        }
        public static void ExecuteScript(string script, IInterpreterFunction[] additionalFunctions)
        {
            var data = new InterpretationData();
            data.InterpretationFuncs.AddRange(additionalFunctions);
            ExecuteScript(script, data);
        }
        public static void ExecuteScript(string script, InterpretationData data)
        {
            try
            {
                StopInterpretations += data.Stop;
                string[] commands = ScriptTools.GetCommands(script);
                foreach (var cmd in commands)
                    ExecuteCommand(cmd, data);
            }
            catch (Exception e) when (!(e is ReturnException) && !(e is BreakException))
            {
                if(!data.IsStopped)
                    MessageBox.Show(e.ToString(), "Ошибка");
                StopInterpretations -= data.Stop;
            }
        }

        public static object ExecuteCommand(string cmdString)
        {
            return ExecuteCommand(cmdString, new InterpretationData()); 
        }


        public static object ExecuteCommand(string cmdString, InterpretationData data)
        {
            if (data.IsStopped)
                throw new StopException();

            CommandModel cmd = new CommandModel(cmdString.Trim());
            Task<object> commandTask = null;

            if (Converter.CanConvertToSimpleType(cmd.Command))//берём простой тип
                commandTask = Converter.ToSimpleType(cmd.Command);
            if (commandTask == null)
                SyntacticConstructionsManager.TryExecute(cmd, data, out commandTask);
            if(commandTask == null)
            {
                //ищем функцию по имени во всей библиотеке, начинаем с функций этой интерпретации
                IInterpreterFunction func = data.InterpretationFuncs.FirstOrDefault(Func => Func.Name == cmd.KeyWord);
                if (func == null)
                    func = AdditionalLibrary.FirstOrDefault(Func => Func.Name == cmd.KeyWord);
                if (func == null)
                    func = MainLibrary.FirstOrDefault(Func => Func.Name == cmd.KeyWord);
                if (func != null)
                    commandTask = func.GetResult(cmd.GetParameters(), data);
            }


            if (commandTask == null)//если ничего не удалось, возвращаем входную команду, как строку
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


        static Interpreter()
        {
            AddFuncToLib();
        }

        #region lib
        static void AddFuncToLib()
        {
            AddToLibrary(new Function(new Func<object[], object>(StopAllScripts), FuncType.Other));
            AddToLibrary(new GetInterpretationInfo("GetInterpretationInfo"));
            AddToLibrary(new UserFunc("MyUserFunc", new string[] { "text"}, "MsgBox(text);"));
        }

        public static void AddToLibrary(IInterpreterFunction f)
        {
            //если функция с таким именем есть - заменяем
            var oldFunc = AdditionalLibrary.Find(curF=>curF.Name == f.Name);
            if (oldFunc != null)
                AdditionalLibrary.Remove(oldFunc);

            AdditionalLibrary.Add(f);
        }

        public static bool RemoveFromLibrary(IInterpreterFunction f)
        {
            bool result = AdditionalLibrary.Remove(f);
            return result;
        }

        [Description("StopAllScripts() - немедленно останавливает выполнение всех скриптов.")]
        [FuncGroup("ScriptRuntimeControl")]
        public static object StopAllScripts(params object[] ps)
        {
            StopInterpretations?.Invoke();
            return ps;
        }
        #endregion

        public static IInterpreterFunction[] GetFullLibrary()
        {
            return MainLibrary.Concat(AdditionalLibrary).ToArray();
        }
    }
}
