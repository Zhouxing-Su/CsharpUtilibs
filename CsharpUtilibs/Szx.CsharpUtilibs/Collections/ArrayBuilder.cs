using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace Szx.CsharpUtilibs.Collections
{
    public class ArrayBuilder<ElementType> : ArrayBuilderBasedOnArray<ElementType>
    {
        /// <summary> unit test. </summary>
        /// <example> 
        /// [Test1]                                                         <para />
        /// Array.CopyTo()1 : 3244 (no allocate)                            <para />
        /// Array.CopyTo()2 : 6709 (allocate on every concat)               <para />
        /// ArrayBuilderBasedOnList1 : 6840 (allocate on output only)       <para />
        /// ArrayBuilderBasedOnList2 : 5790 (allocate on every concat)      <para />
        /// ArrayBuilderBasedOnArray1 : 3930 (allocate on output only)      <para />
        /// ArrayBuilderBasedOnArray2 : 4128 (allocate on every concat)     <para />
        /// StringBuilder1 : 768 (allocate on output only)                  <para />
        /// StringBuilder2 : 784 (allocate on every concat)                 <para />
        /// ====================================                            <para />
        /// Array.CopyTo()1 : 5135 (no allocate)                            <para />
        /// Array.CopyTo()2 : 14760 (allocate on every concat)              <para />
        /// ArrayBuilderBasedOnList1 : 15578 (allocate on output only)      <para />
        /// ArrayBuilderBasedOnList2 : 16569 (allocate on every concat)     <para />
        /// ArrayBuilderBasedOnArray1 : 5964 (allocate on output only)      <para />
        /// ArrayBuilderBasedOnArray2 : 7368 (allocate on every concat)     <para />
        /// StringBuilder1 : 2701 (allocate on output only)                 <para />
        /// StringBuilder2 : 9123 (allocate on every concat)                <para />
        /// ====================================                            <para />
        /// Array.CopyTo()1 : 5816 (no allocate)                            <para />
        /// Array.CopyTo()2 : 82660 (allocate on every concat)              <para />
        /// ArrayBuilderBasedOnList1 : 17794 (allocate on output only)      <para />
        /// ArrayBuilderBasedOnList2 : 19414 (allocate on every concat)     <para />
        /// ArrayBuilderBasedOnArray1 : 6625 (allocate on output only)      <para />
        /// ArrayBuilderBasedOnArray2 : 8168 (allocate on every concat)     <para />
        /// StringBuilder1 : 10217 (allocate on output only)                <para />
        /// StringBuilder2 : 71296 (allocate on every concat)               <para />
        /// ====================================                            <para />
        /// Array.CopyTo()1 : 8741 (no allocate)                            <para />
        /// Array.CopyTo()2 : 467125 (allocate on every concat)             <para />
        /// ArrayBuilderBasedOnList1 : 26149 (allocate on output only)      <para />
        /// ArrayBuilderBasedOnList2 : 28850 (allocate on every concat)     <para />
        /// ArrayBuilderBasedOnArray1 : 8472 (allocate on output only)      <para />
        /// ArrayBuilderBasedOnArray2 : 10195 (allocate on every concat)    <para />
        /// StringBuilder1 : 42324 (allocate on output only)                <para />
        /// StringBuilder2 : 391748 (allocate on every concat)              <para />
        /// ====================================                            <para />
        /// [Test2]                                                         <para />
        /// Array.CopyTo()1 : 4983 (no allocate)                            <para />
        /// Array.CopyTo()2 : 10701 (allocate on every concat)              <para />
        /// ArrayBuilderBasedOnList1 : 8583 (allocate on output only)       <para />
        /// ArrayBuilderBasedOnList2 : 10637 (allocate on every concat)     <para />
        /// ArrayBuilderBasedOnArray1 : 6073 (allocate on output only)      <para />
        /// ArrayBuilderBasedOnArray2 : 5942 (allocate on every concat)     <para />
        /// StringBuilder1 : 1804 (allocate on output only)                 <para />
        /// StringBuilder2 : 1708 (allocate on every concat)                <para />
        /// ====================================                            <para />
        /// Array.CopyTo()1 : 9628 (no allocate)                            <para />
        /// Array.CopyTo()2 : 20590 (allocate on every concat)              <para />
        /// ArrayBuilderBasedOnList1 : 20839 (allocate on output only)      <para />
        /// ArrayBuilderBasedOnList2 : 22764 (allocate on every concat)     <para />
        /// ArrayBuilderBasedOnArray1 : 8468 (allocate on output only)      <para />
        /// ArrayBuilderBasedOnArray2 : 11641 (allocate on every concat)    <para />
        /// StringBuilder1 : 3656 (allocate on output only)                 <para />
        /// StringBuilder2 : 12308 (allocate on every concat)               <para />
        /// ====================================                            <para />
        /// Array.CopyTo()1 : 7832 (no allocate)                            <para />
        /// Array.CopyTo()2 : 118323 (allocate on every concat)             <para />
        /// ArrayBuilderBasedOnList1 : 24357 (allocate on output only)      <para />
        /// ArrayBuilderBasedOnList2 : 24680 (allocate on every concat)     <para />
        /// ArrayBuilderBasedOnArray1 : 10211 (allocate on output only)     <para />
        /// ArrayBuilderBasedOnArray2 : 11257 (allocate on every concat)    <para />
        /// StringBuilder1 : 14749 (allocate on output only)                <para />
        /// StringBuilder2 : 102166 (allocate on every concat)              <para />
        /// ====================================                            <para />
        /// Array.CopyTo()1 : 12962 (no allocate)                           <para />
        /// Array.CopyTo()2 : 759865 (allocate on every concat)             <para />
        /// ArrayBuilderBasedOnList1 : 42077 (allocate on output only)      <para />
        /// ArrayBuilderBasedOnList2 : 41932 (allocate on every concat)     <para />
        /// ArrayBuilderBasedOnArray1 : 13186 (allocate on output only)     <para />
        /// ArrayBuilderBasedOnArray2 : 14975 (allocate on every concat)    <para />
        /// StringBuilder1 : 64633 (allocate on output only)                <para />
        /// StringBuilder2 : 628920 (allocate on every concat)              <para />
        /// ====================================
        /// </example>
        internal new static void Main() {
            //ArrayBuilderBasedOnArray<ElementType>.Main();
            //ArrayBuilderBasedOnList<ElementType>.Main();

            IReadOnlyList<char> rl;
            string s;
            char[] ra = new char[0];

            const int LoopCount = 100000000;
            Stopwatch sw = new Stopwatch();

            for (int k = 0; k < 4; k++) {
                char[] array = new char[5 * k * k * k * k];

                //sw.Restart();
                //for (int i = 0; i < LoopCount; i++) {
                //    ra = ra.Concat(array).ToArray();
                //    if ((i % 10) == 0) {
                //        if ((i % 100) == 0) {
                //            ra = new char[0];
                //        }
                //    }
                //}
                //sw.Stop();
                //Console.WriteLine("Array.Concat() : {0} (allocate on every concat)", sw.ElapsedMilliseconds);

                sw.Restart();
                ra = new char[array.Length * 100];
                int length = 0;
                for (int i = 0; i < LoopCount; i++) {
                    if ((i % 100) == 0) {
                        length = 0;
                    }
                    array.CopyTo(ra, length);
                    length += array.Length;
                    if ((i % 10) == 0) {
                    }
                }
                sw.Stop();
                Console.WriteLine("Array.CopyTo()1 : {0} (no allocate)", sw.ElapsedMilliseconds);

                sw.Restart();
                for (int i = 0; i < LoopCount; i++) {
                    if ((i % 100) == 0) {
                        ra = new char[0];
                    }
                    char[] r = new char[ra.Length + array.Length];
                    ra.CopyTo(r, 0);
                    array.CopyTo(r, ra.Length);
                    ra = r;
                    if ((i % 10) == 0) {
                    }
                }
                sw.Stop();
                Console.WriteLine("Array.CopyTo()2 : {0} (allocate on every concat)", sw.ElapsedMilliseconds);

                sw.Restart();
                ArrayBuilderBasedOnList<char> abbl1 = new ArrayBuilderBasedOnList<char>();
                for (int i = 0; i < LoopCount; i++) {
                    if ((i % 100) == 0) {
                        abbl1.Clear();
                    }
                    abbl1.Concat(array);
                    if ((i % 10) == 0) {
                        rl = abbl1.ToReadOnlyList();
                    }
                }
                sw.Stop();
                Console.WriteLine("ArrayBuilderBasedOnList1 : {0} (allocate on output only)", sw.ElapsedMilliseconds);

                sw.Restart();
                ArrayBuilderBasedOnList<char> abbl2 = new ArrayBuilderBasedOnList<char>();
                for (int i = 0; i < LoopCount; i++) {
                    if ((i % 100) == 0) {
                        abbl2.Clear();
                    }
                    abbl2.Concat(array);
                    rl = abbl2.ToReadOnlyList();
                    if ((i % 10) == 0) {
                    }
                }
                sw.Stop();
                Console.WriteLine("ArrayBuilderBasedOnList2 : {0} (allocate on every concat)", sw.ElapsedMilliseconds);

                sw.Restart();
                ArrayBuilderBasedOnArray<char> abba1 = new ArrayBuilderBasedOnArray<char>();
                for (int i = 0; i < LoopCount; i++) {
                    if ((i % 100) == 0) {
                        abba1.Clear();
                    }
                    abba1.Concat(array);
                    if ((i % 10) == 0) {
                        rl = abba1.ToReadOnlyList();
                    }
                }
                sw.Stop();
                Console.WriteLine("ArrayBuilderBasedOnArray1 : {0} (allocate on output only)", sw.ElapsedMilliseconds);

                sw.Restart();
                ArrayBuilderBasedOnArray<char> abba2 = new ArrayBuilderBasedOnArray<char>();
                for (int i = 0; i < LoopCount; i++) {
                    if ((i % 100) == 0) {
                        abba2.Clear();
                    }
                    abba2.Concat(array);
                    rl = abba2.ToReadOnlyList();
                    if ((i % 10) == 0) {
                    }
                }
                sw.Stop();
                Console.WriteLine("ArrayBuilderBasedOnArray2 : {0} (allocate on every concat)", sw.ElapsedMilliseconds);

                sw.Restart();
                StringBuilder sb1 = new StringBuilder();
                for (int i = 0; i < LoopCount; i++) {
                    if ((i % 100) == 0) {
                        sb1.Clear();
                    }
                    sb1.Append(array);
                    if ((i % 10) == 0) {
                        s = sb1.ToString();
                    }
                }
                sw.Stop();
                Console.WriteLine("StringBuilder1 : {0} (allocate on output only)", sw.ElapsedMilliseconds);

                sw.Restart();
                StringBuilder sb2 = new StringBuilder();
                for (int i = 0; i < LoopCount; i++) {
                    if ((i % 100) == 0) {
                        sb2.Clear();
                    }
                    sb2.Append(array);
                    s = sb2.ToString();
                    if ((i % 10) == 0) {
                    }
                }
                sw.Stop();
                Console.WriteLine("StringBuilder2 : {0} (allocate on every concat)", sw.ElapsedMilliseconds);
                Console.WriteLine("====================================");
            }
        }
    }

    /// <summary>
    /// utility for improving performance on frequent one-dimensional array 
    /// concatenation. Order of elements will be the same as concatenation order.
    /// </summary>
    public class ArrayBuilderBasedOnArray<ElementType> : IDisposable
    {
        /// <summary> unit test. </summary>
        internal static void Main() {
            IReadOnlyList<int> result;
            int[] array = new int[] { 1, 2, 3, 4 };
            ArrayBuilderBasedOnArray<int> arrayBuilder = new ArrayBuilderBasedOnArray<int>();

            for (int k = 0; k < 2; k++) {
                Console.WriteLine("concat null:");
                arrayBuilder.Concat(null);
                result = arrayBuilder.ToReadOnlyList();
                foreach (int item in result) {
                    Console.Write("{0}, ", item);
                }
                Console.WriteLine();

                Console.WriteLine("concat empty array:");
                arrayBuilder.Concat(new int[0]);
                result = arrayBuilder.ToReadOnlyList();
                foreach (int item in result) {
                    Console.Write("{0}, ", item);
                }
                Console.WriteLine();

                Console.WriteLine("concat multiple arrays over capacity:");
                for (int i = 0; i < 10; i++) {
                    arrayBuilder.Concat(array);
                }
                result = arrayBuilder.ToReadOnlyList();
                foreach (int item in result) {
                    Console.Write("{0}, ", item);
                }
                Console.WriteLine();
            }
        }

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
        #endregion

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
                // UPDATE[9]: if the length if small, expand the buffer and copy it?
                arrays.Add(array);
            }
        }

        /// <summary>
        /// get concatenated arrays.
        /// </summary>
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

            // UPDATE[9]: if insertion will be support, please consider removing this 2 lines of code
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
        /// release all resources. <para />
        /// the object must not be used after calling it. 
        /// </summary>
        public void Dispose() {
            arrays = null;
            concatBuffer = null;
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
        #endregion

        #region Property
        #endregion

        #region Type
        #endregion

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
        #endregion

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
        #endregion
    }

    /// <summary>
    /// utility for improving performance on frequent one-dimensional array 
    /// concatenation. Order of elements will be the same as concatenation order.
    /// </summary>
    public class ArrayBuilderBasedOnList<ElementType> : IDisposable
    {
        /// <summary> unit test. </summary>
        internal static void Main() {
            IReadOnlyList<int> result;
            int[] array = new int[] { 1, 2, 3, 4 };
            ArrayBuilderBasedOnList<int> arrayBuilder = new ArrayBuilderBasedOnList<int>();

            for (int k = 0; k < 2; k++) {
                Console.WriteLine("concat null:");
                arrayBuilder.Concat(null);
                result = arrayBuilder.ToReadOnlyList();
                foreach (int item in result) {
                    Console.Write("{0}, ", item);
                }
                Console.WriteLine();

                Console.WriteLine("concat empty array:");
                arrayBuilder.Concat(new int[0]);
                result = arrayBuilder.ToReadOnlyList();
                foreach (int item in result) {
                    Console.Write("{0}, ", item);
                }
                Console.WriteLine();

                Console.WriteLine("concat multiple arrays over capacity:");
                for (int i = 0; i < 10; i++) {
                    arrayBuilder.Concat(array);
                }
                result = arrayBuilder.ToReadOnlyList();
                foreach (int item in result) {
                    Console.Write("{0}, ", item);
                }
                Console.WriteLine();
            }
        }

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
        #endregion

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
                // UPDATE[9]: if the length if small, expand the buffer and copy it?
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

            // UPDATE[9]: if insertion will be support, please consider removing this 2 lines of code
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

        /// <summary> 
        /// release all resources. <para />
        /// the object must not be used after calling it. 
        /// </summary>
        public void Dispose() {
            arrays = null;
            concatBuffer = null;
        }
        #endregion

        #region Property
        #endregion

        #region Type
        #endregion

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
        #endregion

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
        #endregion
    }

    /// <summary>
    /// utility for improving performance on frequent one-dimensional array 
    /// concatenation. Order of elements will be the same as concatenation order.
    /// </summary>
    [Obsolete("just a code example for MultiDimensionalArrayBuilder.", true)]
    public class ArrayBuilder : IDisposable
    {
        /// <summary> unit test. </summary>
        internal static void Main() {
            int[] array = new int[] { 1, 2, 3, 4 };
            ArrayBuilder arrayBuilder = new ArrayBuilder();
            for (int i = 0; i < 10; i++) {
                arrayBuilder.Concat(array);
            }
            int[] rl = (int[])arrayBuilder.ToArray();
            foreach (var item in rl) {
                Console.WriteLine(item);
            }
        }

        #region Constructor
        /// <summary> initialize buffer. </summary>
        /// <param name="array"> first array to fill the buffer. </param>
        /// <param name="elementType"> 
        /// specify element type if all arrays to be concatenated may be null. 
        /// </param>
        /// <param name="concatCount"> possible number of invoking Concat(). </param>
        /// <param name="bufferCapacity"> possible number of elements. </param>
        public ArrayBuilder(Array array = null, Type elementType = null,
            int concatCount = InitArrayNum, int bufferCapacity = InitBufferCapacity) {
            this.arrays = new List<Array>(concatCount);
            this.concatBuffer = Array.CreateInstance(
                elementType ?? array.GetType().GetElementType(), bufferCapacity);
            this.elementType = elementType;
            this.concatedArrayNum = 0;
            this.concatedElementNum = 0;

            Concat(array);
        }
        #endregion

        #region Method
        public void Concat(Array array) {
            if (array == null) { return; }

            if (concatedElementNum + array.Length < concatBuffer.Length) {
                array.CopyTo(concatBuffer, concatedElementNum);
                concatedElementNum += array.Length;
            } else {
                arrays.Add(array);
            }
        }

        /// <summary>
        /// concatenate arrays in the buffer.
        /// </summary>
        /// <returns>
        /// if all arrays in the buffer is null or 
        /// </returns>
        public Array ToArray() {
            if (arrays.Count == 0) { return Array.CreateInstance(ElementType, 0); }

            for (int i = concatedArrayNum; i < arrays.Count; ++i) {
                concatedElementNum += arrays[i].Length;
            }

            if (concatBuffer == null) {
                concatBuffer = Array.CreateInstance(ElementType, AdjustLength());
            } else if (concatBuffer.Length < concatedElementNum) {
                Array oldBuffer = concatBuffer;
                concatBuffer = Array.CreateInstance(ElementType, AdjustLength());

            } else {

            }

            return concatBuffer;
        }

        /// <summary> 
        /// reset state of the buffer to allow new types of array concatenation. <para />
        /// it will remove all buffered elements but keep the resources. 
        /// </summary>
        /// <param name="newElementType"> 
        /// specify element type if all arrays to be concatenated may be null. 
        /// </param>
        public void Clear(Type newElementType = null) {
            arrays.Clear();
            elementType = newElementType;
            concatedArrayNum = 0;
            concatedElementNum = 0;
        }

        /// <summary> 
        /// release all resources. <para />
        /// the object must not be used after calling it. 
        /// </summary>
        public void Dispose() {
            arrays.Clear();
            arrays.Capacity = 0;
            concatBuffer = null;
            elementType = null;
        }

        private int AdjustLength() {
            int newLength = 0;
            return newLength;
        }
        #endregion

        #region Property
        public Type ElementType {
            get { return elementType ?? (elementType = arrays[0].GetType().GetElementType()); }
        }
        #endregion

        #region Type
        #endregion

        #region Constant
        /// <summary> the initial capacity of $arrays. </summary>
        public const int InitArrayNum = 4;
        /// <summary> the length of concatBuffer on the first call of ToReadOnlyList(). </summary>
        public const int InitBufferCapacity = 16;
        #endregion

        #region Field
        /// <summary> hold incoming arrays. </summary>
        private List<Array> arrays;
        /// <summary> hold the concatenated array. </summary>
        private Array concatBuffer;
        /// <summary> 
        /// number of arrays in $arrays which has been concatenated to concatBuffer. 
        /// it is also the index of next array to be concatenated in $arrays.
        /// </summary>
        private int concatedArrayNum;
        /// <summary> capacity of concatBuffer. </summary>
        private int concatedElementNum;
        /// <summary> element type of the first array. </summary>
        private Type elementType;
        #endregion
    }

    /// <summary>
    /// utility for improving performance on frequent array 
    /// concatenation. Order of elements will be the same as concatenation order.
    /// </summary>
    /// <remarks> 
    /// if you are sure the incoming array has only one dimension, 
    /// use ArrayBuilderBasedOnArray<ElementType> for better performance.
    /// </remarks>
    // UNDONE[9]: use Array.Copy() to support multidimensional arrays:
    //       Array.Copy(array, 0, concatBuffer, concatBuffer.Length, array.Length);
    //       array.CopyTo() does not support this feature.
    [Obsolete("not finished.")]
    public class MultiDimensionalArrayBuilder : IDisposable
    {
        /// <summary> unit test. </summary>
        internal static void Main() {
            int[,] array = new int[,] { { 1, 2 }, { 3, 4 } };
            MultiDimensionalArrayBuilder arrayBuilder = new MultiDimensionalArrayBuilder();
            for (int i = 0; i < 10; i++) {
                arrayBuilder.Concat(array);
            }
            int[,] r = (int[,])arrayBuilder.ToArray();
            foreach (var item in r) {
                Console.WriteLine(item);
            }
        }

        #region Constructor
        public MultiDimensionalArrayBuilder() { Clear(); }
        public MultiDimensionalArrayBuilder(Array array)
            : this() { Concat(array); }
        #endregion

        #region Method
        public void Concat(Array array) {
            if (array != null) { arrays.Add(array); }
        }

        public Array ToArray() {
            if (arrays.Count == 0) { return null; }

            for (int i = concatedArrayNum; i < arrays.Count; i++) {
                concatedElementNum += arrays[i].Length;
            }

            if (concatBuffer == null) {
                concatBuffer = Array.CreateInstance(ElementType, AdjustLengths());
            } else if (concatBuffer.Length < concatedElementNum) {
                Array oldBuffer = concatBuffer;
                concatBuffer = Array.CreateInstance(ElementType, AdjustLengths());

            } else {

            }

            return concatBuffer;
        }

        /// <summary> remove all elements but keep resources. </summary>
        public void Clear() {
            arrays.Clear();
            lengths = null;
            elementType = null;
            concatedArrayNum = 0;
            concatedElementNum = 0;
        }

        /// <summary> 
        /// release all resources. 
        /// the object must not be used after calling it. 
        /// </summary>
        public void Dispose() {
            arrays.Clear();
            arrays.Capacity = 0;
            concatBuffer = null;
            lengths = null;
            elementType = null;
            concatedArrayNum = 0;
            concatedElementNum = 0;
        }

        [Obsolete("not finished.")]
        private int[] AdjustLengths() {
            return Lengths;
        }
        #endregion

        #region Property
        public Type ElementType {
            get { return elementType ?? (elementType = arrays[0].GetType().GetElementType()); }
        }

        public int[] Lengths {
            get { return lengths ?? (lengths = arrays[0].GetLengths()); }
        }
        #endregion

        #region Type
        #endregion

        #region Constant
        /// <summary> the initial capacity of $arrays. </summary>
        public const int InitArrayNum = 4;
        /// <summary> the length of concatBuffer on the first call of ToReadOnlyList(). </summary>
        public const int InitBufferCapacity = 16;
        #endregion

        #region Field
        /// <summary> hold incoming arrays. </summary>
        private List<Array> arrays = new List<Array>(InitArrayNum);
        /// <summary> hold the concatenated array. </summary>
        private Array concatBuffer = null;
        /// <summary> 
        /// number of arrays in $arrays which has been concatenated to concatBuffer. 
        /// it is also the index of next array to be concatenated in $arrays.
        /// </summary>
        private int concatedArrayNum;
        /// <summary> capacity of concatBuffer. </summary>
        private int concatedElementNum;
        /// <summary> lengths of all dimensions. </summary>
        private int[] lengths;
        /// <summary> element type of the first array. </summary>
        private Type elementType;
        #endregion
    }
}
