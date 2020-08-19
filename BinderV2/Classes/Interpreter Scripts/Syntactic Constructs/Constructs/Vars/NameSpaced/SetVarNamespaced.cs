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
    class SetVarNamespaced : ISyntacticConstruction
    {
        public Task<object> Execute(CommandModel cmd, InterpretationData data)
        {
            return Task.Run(new Func<object>(() =>
            {
                string script = cmd.Command;
                int assignmentIndex = ScriptTools.GetCharIndexOutsideBrackets(script, '=');
                string varNamespaceAndName = script.Substring(0, assignmentIndex).Trim();
                int dotIndex = ScriptTools.GetCharIndexOutsideBrackets(varNamespaceAndName, '.');
                string varNamespace = varNamespaceAndName.Substring(0, dotIndex).Trim();
                string varName = varNamespaceAndName.Substring(dotIndex+1, (varNamespaceAndName.Length - 1) - dotIndex).Trim();
                string valueString = script.Substring(assignmentIndex + 1, (script.Length - 1) - assignmentIndex).Trim();
                object value = Interpreter.ExecuteCommand(valueString, data);
                if (Vault.namespacedVars.ContainsKey(varNamespace))
                    Vault.namespacedVars[varNamespace][varName] = value;
                else
                {
                    Vault.namespacedVars.Add(varNamespace, new InterpretationScriptData.Vars.Variables());
                    Vault.namespacedVars[varNamespace][varName] = value;
                }
                return value;
            }));
        }

        public bool IsValidConstruction(CommandModel cmd, InterpretationData data)
        {
            return ScriptTools.GetCharIndexOutsideBrackets(cmd.Command, '=') != -1 && ScriptTools.GetCharIndexOutsideBrackets(cmd.Command, '.') != -1;
        }
    }
}
