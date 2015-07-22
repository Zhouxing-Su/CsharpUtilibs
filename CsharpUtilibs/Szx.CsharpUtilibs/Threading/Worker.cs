using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace IDeal.Szx.CsharpUtilibs.Threading
{
    public static class Worker
    {
        /// <summary> run method in synchronized way with a timeout. </summary>
        /// <returns> true if the work is finished within timeout. </returns>
        public static bool WorkUntilTimeout(
            ThreadStart work,
            int millisecondsTimeout = Timeout.Infinite) {
            Thread thread = new Thread(work);
            thread.Start();
            if (!thread.Join(millisecondsTimeout)) {
                thread.Abort();
                return false;
            }
            return true;
        }

        public static bool WorkUntilTimeout(
            ParameterizedThreadStart work, object workArg,
            int millisecondsTimeout = Timeout.Infinite) {
            Thread thread = new Thread(work);
            thread.Start(workArg);
            if (!thread.Join(millisecondsTimeout)) {
                thread.Abort();
                return false;
            }
            return true;
        }

        public static bool WorkUntilTimeout(
            Action work,
            Action onAbort,
            int millisecondsTimeout = Timeout.Infinite) {
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
            Action<object> work, object workArg,
            Action onAbort,
            int millisecondsTimeout = Timeout.Infinite) {
            Thread thread = new Thread((object obj) => {
                try {
                    work(obj);
                } catch (ThreadAbortException) {
                    onAbort();
                }
            });
            thread.Start(workArg);
            if (!thread.Join(millisecondsTimeout)) {
                thread.Abort();
                return false;
            }
            return true;
        }
    }
}
