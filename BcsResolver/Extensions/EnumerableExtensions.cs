using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsResolver.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Interleave<T>(this IList<T> list1, IList<T> list2)
        {
            int length = Math.Min(list1.Count, list2.Count);

            // Combine the first 'length' elements from both lists into pairs
            return list1.Take(length)
            .Zip(list2.Take(length), (a, b) => new [] { a, b })
            // Flatten out the pairs
            .SelectMany(array => array)
            // Concatenate the remaining elements in the lists)
            .Concat(list1.Skip(length))
            .Concat(list2.Skip(length));
        }
    }
}
