using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InterpreterScripts.InterpretationScriptData.Vars
{
    public class Variables
    {
        private readonly Dictionary<string, object> vars = new Dictionary<string, object>();

        public object this[string name]
        {
            get { return vars.ContainsKey(name) ? vars[name] : null; }
            set 
            {
                if (!vars.ContainsKey(name))
                    vars.Add(name, value);
                else
                    vars[name] = value;
            }
        }

        public bool HasVar(string name)
        {
            return vars.ContainsKey(name);
        }

        public string[] GetAllVarsNames()
        {
            return vars.Keys.ToArray();
        }

        public bool RemoveVar(string name)
        {
            return vars.Remove(name);
        }
    }
}
