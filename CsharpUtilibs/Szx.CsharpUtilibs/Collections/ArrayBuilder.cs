using System;
using System.Collections.Generic;


namespace IDeal.Szx.CsharpUtilibs.Collections
{
    public interface IArrayBuilder<ElementType>
    {
        void Concat(ElementType[] array);
        IReadOnlyList<ElementType> ToReadOnlyList();
        void Clear();
        void Flush();
    }

    public class ArrayBuilder : ArrayBuilder<object>,
        IArrayBuilder<object> { }

    /// <summary>
    /// utility for improving performance on frequent one-dimensional array 
    /// concatenation. Order of elements will be the same as concatenation order.
    /// </summary>
#pragma warning disable 618
    public class ArrayBuilder<ElementType> : ArrayBuilderBasedOnArray<ElementType>,
        IArrayBuilder<ElementType> { }
#pragma warning restore 618

    /// <summary>
    /// a specific implementation of ArrayBuilderTest using <![CDATA[ElementType[]]]> as buffer. 
    /// </summary>
    [Obsolete("it may not get the best performance, please use ArrayBuilderTest<T> instead.")]
    public class ArrayBuilderBasedOnArray<ElementType> : IArrayBuilder<ElementType>
    {
        #region Constructor
        /// <summary> initialize buffer. </summary>
        /// <param name="array"> first array to fill the buffer. </param>
        /// <param name="concatCount"> possible number of invoking Concat(). </param>
        /// <param name="bufferCapacity"> possible number of elements. </param>
        public ArrayBuilderBasedOnArray(ElementType[] array = null,
            int concatCount = InitArrayNum, int bufferCapacity = InitBufferCapacity) {
            this.arrays = new List<ElementType[]>(concatCount);
            this.concatBuffer = new ElementType[bufferCapacity];
            this.concatedArrayNum = 0;
            this.concatedElementNum = 0;
            this.totalElementNum = 0;

            Concat(array);
        }
        #endregion Constructor

        #region Method
        /// <summary>
        /// record the array to be concatenated. <para />
        /// call Flush() or call Clone() on incoming arrays if you want to
        /// the elements not be influenced by changes on original arrays.
        /// </summary>
        public void Concat(ElementType[] array) {
            if (array == null) { return; }

            totalElementNum += array.Length;
            if (totalElementNum <= concatBuffer.Length) {
                array.CopyTo(concatBuffer, concatedElementNum);
                concatedElementNum = totalElementNum;
            } else {
                // TUNEUP[9]: if the length if small, expand the buffer and copy it?
                arrays.Add(array);
            }
        }

        /// <summary> get concatenated arrays. </summary>
        /// <returns>
        /// if all arrays in buffer is null or empty,           <para />
        ///     return empty array (size of 0 but not null).    <para />
        /// if there is arrays with actual elements in buffer,  <para />
        ///     return a readonly list of elements.
        /// </returns>
        /// <remarks> 
        /// the returned list is not a copy of the buffer, user could
        /// do deep/shallow copy themselves on demands.
        /// </remarks>
        public IReadOnlyList<ElementType> ToReadOnlyList() {
            if (totalElementNum == 0) { return new ElementType[0]; }
            Flush();
            return new ArraySegment<ElementType>(concatBuffer, 0, totalElementNum);
        }

        /// <summary> do real concatenation on arrays in the buffer. </summary>
        public void Flush() {
            if (concatBuffer.Length < totalElementNum) {
                ElementType[] oldBuffer = concatBuffer;
                AdjustCapacity(totalElementNum);
                oldBuffer.CopyTo(concatBuffer, 0);
            }

            for (; concatedArrayNum < arrays.Count; concatedArrayNum++) {
                arrays[concatedArrayNum].CopyTo(concatBuffer, concatedElementNum);
                concatedElementNum += arrays[concatedArrayNum].Length;
            }

            // TUNEUP[9]: if insertion will be support, please consider removing this 2 lines of code
            arrays.Clear();
            concatedArrayNum = 0;
        }

        /// <summary> 
        /// reset state of the buffer to allow new types of array concatenation. <para />
        /// it will remove all buffered elements but keep the resources. 
        /// </summary>
        public void Clear() {
            arrays.Clear();
            concatedArrayNum = 0;
            concatedElementNum = 0;
            totalElementNum = 0;
        }

        /// <summary> 
        /// adjust buffer to a size of some power of 2 next to required capacity. <para />
        /// this can also be used to shrink the buffer to save space. 
        /// </summary>
        private void AdjustCapacity(int requiredCapacity) {
            int newLength = requiredCapacity;
            newLength |= (newLength >> 1);
            newLength |= (newLength >> 2);
            newLength |= (newLength >> 4);
            newLength |= (newLength >> 8);
            newLength |= (newLength >> 16);
            newLength++;

            concatBuffer = new ElementType[newLength];
        }
        #endregion Method

        #region Property
        #endregion Property

        #region Type
        #endregion Type

        #region Constant
        /// <summary> the initial capacity of $arrays. </summary>
        public const int InitArrayNum = 4;
        /// <summary> the length of concatBuffer on the first call of ToReadOnlyList(). </summary>
        public const int InitBufferCapacity = 16;
        /// <summary> 
        /// threshold for deciding whether copy incoming array to buffer directly 
        /// or save it to $arrays temporarily.
        /// </summary>
        public const int ThresholdArrayLength = 4;
        #endregion Constant

