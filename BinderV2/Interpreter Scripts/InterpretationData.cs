using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InterpreterScripts.InterpretationFunctions;
using InterpreterScripts.InterpretationScriptData.Vars;
using System.Net.Http.Headers;
using System.Windows;

namespace InterpreterScripts.InterpretationScriptData
{
    public class InterpretationData
    {
        public bool IsStopped { get; private set; }

        public void Stop()
        {
            IsStopped = true;
        }

        private List<IInterpreterFunction> interpretationFuncs = new List<IInterpreterFunction>();
        public List<IInterpreterFunction> InterpretationFuncs
        {
            get { return interpretationFuncs; }
            set
            {
                if (value != null)
                    interpretationFuncs = value;
                else
                    throw new NullReferenceException();
            }
        }

        public Variables Vars { get; private set; }

        public InterpretationData()
        {
            Vars = new Variables();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Vars:\n");
            foreach (string varName in Vars.GetAllVarsNames())
                sb.Append(varName + " = " + Vars[varName].ToString() + "\n");
            sb.Append("\n");
            sb.Append("Funcs:\n");
            foreach (var func in InterpretationFuncs)
                sb.Append(func.Name + "\n");
            return sb.ToString();
        }
    }
}
