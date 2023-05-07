using System;
using System.Threading;


namespace IDeal.Szx.CsharpUtilibs.Sys.Concurrency {
    public static class Worker {
        /// <summary> run method in synchronized way with a non-negative timeout. </summary>
        /// <returns> true if the work is finished within timeout. </returns>
        /// <remarks> 
        /// Timeout.Infinite is not supported (you should call your method directly). <para />
        /// use lambda expression to wrap the parameterized methods. 
        /// </remarks>
        public static bool WorkUntilTimeout(ThreadStart work,
            int millisecondsTimeout) {
            if (millisecondsTimeout < 0) { return false; }
            Thread thread = new Thread(work);
            thread.Start();
            if (!thread.Join(millisecondsTimeout)) {
                thread.Abort();
                return false;
            }
            return true;
        }

        public static bool WorkUntilTimeout(Action work, Action onAbort,
            int millisecondsTimeout) {
            if (millisecondsTimeout < 0) {
                onAbort();
                return false;
            }
            Thread thread = new Thread(() => {
                try {
                    work();
                } catch (ThreadAbortException) {
                    onAbort();
                }
            });
            thread.Start();
            if (!thread.Join(millisecondsTimeout)) {
                thread.Abort();
                return false;
            }
            return true;
        }

        public static bool WorkUntilTimeout(
            Action work, Action onAbort, Action onExit,
            int millisecondsTimeout) {
            if (millisecondsTimeout < 0) {
                onAbort();
                onExit();
                return false;
            }
            Thread thread = new Thread(() => {
                try {
                    work();
                } catch (ThreadAbortException) {
                    onAbort();
                } finally {
                    onExit();
                }
            });
            thread.Start();
            if (!thread.Join(millisecondsTimeout)) {
                thread.Abort();
                return false;
            }
            return true;
        }
    }
}
