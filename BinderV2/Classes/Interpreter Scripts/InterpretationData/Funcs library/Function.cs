using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows;
using InterpreterScripts.Exceptions;
using InterpreterScripts.Script;
using InterpreterScripts.FuncAttributes;
using System.ComponentModel;
using System.CodeDom;

namespace InterpreterScripts.InterpretationScriptData.StandartFunctions
{
    public class Function
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string GroupName { get; private set; }
        public FuncType ReturnType { get; private set; }
        private Func<object[], object> meth;

        public Function(MethodInfo mi, object target, FuncType returnType)
        {
            if (mi == null)
                throw new ArgumentNullException();
            if (!mi.IsStatic && target == null)
                throw new MethodIsNotStaticException();
            
            meth = (Func<object[], object>)mi.CreateDelegate(typeof(Func<object[], object>), target);
            Name = mi.Name;
            ReturnType = returnType;
            
            if (mi.GetCustomAttribute<DescriptionAttribute>() != null)//проверка на наличие атрибута описания
                Description = mi.GetCustomAttribute<DescriptionAttribute>().Description;

            if (mi.GetCustomAttribute<FuncGroupAttribute>() != null)//проверка на наличие атрибута группы
                GroupName = mi.GetCustomAttribute<FuncGroupAttribute>().GroupName;
            else
                GroupName = "None";
        }

        public Function(Delegate func, FuncType returnType) : this(func.Method, func.Target, returnType)
        {  }


        public Task<object> GetResult(params object[] parameters)
        {
            return Task.Run(() =>
            {
                return meth.Invoke(ScriptTools.GetParametersFromArray(parameters));
            });
        }

        public override bool Equals(object obj)
        {
            if(obj is Function)
            {
                Function f2 = (Function)obj;
                return Name == f2.Name && meth == f2.meth;
            }
            else
                return false;
        }

        public override int GetHashCode()
        {
            return meth.GetHashCode();
        }
    }
}
