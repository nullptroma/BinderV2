using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class ICollectionExtensionMethods
    {
        public static T Find<T>(this IEnumerable<T> list, Func<T, bool> predication)
        {
            foreach (T item in list)
                if (predication(item))
                    return item;
            return default(T);
        }
    }
}
