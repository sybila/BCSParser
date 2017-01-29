using System;
using System.Collections.Generic;

namespace BcsResolver.Extensions
{
    public static class FunctionalExtensions
    {
        public static TValue GetValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
            => dictionary[key];

        public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            if (!dictionary.TryGetValue(key, out value))
            {
                return default(TValue);
            }
            return value;
        }

        public static TTarget ApplyAction<TTarget>(this TTarget target, Action<TTarget> outerAction)
        {
            outerAction(target);
            return target;
        }

        public static TResult Apply<TTarget, TResult>(this TTarget target, Func<TTarget, TResult> outerFunction)
            => outerFunction(target);

        public static TOut CastTo<TOut>(this object original)
            where TOut : class
            => (TOut)original;

        public static TOut As<TOut>(this object original)
            where TOut : class
            => original as TOut;

        public static bool IsNot<T>(this object obj)
            where T : class
            => !(obj is T);

        public static bool Is<T>(this object obj)
            where T : class
            => obj is T;

        public static void Is<T>(this object obj, Action<T> action)
            where T : class
        {
            T tas = obj.As<T>();
            if (tas != null)
            {
                action(tas);
            }
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }
    }
}
