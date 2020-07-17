using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows;
using BinderV2.Interpreter.DescriptionAttribute;

namespace BinderV2.Interpreter.Script.FuncClass
{
    public class ScriptFunc
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        private Func<object[], object[]> method;

        public ScriptFunc(string name, string desc, Func<object, object[]> m)
        {
            Name = name;
            Description = desc;
            method = m;
        }

        public ScriptFunc(MethodInfo mi)
        {
            Name = mi.Name;
            Description = mi.GetCustomAttribute<ScriptDescriptionAttribute>().Description;
            method = (Func<object[], object[]>)mi.CreateDelegate(typeof(Func<object[], object[]>));
        }

        public object[] GetResult(params object[] parameters)
        {
            return method.Invoke(ScriptTools.GetParametersFromArray(parameters));
        }

        
    }
}
