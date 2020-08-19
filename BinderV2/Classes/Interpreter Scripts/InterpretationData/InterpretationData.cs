using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterpreterScripts.InterpretationScriptData.StandartFunctions;
using InterpreterScripts.InterpretationScriptData.CustomFunctions;
using InterpreterScripts.InterpretationScriptData.Vars;

namespace InterpreterScripts.InterpretationScriptData
{
    public class InterpretationData
    {
        private HashSet<Function> additionalFunctions = new HashSet<Function>();
        public HashSet<Function> AdditionalFunctions
        {
            get { return additionalFunctions; }
            set 
            { 
                if (value != null)
                    additionalFunctions = value;
                else
                    throw new NullReferenceException();
            }
        }

        private HashSet<UserFunc> customFunctions = new HashSet<UserFunc>();
        public HashSet<UserFunc> CustomFunctions
        {
            get { return customFunctions; }
            set
            {
                if (value != null)
                    customFunctions = value;
                else
                    throw new NullReferenceException();
            }
        }

        public Variables Vars { get; private set; }

        public InterpretationData()
        {
            Vars = new Variables();
        }
    }
}
