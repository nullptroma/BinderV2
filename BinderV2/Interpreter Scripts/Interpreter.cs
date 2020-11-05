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

namespace InterpreterScripts
{
    public static class Interpreter
    {
        private static Function[] MainLibrary = FuncsLibManager.GetLibrary();
        private static HashSet<Function> AdditionalLibrary = new HashSet<Function>();

        public static void ExecuteScript(string script)
        {
            ExecuteScript(script, new InterpretationData());
        }
        public static void ExecuteScript(string script, Function[] additionalFunctions)
        {
            ExecuteScript(script, new InterpretationData() { AdditionalFunctions = new HashSet<Function>(additionalFunctions) });
        }
        public static void ExecuteScript(string script, InterpretationData data)
        {
            try
            {
                string[] commands = ScriptTools.GetCommands(script);
                foreach (var cmd in commands)
                    ExecuteCommand(cmd, data);
            }
            catch (Exception e) when (!(e is ReturnException) && !(e is BreakException))
            {
                MessageBox.Show(e.ToString(), "Ошибка");
            }
        }

        public static object ExecuteCommand(string cmdString)
        {
            return ExecuteCommand(cmdString, new InterpretationData()); 
        }


        public static object ExecuteCommand(string cmdString, InterpretationData data)
        {
            CommandModel cmd = new CommandModel(cmdString.Trim());
            Task<object> commandTask = null;

            if (Converter.CanConvertToSimpleType(cmd.Command))//берём простой тип
            {
                commandTask = Converter.ToSimpleType(cmd.Command);
            }
            else if(SyntacticConstructionsManager.IsValidConstruction(cmd, data))//ищем конструкции
            {
                commandTask = SyntacticConstructionsManager.ExecuteConstruction(cmd, data);
            }
            else if (data.CustomFunctions.Find(cFunc => cFunc.Name == cmd.KeyWord) != null)//ищем пользовательскую функцию
            {
                commandTask = data.CustomFunctions.Find(cFunc => cFunc.Name == cmd.KeyWord).GetResult(cmd.GetParameters(), data);//TODO
            }
            else if (data.AdditionalFunctions.Find(Func => Func.Name == cmd.KeyWord) != null)//ищем добавочную(для текущей интерпретации) функцию
            {
                object[] parameters = GetParametersFromStringArray(cmd.GetParameters(), data);
                commandTask = data.AdditionalFunctions.Find(Func => Func.Name == cmd.KeyWord).GetResult(parameters);
            }
            else if (AdditionalLibrary.Find(Func => Func.Name == cmd.KeyWord) != null)//ищем добавочную функцию
            {
                object[] parameters = GetParametersFromStringArray(cmd.GetParameters(), data);
                commandTask = AdditionalLibrary.Find(Func => Func.Name == cmd.KeyWord).GetResult(parameters);
            }
            else if (MainLibrary.Find(Func => Func.Name == cmd.KeyWord) != null)//ищем встроенную функцию
            {
                object[] parameters =  GetParametersFromStringArray(cmd.GetParameters(), data);
                commandTask = MainLibrary.Find(Func => Func.Name == cmd.KeyWord).GetResult(parameters);
            }
            else//если ничего не удалось, возвращаем входную команду, как строку
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

        private static object[] GetParametersFromStringArray(string[] parametersString, InterpretationData data)
        {
            object[] parameters = new object[parametersString.Length];
            for (int i = 0; i < parameters.Length; i++)
                parameters[i] = ExecuteCommand(parametersString[i], data);
            return parameters;
        }

        public static void AddToLibrary(Function f)
        {
            AdditionalLibrary.Add(f);
        }

        public static bool RemoveFromLibrary(Function f)
        {
            return AdditionalLibrary.Remove(f);
        }

        public static Function[] GetFullLibrary()
        {
            return Enumerable.Concat(MainLibrary, AdditionalLibrary).ToArray();
        }
    }
}
