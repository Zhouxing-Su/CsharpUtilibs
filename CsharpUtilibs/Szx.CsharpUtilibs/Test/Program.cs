using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;


#pragma warning disable 618


namespace IDeal.Szx.CsharpUtilibs.Test {
    using IDeal.Szx.CsharpUtilibs.Collections;
    using IDeal.Szx.CsharpUtilibs.Serialization;
    using IDeal.Szx.CsharpUtilibs.System;
    using IDeal.Szx.CsharpUtilibs.System.Threading;
    using IDeal.Szx.CsharpUtilibs.System.Network;
    using IDeal.Szx.CsharpUtilibs.System.File;
    using IDeal.Szx.CsharpUtilibs.Random;
    using IDeal.Szx.CsharpUtilibs.Geometry;


    internal class Program {
        private static void Main(string[] args) {
            //Collections.ArrayBuilderTest.Test();
            Collections.MultiDimArrayTest.Test();
            //Collections.ObjectSetTest.Test();
            //Collections.ReadOnlySetTest.Test();

            Serialization.SerializerTest.Test();
            //Serialization.JsonTest.Test();

            //System.ArgsProcessorTest.Test();
            //System.Threading.WorkerTest.Test();
            //System.Threading.ListenerTest.Test();
            //System.Threading.AtomicCounterTest.Test();
            //System.Network.IPTest.Test();
            //System.File.ConvertScaleTest.Test();

            //Random.SelectTest.Test();
            //Geometry.CircleTest.Test();
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

    namespace Collections {
        internal static class ArrayBuilderTest {
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

                    Console.WriteLine("concat empty data:");
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

                    Console.WriteLine("concat empty data:");
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
                    //    ra = ra.Concat(data).ToArray();
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

        internal static class ObjectSetTest {
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
                    if (objSet.Contains(obj)) { Console.WriteLine("bug!!!"); }
                    if (!objSet.Add(obj)) { Console.WriteLine("bug!!!"); }
                    if (!objSet.Contains(obj)) { Console.WriteLine("bug!!!"); }
                }
                sw.Stop();
                Console.WriteLine(sw.ElapsedMilliseconds);


                sw.Restart();
                objSet = new ObjectSetUsingObjectIDGenerator();
                for (int i = 0; i < 10000000; i++) {
                    object obj = new object();
                    if (objSet.Contains(obj)) { Console.WriteLine("bug!!!"); }
                    if (!objSet.Add(obj)) { Console.WriteLine("bug!!!"); }
                    if (!objSet.Contains(obj)) { Console.WriteLine("bug!!!"); }
                }
                sw.Stop();
                Console.WriteLine(sw.ElapsedMilliseconds);
            }

            internal static void TestPerformance() {
            }
        }

        internal static class MultiDimArrayTest {
            internal static void Test() {
                TestCorrectness();
                TestPerformance();
            }

            internal static void TestCorrectness() {
                MultiDimArray<int> a = new MultiDimArray<int>(2, 3, 4, 3, 2);
                for (int i = 0; i < a.Length; i++) {
                    a[i] = i;
                    Console.Write(a[i] + " ");
                }
                Console.WriteLine();
                Console.WriteLine();
                for (int i = 0; i < a.Lengths[0] * a.Lengths[1]; i++) {
                    for (int j = 0; j < a.Lengths[2]; j++) {
                        for (int k = 0; k < a.Lengths[3] * a.Lengths[4]; k++) {
                            a[i, j, k] = k;
                            Console.Write(a[i, j, k] + " ");
                        }
                    }
                }
                Console.WriteLine();
                Console.WriteLine();
                for (int i = 0; i < a.Lengths[0]; i++) {
                    for (int j = 0; j < a.Lengths[1]; j++) {
                        for (int k = 0; k < a.Lengths[2]; k++) {
                            for (int l = 0; l < a.Lengths[3]; l++) {
                                for (int m = 0; m < a.Lengths[4]; m++) {
                                    a[i, j, k, l, m] = m + l;
                                    Console.Write(a[i, j, k, l, m] + " ");
                                }
                            }
                        }
                    }
                }
            }

            internal static void TestPerformance() {
            }
        }

        internal static class ReadOnlySetTest {
            internal static void Test() {
                TestCorrectness();
                TestPerformance();
            }

            internal static void TestCorrectness() {
                ISet<int> set = new HashSet<int>() { 1, 2, 3, 4, 5 };

                ReadOnlySet<int> roset = new ReadOnlySet<int>(set);
                //roset.Add(6);     // should be error
                Console.WriteLine(roset.Contains(1));  // should be correct
                Console.WriteLine(roset.Count);
            }

            internal static void TestPerformance() {
            }
        }
    }

    namespace Serialization {
        internal static class SerializerTest {
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

        internal static class JsonTest {
            internal static void Test() {
                TestCorrectness();
                TestPerformance();
            }

            internal static void TestCorrectness() {
                C3 b = new C3();
                Json.save("test.json", b);
                b = Json.load<C3>("test.json");
            }

            internal static void TestPerformance() {
            }
        }
    }

    namespace System {
        internal static class ArgsProcessorTest {
            internal static void Test() {
                TestCorrectness();
                TestPerformance();
            }

