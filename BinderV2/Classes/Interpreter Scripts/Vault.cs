using InterpreterScripts.InterpretationScriptData.Vars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterScripts.DataVault
{
    public static class Vault
    {
        public static Dictionary<string, Variables> namespacedVars = new Dictionary<string, Variables>();
    }
}
