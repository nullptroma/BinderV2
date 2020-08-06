using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows;
using InterpreterScripts.Funcslibrary.Exceptions;
using InterpreterScripts.Script;
using System.CodeDom;

namespace InterpreterScripts.Funcslibrary
{
    public class Function
    {
        public string Name { get { return meth.Name; } }
        public string Description { get { return meth.GetCustomAttribute<FuncDescriptionAttribute>().Description; } }
        public string GroupName { get; private set; }
        public FuncType ReturnType { get; private set; }
        private MethodInfo meth;

        public Function(MethodInfo mi, FuncType returnType)
        {
            if (mi == null)
                throw new ArgumentNullException();
            if (!mi.IsStatic)
                throw new MethodIsNotStaticException();
            if (mi.GetCustomAttribute<FuncDescriptionAttribute>() == null)//проверка на наличие атрибута описания
                throw new NotFoundDescriptionAttributeException();

            meth = mi;
            ReturnType = returnType;

            if (mi.GetCustomAttribute<FuncGroupAttribute>() != null)//проверка на наличие атрибута группы
                GroupName = mi.GetCustomAttribute<FuncGroupAttribute>().GroupName;
            else
                GroupName = "None";
        }

        public object GetResult(params object[] parameters)
        {
            return meth.Invoke(null, ScriptTools.GetParametersFromArray(parameters));
        }
    }
}
