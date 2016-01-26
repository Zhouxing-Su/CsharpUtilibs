using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IDeal.Szx.CsharpUtilibs.Collections {
    public class MultiDimArray<T> : IList<T> {
        #region Constructor
        public MultiDimArray(params int[] lengths) {
            this.lengths = (int[])lengths.Clone();
            this.steps = new int[lengths.Length];
            this.length = 1;
            for (int i = 0; i < lengths.Length; i++) {
                this.steps[i] = this.length;
                this.length *= lengths[i];
            }
            this.data = new T[length];
        }
        #endregion Constructor

        #region Method
        #region IList<T> interface
        public int IndexOf(T item) {
            return Array.IndexOf(data, item);
        }

        public bool Contains(T item) {
            return data.Contains(item);
        }

        /// <summary> copy data to simple array. </summary>
        public void CopyTo(T[] array, int arrayIndex) {
            data.CopyTo(array, arrayIndex);
        }
        /// <summary> copy data and leave the original dimensions untouched. </summary>
        public void CopyTo(MultiDimArray<T> array, int arrayIndex) {
            data.CopyTo(array.data, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return data.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            return (IEnumerator<T>)data.GetEnumerator();
        }

        #region not supported interface
        public void Insert(int index, T item) {
            throw new NotSupportedException();
        }
        public void RemoveAt(int index) {
            throw new NotSupportedException();
        }
        public bool Remove(T item) {
            throw new NotSupportedException();
        }
        public void Add(T item) {
            throw new NotSupportedException();
        }
        public void Clear() {
            throw new NotSupportedException();
        }
        #endregion not supported interface
        #endregion IList<T> interface

        private int GetStep(params int[] index) {
            int step = 0;
            for (int i = 0; i < index.Length;) {
                int tmp = steps[i];
                tmp *= (index[index.Length - (++i)]);
                step += tmp;
            }
            return step;
        }
        #endregion Method

        #region Property
        public T this[int i0] {
            get { return data[i0]; }
            set { data[i0] = value; }
        }

        public T this[int i0, int i1] {
            get { return data[i0 * steps[1] + i1]; }
            set { data[i0 * steps[1] + i1] = value; }
        }

        public T this[int i0, int i1, int i2] {
            get { return data[i0 * steps[2] + i1 * steps[1] + i2]; }
            set { data[i0 * steps[2] + i1 * steps[1] + i2] = value; }
        }

        public T this[int i0, int i1, int i2, int i3] {
            get { return data[i0 * steps[3] + i1 * steps[2] + i2 * steps[1] + i3]; }
            set { data[i0 * steps[3] + i1 * steps[2] + i2 * steps[1] + i3] = value; }
        }

        public T this[params int[] index] {
            get { return data[GetStep(index)]; }
            set { data[GetStep(index)] = value; }
        }

        public int Rank { get { return lengths.Length; } }
        public int Length { get { return length; } }
        public IReadOnlyList<int> Lengths { get { return lengths; } }

        #region IList<T> interface
        public int Count { get { return Length; } }
        public bool IsReadOnly { get { return false; } }
        #endregion IList<T> interface
        #endregion Property

        #region Type
        #endregion Type

        #region Constant
        #endregion Constant

        #region Field
        /// <summary> real data. </summary>
        private T[] data;
        /// <summary> length of each dimension. </summary>
        /// <remarks>
        /// dim     0 1 2 3 ... n
        /// lengths 0 1 2 3 ... n
        /// </remarks>
        private int[] lengths;
        /// <summary> number of index increment when certain dimension increasing. </summary>
        /// <remarks>
        /// dim   0 1 2 3 ... n
        /// steps n ... 3 2 1 0
        /// </remarks>
        private int[] steps;
        /// <summary> total length. </summary>
        private int length;
        #endregion Field
    }
}
