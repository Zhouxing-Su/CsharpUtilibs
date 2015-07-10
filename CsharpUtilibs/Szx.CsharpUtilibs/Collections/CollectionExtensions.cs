using System;
using System.Collections;
using System.Collections.Generic;


namespace Szx.CsharpUtilibs.Collections
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
}