            internal static void TestCorrectness() {
                ArgsProcessor ap = new ArgsProcessor();
                ap.Process(
                    new string[] { "cc", "-o", "test", "test.c", "-h", "-O3", "--verbose", "test.cc.log" },
                    new string[] { "-o", "--verbose" },
                    new string[] { "/h", "-h", "-O3" });

                foreach (var item in ap.PlainArgs) {
                    Console.Write(item + " ");
                }
                Console.WriteLine();
                foreach (var item in ap.MapArgs) {
                    Console.Write(item + " ");
                }
                Console.WriteLine();
                foreach (var item in ap.SwitchArgs) {
                    Console.Write(item + " ");
                }
                Console.WriteLine();
                Console.WriteLine();

                ap.Process(
                    new string[] { },
                    new string[] { "-o", "--verbose" });

                foreach (var item in ap.PlainArgs) {
                    Console.Write(item + " ");
                }
                Console.WriteLine();
                foreach (var item in ap.MapArgs) {
                    Console.Write(item + " ");
                }
                Console.WriteLine();
                foreach (var item in ap.SwitchArgs) {
                    Console.Write(item + " ");
                }
                Console.WriteLine();
                Console.WriteLine();

                ap.Process(null);
                foreach (var item in ap.PlainArgs) {
                    Console.Write(item + " ");
                }
                Console.WriteLine();
                foreach (var item in ap.MapArgs) {
                    Console.Write(item + " ");
                }
                Console.WriteLine();
                foreach (var item in ap.SwitchArgs) {
                    Console.Write(item + " ");
                }
                Console.WriteLine();
                Console.WriteLine();

                ap.Process(
                    new string[] { "cc", "-o", "test", "test.c", "-h", "-O3", "--verbose", "test.cc.log" },
                    switchs: new string[] { "/h", "-h", "-O3" });
                foreach (var item in ap.PlainArgs) {
                    Console.Write(item + " ");
                }
                Console.WriteLine();
                foreach (var item in ap.MapArgs) {
                    Console.Write(item + " ");
                }
                Console.WriteLine();
                foreach (var item in ap.SwitchArgs) {
                    Console.Write(item + " ");
                }
                Console.WriteLine();
                Console.WriteLine();
            }

            internal static void TestPerformance() {
            }
        }

        namespace Threading {
            internal static class WorkerTest {
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
                    Worker.WorkUntilTimeout(C0.f, () => { new C0().g(3); }, C0.h, 500);
                }

                internal static void TestPerformance() {
                }
            }

            internal static class ListenerTest {
                internal static void Test() {
                    TestCorrectness();
                    TestPerformance();
                }

                internal static void TestCorrectness() {
                    Listener.waitTerminationCodeAsync("async");
                    Console.WriteLine("waiting...");
                    Listener.waitTerminationCode("sync");
                }

                internal static void TestPerformance() {
                }
            }

            internal static class AtomicCounterTest {
                internal static void Test() {
                    TestCorrectness();
                    TestPerformance();
                }

                internal static void TestCorrectness() {
                    Console.WriteLine(GlobalAtomicCounter.next());
                    Console.WriteLine(GlobalAtomicCounter.next());

                    AtomicCounter ac = new AtomicCounter();
                    Console.WriteLine(ac.next());
                    Console.WriteLine(ac.next());
                }

                internal static void TestPerformance() {
                }
            }
        }

        namespace Network {
            internal static class IPTest {
                internal static void Test() {
                    TestCorrectness();
                    TestPerformance();
                }

                internal static void TestCorrectness() {
                    foreach (var ip in IP.GetLocalIPv4()) {
                        Console.WriteLine(ip);
                    }
                    foreach (var ip in IP.GetLocalIPv6()) {
                        Console.WriteLine(ip);
                    }
                }

                internal static void TestPerformance() {
                }
            }
        }

        namespace File {
            internal static class ConvertScaleTest {
                internal static void Test() {
                    TestCorrectness();
                    TestPerformance();
                }

                internal static void TestCorrectness() {
                    Console.WriteLine(ConvertScale.toProper(35234523));
                    Console.WriteLine(ConvertScale.toGB(4564754858));
                }

                internal static void TestPerformance() {
                }
            }
        }
    }

    namespace Random {
        internal static class SelectTest {
            internal static void Test() {
                TestCorrectness();
                TestPerformance();
            }

            internal static void TestCorrectness() {
                global::System.Random rand = new global::System.Random();

                int[] a = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                Select rs = new Select();
                int select = a[0];

                for (int i = 1; i < a.Length; ++i) {
                    if (rs.isSelected(rand.Next())) { select = a[i]; }
                }
                Console.WriteLine(select);

                rs.reset(0);
                select = -1;
                for (int i = 0; i < a.Length; ++i) {
                    if (rs.isSelected(rand.Next())) { select = a[i]; }
                }
                Console.WriteLine(select);
            }

            internal static void TestPerformance() {
            }
        }
    }

    namespace Geometry {
        internal static class CircleTest {
            internal static void Test() {
                TestCorrectness();
                TestPerformance();
            }

            internal static void TestCorrectness() {
                Circle c1 = new Circle(0, 0, 2);
                Circle c2 = new Circle(1, 1, 1);
                List<Point2D> points = Circle.intersectionPoints(c1, c2);
                foreach (var item in points) {
                    Console.WriteLine("(" + item.x + "," + item.y + ")");
                }
            }

            internal static void TestPerformance() {
            }
        }
    }
}
