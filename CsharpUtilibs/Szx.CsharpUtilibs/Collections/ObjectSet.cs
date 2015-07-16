using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;


namespace Szx.CsharpUtilibs.Collections
{
    /// <summary> 
    /// provide simple interface to track objects 
    /// and check if an object is tracked.
    /// </summary>
    public interface IObjectSet
    {
        /// <summary> check the existence of an object. </summary>
        /// <returns> true if object is exist, false otherwise. </returns>
        bool IsExist(object obj);

        /// <summary> if the object is not in the set, add it in. else do nothing. </summary>
        /// <returns> true if successfully added, false otherwise. </returns>
        bool Add(object obj);
    }

#pragma warning disable 618
    public sealed class ObjectSet : ObjectSetUsingConditionalWeakTable { }
#pragma warning restore 618

    [Obsolete("it may not get the best performance, please use ObjectSet instead.")]
    public class ObjectSetUsingConditionalWeakTable : IObjectSet
    {
        #region Constructor
        #endregion

        #region Method
        public bool IsExist(object obj) {
            return objectSet.TryGetValue(obj, out tryGetValue_out0);
        }

        public bool Add(object obj) {
            if (IsExist(obj)) {
                return false;
            } else {
                objectSet.Add(obj, null);
                return true;
            }
        }
        #endregion

        #region Property
        #endregion

        #region Type
        #endregion

        #region Constant
        #endregion

        #region Field
        /// <summary> internal representation of the set. (only use the key) </summary>
        private ConditionalWeakTable<object, object> objectSet = new ConditionalWeakTable<object, object>();

        /// <summary> used to fill the out parameter of ConditionalWeakTable.TryGetValue(). </summary>
        private static object tryGetValue_out0 = null;
        #endregion
    }

    [Obsolete("it will crash if there are too many objects, please use ObjectSet instead.")]
    public sealed class ObjectSetUsingObjectIDGenerator : IObjectSet
    {
        #region Constructor
        #endregion

        #region Method
        public bool IsExist(object obj) {
            bool firstTime;
            idGenerator.HasId(obj, out firstTime);
            return !firstTime;
        }

        public bool Add(object obj) {
            bool firstTime;
            idGenerator.GetId(obj, out firstTime);
            return firstTime;
        }
        #endregion

        #region Property
        #endregion

        #region Type
        #endregion

        #region Constant
        #endregion

        #region Field
        /// <summary> internal representation of the set. </summary>
        private ObjectIDGenerator idGenerator = new ObjectIDGenerator();
        #endregion
    }
}
