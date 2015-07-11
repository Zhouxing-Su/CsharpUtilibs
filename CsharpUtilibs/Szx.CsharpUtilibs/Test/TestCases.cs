using System.Collections;
using System.Collections.Generic;


namespace Szx.CsharpUtilibs.Test
{
    internal class C0
    {

    }

    internal class C1
    {
        string id = "C1";
        int[] a = new int[0];
        string[] s = new string[2] { null, "zz" };

        C0 z = new C0();
        C2[,] b = new C2[2, 2] { { new C2(), new C3() }, { new C2(), new C2() } };
        C3 c = new C3();
        static C4 d = new C4();
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
