using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Szx.CsharpUtilibs.Threading
{
    public static class Worker
    {
        public static void WorkUntilTimeout(
            ThreadStart work,
            int millisecondsTimeout = Timeout.Infinite) {
            Thread thread = new Thread(work);
            thread.Start();
            thread.Join(millisecondsTimeout);
            thread.Abort();
        }

        public static void WorkUntilTimeout(
            ParameterizedThreadStart work, object workArg,
            int millisecondsTimeout = Timeout.Infinite) {
            Thread thread = new Thread(work);
            thread.Start(workArg);
            thread.Join(millisecondsTimeout);
            thread.Abort();
        }

        public static void WorkUntilTimeout(
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
            thread.Join(millisecondsTimeout);
            thread.Abort();
        }

        public static void WorkUntilTimeout(
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
            thread.Join(millisecondsTimeout);
            thread.Abort();
        }
    }
}