        #region Field
        /// <summary> hold incoming arrays. </summary>
        private List<ElementType[]> arrays;
        /// <summary> hold the concatenated array. </summary>
        private ElementType[] concatBuffer;
        /// <summary> 
        /// number of arrays in $arrays which has been concatenated to concatBuffer. 
        /// it is also the index of next array to be concatenated in $arrays.
        /// </summary>
        private int concatedArrayNum;
        /// <summary> capacity of concatBuffer. </summary>
        private int concatedElementNum;
        /// <summary> number of elements in buffer. </summary>
        private int totalElementNum;
        #endregion Field
    }

    /// <summary>
    /// a specific implementation of ArrayBuilderTest using <![CDATA[List<ElementType>]]> as buffer. 
    /// </summary>
    [Obsolete("it may not get the best performance, please use ArrayBuilderTest<T> instead.")]
    public class ArrayBuilderBasedOnList<ElementType> : IArrayBuilder<ElementType>
    {
        #region Constructor
        /// <summary> initialize buffer. </summary>
        /// <param name="array"> first array to fill the buffer. </param>
        public ArrayBuilderBasedOnList(ElementType[] array = null) {
            this.arrays = new List<ElementType[]>();
            this.concatBuffer = new List<ElementType>();
            this.concatedArrayNum = 0;
            this.concatedElementNum = 0;
            this.totalElementNum = 0;

            Concat(array);
        }

        /// <summary> initialize buffer. </summary>
        /// <param name="concatCount"> possible number of invoking Concat(). </param>
        /// <param name="bufferCapacity"> possible number of elements. </param>
        public ArrayBuilderBasedOnList(int concatCount, int bufferCapacity) {
            this.arrays = new List<ElementType[]>(concatCount);
            this.concatBuffer = new List<ElementType>(bufferCapacity);
            this.concatedArrayNum = 0;
            this.concatedElementNum = 0;
            this.totalElementNum = 0;
        }

        /// <summary> initialize buffer. </summary>
        /// <param name="array"> first array to fill the buffer. </param>
        /// <param name="concatCount"> possible number of invoking Concat(). </param>
        /// <param name="bufferCapacity"> possible number of elements. </param>
        public ArrayBuilderBasedOnList(ElementType[] array,
            int concatCount, int bufferCapacity) {
            this.arrays = new List<ElementType[]>(concatCount);
            this.concatBuffer = new List<ElementType>(bufferCapacity);
            this.concatedArrayNum = 0;
            this.concatedElementNum = 0;
            this.totalElementNum = 0;

            Concat(array);
        }
        #endregion Constructor

        #region Method
        /// <summary>
        /// record the array to be concatenated. <para />
        /// call Flush() or call Clone() on incoming arrays if you want to
        /// the elements not be influenced by changes on original arrays.
        /// </summary>
        public void Concat(ElementType[] array) {
            if (array == null) { return; }

            totalElementNum += array.Length;
            if (totalElementNum <= concatBuffer.Capacity) {
                concatBuffer.AddRange(array);
                concatedElementNum = totalElementNum;
            } else {
                // TUNEUP[9]: if the length if small, expand the buffer and copy it?
                arrays.Add(array);
            }
        }

        /// <summary>
        /// get concatenated arrays.
        /// </summary>
        /// <returns>
        /// if all arrays in buffer is null or empty, <para />
        ///     return empty array (size of 0 but not null). <para />
        /// if there is arrays with actual elements in buffer, <para />
        ///     return a readonly list of elements.
        /// </returns>
        /// <remarks> 
        /// the returned list is not a copy of the buffer, user could
        /// do deep/shallow copy themselves on demands.
        /// </remarks>
        public IReadOnlyList<ElementType> ToReadOnlyList() {
            if (totalElementNum == 0) { return new ElementType[0]; }
            Flush();
            return concatBuffer;
        }

        /// <summary> do real concatenation on arrays in the buffer. </summary>
        public void Flush() {
            if (concatBuffer.Capacity < totalElementNum) {
                concatBuffer.Capacity = totalElementNum + BufferedCapacity;
            }

            for (; concatedArrayNum < arrays.Count; concatedArrayNum++) {
                concatBuffer.AddRange(arrays[concatedArrayNum]);
                concatedElementNum += arrays[concatedArrayNum].Length;
            }

            // TUNEUP[9]: if insertion will be support, please consider removing this 2 lines of code
            arrays.Clear();
            concatedArrayNum = 0;
        }

        /// <summary> 
        /// reset state of the buffer to allow new types of array concatenation. <para />
        /// it will remove all buffered elements but keep the resources. 
        /// </summary>
        public void Clear() {
            arrays.Clear();
            concatBuffer.Clear();
            concatedArrayNum = 0;
            concatedElementNum = 0;
            totalElementNum = 0;
        }
        #endregion Method

        #region Property
        #endregion Property

        #region Type
        #endregion Type

        #region Constant
        /// <summary> the initial capacity of $arrays. </summary>
        public const int InitArrayNum = 4;
        /// <summary> the preserved capacity for incoming short arrays. </summary>
        public const int BufferedCapacity = 16;
        /// <summary> 
        /// threshold for deciding whether copy incoming array to buffer directly 
        /// or save it to $arrays temporarily.
        /// </summary>
        public const int ThresholdArrayLength = BufferedCapacity / InitArrayNum;
        #endregion Constant

        #region Field
        /// <summary> hold incoming arrays. </summary>
        private List<ElementType[]> arrays;
        /// <summary> hold the concatenated array. </summary>
        private List<ElementType> concatBuffer;
        /// <summary> 
        /// number of arrays in $arrays which has been concatenated to concatBuffer. 
        /// it is also the index of next array to be concatenated in $arrays.
        /// </summary>
        private int concatedArrayNum;
        /// <summary> capacity of concatBuffer. </summary>
        private int concatedElementNum;
        /// <summary> number of elements in buffer. </summary>
        private int totalElementNum;
        #endregion Field
    }
}
