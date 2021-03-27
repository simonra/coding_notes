using System.Collections.Generic;

namespace Uncategorized
{
    public static class MiscExtensions
    {
        public static string StrJoin(this IEnumerable<string> values, string separator)
        {
            return string.Join(separator, values);
        }

        public static void AddIfNotNull<T>(this ICollection<T> collection, T? value)
        {
            if(value != null)
            {
                collection.Add(value);
            }
        }

        public static bool IsTrue(this bool? value)
        {
            return value == true;
        }

        public static bool IsFalse(this bool? value)
        {
            return value == false;
        }
    }
}
