using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Szx.CsharpUtilibs.Serialization
{
    using Szx.CsharpUtilibs.Collections;
    using Szx.CsharpUtilibs.Test;


    public class SerializerBase
    {
        /// <summary> unit test on object set. </summary>
        internal static void Main() {
            C1 a = new C1();
            new SerializerBase().Traverse(a);
        }

        #region Constructor
        public SerializerBase() : this(ConsoleWriter) { }

        public SerializerBase(IVisitor visitor) {
            this.visitor = visitor;
        }
        #endregion

        #region Method
        /// <summary> get fields including which in base classes. </summary>
        public IReadOnlyList<FieldInfo> GetAllFields(object obj, VisitPolicy policy) {
            ArrayBuilder<FieldInfo> fieldInfos = new ArrayBuilder<FieldInfo>();

            Type t = obj.GetType();
            do {
                fieldInfos.Concat(t.GetFields((BindingFlags)policy));
                t = t.BaseType;
            } while (t != typeof(object));

            return fieldInfos.ToReadOnlyList();
        }

        /// <summary> get fields including which in base classes. </summary>
        public IReadOnlyList<FieldInfo> GetAllFields(object obj) {
            return GetAllFields(obj, visitor.Policy);
        }

        /// <summary> 
        /// visit every object in the graph except root. <para />
        /// sequence of events is described in IVisitor interface.
        /// </summary>
        public void Traverse(object obj) {
            visitor.OnEnterTraverse();
            InternalTraverse(obj);
            visitor.OnLeaveTraverse();
        }

        /// <summary> internal implementation invoked by Traverse(). </summary>
        /// <remarks>
        /// System.Collections categorization: <para />
        /// <![CDATA[
        ///     IDictionary  :  void Add(Object key, Object value) <--> foreach
        ///         Dictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, 
        ///             IDictionary, ICollection, IReadOnlyDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, 
        ///             TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, ISerializable, 
        ///             IDeserializationCallback
        ///         SortedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, 
        ///             IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, ICollection, IEnumerable  
        ///         SortedList<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, 
        ///             IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, ICollection, IEnumerable  
        ///         Hashtable : IDictionary, ICollection, IEnumerable, ISerializable, IDeserializationCallback, ICloneable  
        ///         SortedList : IDictionary, ICollection, IEnumerable, ICloneable  
        ///         
        ///     IList  :  void Add(Object item) <--> foreach
        ///         ArrayList : IList, ICollection, IEnumerable, ICloneable  
        ///         *List<T> : IList<T>, ICollection<T>, IList, ICollection, IReadOnlyList<T>, IReadOnlyCollection<T>, 
        ///             IEnumerable<T>, IEnumerable  
        ///     
        ///     ICollection<T>  :  void Add(T item) <--> foreach
        ///         HashSet<T> : ISerializable, IDeserializationCallback, ISet<T>, ICollection<T>, IEnumerable<T>, 
        ///             IEnumerable  
        ///         LinkedList<T> : ICollection<T>, IEnumerable<T>, ICollection, IEnumerable, ISerializable, 
        ///             IDeserializationCallback  
        ///         *List<T> : IList<T>, ICollection<T>, IList, ICollection, IReadOnlyList<T>, IReadOnlyCollection<T>, 
        ///             IEnumerable<T>, IEnumerable  
        ///         SortedSet<T> : ISet<T>, ICollection<T>, IEnumerable<T>, ICollection, IEnumerable, ISerializable, 
        ///             IDeserializationCallback  
        ///         
        ///     BitArray  :  BitArray(Int32[]) <--> void CopyTo(Array array, int index) + foreach
        ///         BitArray : ICollection, IEnumerable, ICloneable  
        ///         
        ///     Stack | Stack<T>  :  void Push( Object obj ) <--> Object[] ToArray() + foreach
        ///         Stack : ICollection, IEnumerable, ICloneable  
        ///         Stack<T> : IEnumerable<T>, ICollection, IEnumerable
        ///         
        ///     Queue | Queue<T>  :  void Enqueue( Object obj ) <--> Object[] ToArray() + foreach
        ///         Queue : ICollection, IEnumerable, ICloneable  
        ///         Queue<T> : IEnumerable<T>, ICollection, IEnumerable  
        /// ]]> 
        /// </remarks>
        // TODO1: pass value type by ref ?
        private void InternalTraverse(object obj) {
            if ((obj == null) || !visitedObjects.Add(obj)) { return; }

            Type t = obj.GetType();
            if (t.IsArray) {
                TraverseCollection((Array)obj, visitor.OnEnterArray, visitor.OnLeaveArray);
            } else if (obj is BitArray) {
                TraverseCollection((BitArray)obj,
                    visitor.OnEnterBitArray, visitor.OnLeaveBitArray);
            } else if (((obj is Stack) || (typeof(Stack<>).IsAssignableFrom(t)))
                || ((obj is Queue) || (typeof(Queue<>).IsAssignableFrom(t)))) {
                TraverseCollection((IEnumerable)obj,
                    visitor.OnEnterQueueOrStack, visitor.OnLeaveQueueOrStack);
            } else if (obj is IDictionary) {    // with `void Add(Object key, Object value)`
                TraverseCollection((IDictionary)obj,
                    visitor.OnEnterDictionary, visitor.OnLeaveDictionary);
            } else if ((obj is IList)) {  // with `void Add(Object item)`
                TraverseCollection((IList)obj, visitor.OnEnterList, visitor.OnLeaveList);
            } else if (typeof(ICollection<>).IsAssignableFrom(t)) {
                TraverseCollection((ICollection<object>)obj,
                    visitor.OnEnterCollection, visitor.OnLeaveCollection);
            } else {    // non-collection types
                IReadOnlyList<FieldInfo> fieldInfos =
                    t.IsPrimitive ? GetAllFields(obj, VisitPolicy.Instance) : GetAllFields(obj);
                visitor.OnEnterNode(obj, fieldInfos);
                foreach (FieldInfo f in fieldInfos) {
                    if (f.IsLiteral) { continue; }
                    object subobj = f.GetValue(obj);
                    if ((f.FieldType.IsPrimitive) || (subobj is string)) {
                        visitor.OnVisitLeaf(subobj, f);
                    } else {
                        visitor.OnEnterNonLeaf(subobj, f);
                        InternalTraverse(subobj);
                        visitor.OnLeaveNonLeaf(subobj, f);
                    }
                }
                visitor.OnLeaveNode(obj, fieldInfos);
            }
        }

        private void TraverseCollection<T>(T collection,
            Action<T> OnVisit, Action<T> OnLeave) where T : IEnumerable {
            OnVisit(collection);
            foreach (object item in collection) {
                InternalTraverse(item);
            }
            OnLeave(collection);
        }
        #endregion

        #region Property
        #endregion

        #region Type
        #endregion

        #region Constant
        public const BindingFlags BindAllFields = BindingFlags.Public
            | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        public static readonly TextWriterVisitor ConsoleWriter
            = new TextWriterVisitor(Console.Out);
        #endregion

        #region Field
        private IVisitor visitor;

        private ObjectSet visitedObjects = new ObjectSet();
        #endregion
    }
}
