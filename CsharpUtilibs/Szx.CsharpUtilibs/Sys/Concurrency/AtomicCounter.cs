using System.Threading;


namespace IDeal.Szx.CsharpUtilibs.Sys.Concurrency {
    public static class GlobalAtomicCounter {
        public static int next() { return Interlocked.Increment(ref globalId); }

        private static int globalId = -1;
    }

    public class AtomicCounter {
        public long next() { return Interlocked.Increment(ref id); }

        private long id = -1;
    }
}
