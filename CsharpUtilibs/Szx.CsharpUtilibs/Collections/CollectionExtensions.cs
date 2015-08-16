//#define WITHOUT_LINQ


using System;
using System.Collections;
using System.Collections.Generic;


namespace IDeal.Szx.CsharpUtilibs.Collections
{
    internal static class ArrayExtensions
    {
        /// <summary> put lengths of all dimensions in int[] and return it. </summary>
        public static int[] GetLengths(this Array array) {
            int[] lengths = new int[array.Rank];
            for (int i = 0; i < lengths.Length; i++) {
                lengths[i] = array.GetLength(i);
            }
            return lengths;
        }

        public static void Fill<T>(this IList<T> list, T value) {
            for (int i = 0; i < list.Count; i++) {
                list[i] = value;
            }
        }
        public static void Fill<T>(this IList<T> list, T value, int offset, int length) {
            for (int i = offset; i < length; i++) {
                list[i] = value;
            }
        }
    }

    internal static class QueueOrStackExtension
    {
        public static void Add(this Stack s, object obj) {
            s.Push(obj);
        }
        public static void Add<T>(this Stack<T> s, object obj) {
            s.Push((T)obj);
        }
        public static void Add(this Queue s, object obj) {
            s.Enqueue(obj);
        }
        public static void Add<T>(this Queue<T> s, object obj) {
            s.Enqueue((T)obj);
        }
    }

    internal static class IEnumerableExtensions
    {
        public static object First(this IEnumerable collection) {
            IEnumerator enumerator = collection.GetEnumerator();
            enumerator.MoveNext();
            return enumerator.Current;
        }

#if WITHOUT_LINQ
        public static object First<T>(this IEnumerable<T> collection) {
            using (IEnumerator<T> enumerator = collection.GetEnumerator()) {
                enumerator.MoveNext();
                return enumerator.Current;
            }
        }
#endif
        public static object GetFirst(IEnumerable collection) {
            return collection.First();
        }

        public static object GetFirst<T>(IEnumerable<T> collection) {
            return collection.First();
        }
    }
}
