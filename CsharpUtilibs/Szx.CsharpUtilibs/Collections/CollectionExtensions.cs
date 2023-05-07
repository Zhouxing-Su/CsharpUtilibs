//#define WITHOUT_LINQ


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace IDeal.Szx.CsharpUtilibs.Collections {
    internal static class ArrayExtensions {
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

        public static void Pop<T>(this List<T> l) { l.RemoveAt(l.Count - 1); }

        public static bool LexLess<T>(T[] l, T[] r) where T : IComparable<T> {
            int len = Math.Min(l.Length, r.Length);
            for (int i = 0; i < len; ++i) {
                int diff = l[i].CompareTo(r[i]);
                if (diff != 0) { return diff < 0; }
            }
            return l.Length < r.Length;
        }
    }

    internal static class QueueOrStackExtension {
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

    internal static class IEnumerableExtensions {
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

    internal static class DictExtension {
        public static void TryInc<TKey>(IDictionary<TKey, int> dictionary, TKey key) {
            if (!dictionary.ContainsKey(key)) { dictionary.Add(key, 0); }
            ++dictionary[key];
        }
        public static void TryDec<TKey>(IDictionary<TKey, int> dictionary, TKey key) {
            if (--dictionary[key] <= 0) { dictionary.Remove(key); }
        }
        public static T TryAdd<TKey, T>(IDictionary<TKey, T> dictionary, TKey key) where T : new() {
            if (!dictionary.ContainsKey(key)) { dictionary.Add(key, new T()); }
            return dictionary[key];
        }

        public static bool TryUpdateMin<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue value) where TValue : IComparable<TValue> {
            if (!dictionary.ContainsKey(key)) { dictionary.Add(key, value); return true; }
            if (value.CompareTo(dictionary[key]) < 0) { dictionary[key] = value; return true; }
            return false;
        }

        public static int[] OrderedKeys<T>(IDictionary<int, T> mapping) {
            int[] keys = mapping.Keys.ToArray();
            Array.Sort(keys);
            return keys;
        }
        public static int[] MapBack(IDictionary<int, int> mapping) {
            int[] keys = OrderedKeys(mapping);
            for (int i = 0; i < keys.Length; ++i) { mapping[keys[i]] = i; }
            return keys;
        }
    }
}
