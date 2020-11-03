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
        public string Description { get { return "Namespace.Name = value - позволяет задать значение переменной Name в пространстве имён Namespace."; } }

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
            int index = ScriptTools.GetCharIndexOutsideBrackets(cmd.Command, '=');
            try 
            {
                if (cmd.Command[index - 1] == '=' || cmd.Command[index + 1] == '=')
                    return false;
            }
            catch { return false; }
            return index != -1 && ScriptTools.GetCharIndexOutsideBrackets(cmd.Command, '.') != -1;
        }
    }
}
