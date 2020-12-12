using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.ScriptCommand;
using InterpreterScripts.DataVault;
using InterpreterScripts.SyntacticConstructions;
using System.Windows;

namespace InterpreterScripts.SyntacticConstructions.Constructions
{
    public class WithOtherData : ISyntacticConstruction
    {
        public string Name { get { return "WhithOtherData"; } }
        public string Description { get { return "WithOtherData - выполняет команду с другими данными интерпретации name:команда, где name-имя новых/существующих данных."; } }

        public Task<object> TryExecute(CommandModel cmd, InterpretationData data)
        {
            var colonIndex = Script.ScriptTools.GetCharIndexOutsideBrackets(cmd.Command, ':');//глобально
            if (colonIndex != -1)
            {
                string newDataName = cmd.Command.Substring(0,colonIndex);
                if (!IsValidName(newDataName))
                    return null;

                if (!Vault.InterpretationDatas.ContainsKey(newDataName))
                    Vault.InterpretationDatas.Add(newDataName, new InterpretationData());
                return Task.Run(()=> Interpreter.ExecuteCommand(cmd.Command.Substring(colonIndex + 1), Vault.InterpretationDatas[newDataName]));
            }

            colonIndex = Script.ScriptTools.GetCharIndexOutsideBrackets(cmd.Command, '.');//локально
            if (colonIndex != -1)
            {
                string newDataName = cmd.Command.Substring(0, colonIndex);
                if (!IsValidName(newDataName))
                    return null;
                InterpretationData dataToExecute = null;
                if (!data.Vars.HasVar(newDataName) || !(data.Vars[newDataName] is InterpretationData))
                {
                    var newData = Interpreter.ExecuteCommand(newDataName, data);
                    if (newData is InterpretationData newDataCasted)
                        dataToExecute = newDataCasted;
                    else
                    {
                        data.Vars[newDataName] = new InterpretationData();
                        dataToExecute = (InterpretationData)data.Vars[newDataName];
                    }
                }
                else
                    dataToExecute = (InterpretationData)data.Vars[newDataName];
                return Task.Run(() => Interpreter.ExecuteCommand(cmd.Command.Substring(colonIndex + 1), dataToExecute));
            }
            return null;
        }

        private bool IsValidName(string name)
        {
            char[] symbols = new char[] { '\"', '\\', ' ', };
            return char.IsLetter(name[0]) && name.AsParallel().All(ch => !symbols.Contains(ch));
        }
    }
}
