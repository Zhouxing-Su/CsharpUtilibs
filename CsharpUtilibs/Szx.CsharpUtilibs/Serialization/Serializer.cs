using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;


namespace IDeal.Szx.CsharpUtilibs.Serialization
{
    using IDeal.Szx.CsharpUtilibs.Collections;
    using IDeal.Szx.CsharpUtilibs.Test;


    public class SerializerBase
    {
        #region Constructor
        public SerializerBase() : this(ConsoleWriter) { }

        public SerializerBase(IVisitor visitor) {
            this.visitor = visitor;
        }
        #endregion Constructor

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
        // TODO[8]: return the buffer to reduce time holding resources?
        public void Traverse(object obj) {
            visitor.OnEnterTraverse();
            if (obj != null) { InternalTraverse(obj); }
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
        ///     BitArray  :  BitArray(Int32[]) <--> void CopyTo(Array data, int index) + foreach
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
        // TODO[1]: pass value type by ref ?
        private void InternalTraverse(object obj) {
            Type t = obj.GetType();
            if (obj is string) {
                TraverseObject(obj);
            } else if (t.IsArray) {
                TraverseCollection((Array)obj, visitor.OnEnterArray, visitor.OnLeaveArray);
            } else if (t == typeof(BitArray)) {
                TraverseCollection((BitArray)obj,
                    visitor.OnEnterBitArray, visitor.OnLeaveBitArray);
            } else if (obj is IDictionary) {  // with `void Add(Object key, Object value)`
                TraverseCollection((IDictionary)obj,
                    visitor.OnEnterDictionary, visitor.OnLeaveDictionary);
            } else if (obj is IList) {      // with `void Add(Object item)`
                TraverseCollection((IList)obj, visitor.OnEnterList, visitor.OnLeaveList);
            } else if (obj is IEnumerable) {      // any other collections left 
                TraverseCollection((IEnumerable)obj,    // (must implement `void Add(Object item)`)
                    visitor.OnEnterCollection, visitor.OnLeaveCollection);
            } else {    // non-collection types
                IReadOnlyList<FieldInfo> fieldInfos =
                    t.IsPrimitive ? GetAllFields(obj, VisitPolicy.Instance) : GetAllFields(obj);
                foreach (FieldInfo f in fieldInfos) {
                    if (f.IsLiteral) { continue; }
                    TraverseObject(f.GetValue(obj), f);
                }
            }
        }

        private void TraverseCollection(IEnumerable collection,
            Action<object> OnVisit, Action<object> OnLeave) {
            OnVisit(collection);
            foreach (object item in collection) {
                TraverseObject(item);
            }
            OnLeave(collection);
        }

        private void TraverseObject(object obj, FieldInfo fieldInfo = null) {
            visitor.OnEnterNode(obj, fieldInfo);
            if (obj != null) {
                string id;
                if (visitedObjects.TryGetValue(obj, out id)) {
                    obj = id;
                } else {
                    visitedObjects.Add(obj, ObjectIdPrefix + (objectID++));
                }

                if (obj.GetType().IsPrintable()) {
                    visitor.OnVisitLeaf(obj, fieldInfo);
                } else {
                    visitor.OnEnterNonLeaf(obj, fieldInfo);
                    InternalTraverse(obj);
                    visitor.OnLeaveNonLeaf(obj, fieldInfo);
                }
            } else {    // print "null"
                visitor.OnVisitLeaf(obj, fieldInfo);
            }
            visitor.OnLeaveNode(obj, fieldInfo);
        }
        #endregion Method

        #region Property
        #endregion Property

        #region Type
        #endregion Type

        #region Constant
        public const BindingFlags BindAllFields = BindingFlags.Public
            | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        public static readonly TextWriterVisitor ConsoleWriter
            = new TextWriterVisitor(Console.Out);

        /// <summary> concatenated with $objectID to form the ID string. </summary>
        // TUNEUP[9]: this may conflict if there is a string "@N"
        public const string ObjectIdPrefix = "\uE000@\uF8FF\u0000#";
        #endregion Constant

        #region Field
        protected IVisitor visitor;

        /// <summary> serial number of objects. </summary>
        protected int objectID = 0;
        /// <summary> record visited objects and their ID string. </summary>
        protected ConditionalWeakTable<object, string> visitedObjects = new ConditionalWeakTable<object, string>();
        #endregion Field
    }
}
