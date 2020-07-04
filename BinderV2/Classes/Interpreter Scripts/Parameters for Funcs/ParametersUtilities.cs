using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinderV2.Interpreter.ParametersForFuncs
{
    public static class ParametersUtilities
    {
        public static object[] GetParameters(object[] sourceParams)//извлекает все параметры из массивов
        {
            object[] answer = new object[0];
            GetParametersFromArrays(ref answer, sourceParams);
            return answer;
        }


        private static void GetParametersFromArrays(ref object[] outputParameters, object[] parameters)
        {
            foreach (object parameter in parameters)
            {

                if (parameter is object[])
                {
                    GetParametersFromArrays(ref outputParameters, (object[])parameter);
                }
                else
                {
                    AddToArray(ref outputParameters, parameter);
                }
            }
        }

        private static void AddToArray<T>(ref T[] array, T value)
        {
            int indexToAdd = array.Length;
            Array.Resize(ref array, array.Length + 1);
            array[indexToAdd] = value;
        }
    }
}
