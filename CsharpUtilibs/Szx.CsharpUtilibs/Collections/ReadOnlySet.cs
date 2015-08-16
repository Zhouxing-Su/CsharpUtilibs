using System.Collections;
using System.Collections.Generic;


namespace IDeal.Szx.CsharpUtilibs.Collections
{
    public class ReadOnlySet<T> : ISet<T>
    {
        #region Constructor
        public ReadOnlySet(ISet<T> set) { this.set = set; }
        #endregion Constructor

        #region Method
        #region not implemented
        bool ISet<T>.Add(T item) {
            throw new System.NotImplementedException();
        }

        void ISet<T>.ExceptWith(IEnumerable<T> other) {
            throw new System.NotImplementedException();
        }

        void ISet<T>.IntersectWith(IEnumerable<T> other) {
            throw new System.NotImplementedException();
        }

        void ISet<T>.SymmetricExceptWith(IEnumerable<T> other) {
            throw new System.NotImplementedException();
        }

        void ISet<T>.UnionWith(IEnumerable<T> other) {
            throw new System.NotImplementedException();
        }

        void ICollection<T>.Add(T item) {
            throw new System.NotImplementedException();
        }

        void ICollection<T>.Clear() {
            throw new System.NotImplementedException();
        }

        bool ICollection<T>.Remove(T item) {
            throw new System.NotImplementedException();
        }
        #endregion not implemented

        public bool IsProperSubsetOf(IEnumerable<T> other) {
            return set.IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other) {
            return set.IsProperSupersetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<T> other) {
            return set.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other) {
            return set.IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other) {
            return set.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<T> other) {
            return set.SetEquals(other);
        }

        public bool Contains(T item) {
            return set.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            set.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator() {
            return set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return set.GetEnumerator();
        }
        #endregion Method

        #region Property
        public int Count {
            get { return set.Count; }
        }

        public bool IsReadOnly {
            get { return set.IsReadOnly; }
        }
        #endregion Property

        #region Type
        #endregion Type

        #region Constant
        #endregion Constant

        #region Field
        ISet<T> set;
        #endregion Field
    }
}
