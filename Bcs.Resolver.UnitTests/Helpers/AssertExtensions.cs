using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BcsResolver.Tests.Helpers
{
    public static class AssertExtensions
    {
        public static TTarget AssertCast<TTarget>(this object source)
        {
            if (!(source is TTarget))
            {
                throw new AssertFailedException($"Source object ({source.GetType().FullName}) is not of expected type {typeof(TTarget).FullName}.");
            }
            return (TTarget) source;
        }

        public static TEnumerable AssertCount<TEnumerable>(this TEnumerable enumerable, int expectedCount)
            where TEnumerable : IEnumerable
        {
            var count = enumerable.OfType<object>().Count();
            if (count != expectedCount)
            {
                throw new AssertFailedException($"Enumerable has incorect element count expected: {expectedCount} found: {count}");
            }
            return enumerable;
        }

        public static void AssertSequenceEquals<T>(this IEnumerable<T> expected, IEnumerable<T> tested)
        {
            var e = expected as IList<T> ?? expected.ToList();
            var t = tested as IList<T> ?? tested.ToList();

            if (e.Count() != t.Count())
            {
                throw new AssertFailedException($"Sequence lenghts differ. Expected: {e.Count()} found: {t.Count()}");
            }

            for (var i = 0; i < e.Count; i++)
            {
                var expectedElement = e[i];
                var testeElement = t[i];
                if (!expectedElement.Equals(testeElement))
                {
                    throw new AssertFailedException($"Sequences differ. Position: {i} Expected element: {expectedElement.ToString()} found: {testeElement.ToString()}");
                }
            }
        }
    }
}