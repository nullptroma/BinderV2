using InterpreterScripts.DataVault;
using InterpreterScripts.InterpretationScriptData;
using InterpreterScripts.Script;
using InterpreterScripts.ScriptCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterpreterScripts.SyntacticConstructions.Constructions
{
    class GetVarNamespaced : ISyntacticConstruction
    {
        public Task<object> Execute(CommandModel cmd, InterpretationData data)
        {
            return Task.Run(new Func<object>(() =>
            {
                string script = cmd.Command;
                int dotIndex = ScriptTools.GetCharIndexOutsideBrackets(script, '.');
                string varNamespace = script.Substring(0, dotIndex).Trim();
                string varName = script.Substring(dotIndex + 1, (script.Length - 1) - dotIndex).Trim();
                return Vault.namespacedVars.ContainsKey(varNamespace) ? Vault.namespacedVars[varNamespace][varName] : null;
            }));
        }

        public bool IsValidConstruction(CommandModel cmd, InterpretationData data)
        {
            string script = cmd.Command;
            int dotIndex = ScriptTools.GetCharIndexOutsideBrackets(script, '.');
            if (dotIndex == -1)
                return false;
            string varNamespace = script.Substring(0, dotIndex).Trim();
            string varName = script.Substring(dotIndex + 1, (script.Length - 1) - dotIndex).Trim();
            return Vault.namespacedVars.ContainsKey(varNamespace) && Vault.namespacedVars[varNamespace].HasVar(varName);
        }
    }
}
