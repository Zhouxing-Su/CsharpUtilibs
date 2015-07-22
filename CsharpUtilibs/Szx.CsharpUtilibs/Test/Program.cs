using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;


#pragma warning disable 618


namespace Szx.CsharpUtilibs.Test
{
    using Szx.CsharpUtilibs.Collections;
    using Szx.CsharpUtilibs.Serialization;
    using Szx.CsharpUtilibs.Threading;


    internal class Program
    {
        private static void Main(string[] args) {
            //Collections.ArrayBuilderTest.Test();
            //Collections.ObjectSetTest.Test();

            //Serialization.SerializerTest.Test();

            Threading.WorkerTest.Test();
        }
    }

    //internal static class Test
    //{
    //    internal static void Test() {
    //          TestCorrectness();
    //          TestPerformance();
    //    }

    //    internal static void TestCorrectness() {
    //    }

    //    internal static void TestPerformance() {
    //    }
    //}

    namespace Collections
    {
        internal static class ArrayBuilderTest
        {
            internal static void Test() {
                TestCorrectness();
                TestPerformance();
            }

            internal static void TestCorrectness() {
                int[] array = new int[] { 1, 2, 3, 4 };
                IArrayBuilder<int> arrayBuilder;
                IReadOnlyList<int> result;

                arrayBuilder = new ArrayBuilderBasedOnArray<int>();
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

                arrayBuilder = new ArrayBuilderBasedOnList<int>();
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

            /// <summary> benchmark result. </summary>
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
            internal static void TestPerformance() {
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

        internal static class ObjectSetTest
        {
            internal static void Test() {
                TestCorrectness();
                TestPerformance();
            }

            internal static void TestCorrectness() {
                Stopwatch sw = new Stopwatch();
                IObjectSet objSet;

                sw.Restart();
                objSet = new ObjectSetUsingConditionalWeakTable();
                for (int i = 0; i < 10000000; i++) {
                    object obj = new object();
                    if (objSet.IsExist(obj)) { Console.WriteLine("bug!!!"); }
                    if (!objSet.Add(obj)) { Console.WriteLine("bug!!!"); }
                    if (!objSet.IsExist(obj)) { Console.WriteLine("bug!!!"); }
                }
                sw.Stop();
                Console.WriteLine(sw.ElapsedMilliseconds);


                sw.Restart();
                objSet = new ObjectSetUsingObjectIDGenerator();
                for (int i = 0; i < 10000000; i++) {
                    object obj = new object();
                    if (objSet.IsExist(obj)) { Console.WriteLine("bug!!!"); }
                    if (!objSet.Add(obj)) { Console.WriteLine("bug!!!"); }
                    if (!objSet.IsExist(obj)) { Console.WriteLine("bug!!!"); }
                }
                sw.Stop();
                Console.WriteLine(sw.ElapsedMilliseconds);
            }

            internal static void TestPerformance() {
            }
        }
    }

    namespace Serialization
    {
        internal static class SerializerTest
        {
            internal static void Test() {
                TestCorrectness();
                TestPerformance();
            }

            internal static void TestCorrectness() {
                C1 a = new C1();
                new SerializerBase().Traverse(a);
            }

            internal static void TestPerformance() {
            }
        }
    }

    namespace Threading
    {
        internal static class WorkerTest
        {
            internal static void Test() {
                TestCorrectness();
                TestPerformance();
            }

            internal static void TestCorrectness() {
                Worker.WorkUntilTimeout(C0.f, 500);
                Thread.Sleep(2000);
                Console.WriteLine();
                Worker.WorkUntilTimeout(C0.f, C0.h, 500);
                Thread.Sleep(2000);
                Console.WriteLine();
                Worker.WorkUntilTimeout(new C0().g, 3, 500);
                Thread.Sleep(2000);
                Console.WriteLine();
                Worker.WorkUntilTimeout(new C0().g, 5, C0.h, 500);
            }

            internal static void TestPerformance() {
            }
        }
    }
}
