using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;


namespace Szx.CsharpUtilibs.Test
{
    internal class C0
    {
        public static void f() {
            Console.WriteLine("in f()");
            Thread.Sleep(100000);
            Console.WriteLine("out f()");
        }

        public void g(object obj) {
            Console.WriteLine("in g(" + obj + ")");
            Thread.Sleep(100000);
            Console.WriteLine("out g(" + obj + ")");
        }

        public static void h() {
            Console.WriteLine("in h()");
            Thread.Sleep(1000);
            Console.WriteLine("out h()");
        }
    }

    internal class C1
    {
        string id = "C1";
        int[] a = new int[0];
        string[] s = new string[2] { null, "zz" };

        C0 z = new C0();
        C2[,] b = new C2[2, 2] { { new C2(), c }, { new C2(), new C2() } };
        static C3 c = new C3();
        C4 d = new C4();
        C5 e = new C5();
        C6 f = new C6();

        public override string ToString() {
            return id.ToString();
        }
    }

    internal class C2
    {
        public C2() { Id = "C2"; }
        string Id { get; set; }

        public override string ToString() {
            return Id.ToString();
        }
    }

    internal class C3 : C2
    {
        string id = "C3";
        HashSet<char> h = new HashSet<char> { 's', 'z', 'x' };

        public override string ToString() {
            return id.ToString();
        }
    }

    internal class C4
    {
        string id = "C4";
        List<int> l = new List<int> { 1, 2, 3 };

        public override string ToString() {
            return id.ToString();
        }
    }

    internal class C5
    {
        string id = "C5";
        Dictionary<int, string> d = new Dictionary<int, string> { 
            {1, "szx"}, {3,"cyj"}, {2,"lzp"}, {100,"bwb"}
        };

        public override string ToString() {
            return id.ToString();
        }
    }

    internal class C6
    {
        string id = "C6";
        BitArray b = new BitArray(new bool[] { true, false, true });

        public override string ToString() {
            return id.ToString();
        }
    }
}
