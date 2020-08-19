using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace InterpreterScripts.TypeConverter
{
    public static class Converter
    {
        private static Func<string, object>[] converters = new Func<string, object>[] { StringConverter, IntConverter, DoubleConverter, BoolConverter };
        public static Task<object> ToSimpleType(string strValue)
        {
            return Task.Run(()=> 
            {
                foreach (Func<string, object> converter in converters)
                {
                    object result = converter(strValue);
                    if (result != null)
                        return result;
                }
                return null;
            });
        }

        public static bool CanConvertToSimpleType(string strValue)
        {
            return ToSimpleType(strValue).Result != null;
        }


        private static object StringConverter(string value)
        {
            if (value.Length < 2)
                return null;
            if (value[0] == '\"' && value[value.Length-1] == '\"')
                return value.Substring(1, Math.Max(1, value.Length - 2));
            return null;
        }

        private static object DoubleConverter(string value)
        {
            if (double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out double result)) return result;
            return null;
        }

        private static object IntConverter(string value)
        {
            if (int.TryParse(value, out int result)) return result;
            return null;
        }

        private static object BoolConverter(string value)
        {
            if (bool.TryParse(value, out bool result)) return result;
            return null;
        }
    }
}
