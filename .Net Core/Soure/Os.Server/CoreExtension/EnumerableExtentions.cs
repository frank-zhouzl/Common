using System;
using System.Collections.Generic;
using System.Text;

namespace CoreExtension
{
    /// <summary>
    /// Enumerable拓展方法
    /// </summary>
    public static partial class EnumerableExtentions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> process)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (process == null)
            {
                throw new ArgumentNullException(nameof(process));
            }

            foreach (T item in source)
            {
                process(item);
            }
        }
    }
}
