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

                if (!data.Vars.HasVar(newDataName) || !(data.Vars[newDataName] is InterpretationData))
                    data.Vars[newDataName] = new InterpretationData();
                return Task.Run(() => Interpreter.ExecuteCommand(cmd.Command.Substring(colonIndex + 1), (InterpretationData)data.Vars[newDataName]));
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
