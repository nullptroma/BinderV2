using InterpreterScripts.DataVault;
using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.ScriptCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterpreterScripts.SyntacticConstructions.Constructions
{
    class CheckVarNamespaced : ISyntacticConstruction
    {
        public string Description { get { return "CheckVar(Name) - проверяет наличие переменной Name в пределах данного скрипта."; } }

        public Task<object> TryExecute(CommandModel cmd, InterpretationData data)
        {
            if (cmd.KeyWord == "CheckVar" && cmd.GetParameters().Length == 2)
                return Execute(cmd, data);
            return null;
        }

        private Task<object> Execute(CommandModel cmd, InterpretationData data)
        {
            return Task.Run(new Func<object>(() =>
            {
                var pars = cmd.GetParameters();
                var varNamespace = Interpreter.ExecuteCommand(pars[0], data).ToString();
                if (pars.Length == 2)
                    if (Vault.namespacedVars.ContainsKey(varNamespace))
                    {
                        var varName = Interpreter.ExecuteCommand(pars[1], data).ToString();
                        return Vault.namespacedVars[varNamespace].HasVar(varName);
                    }
                return false;
            }));
        }
    }
}
