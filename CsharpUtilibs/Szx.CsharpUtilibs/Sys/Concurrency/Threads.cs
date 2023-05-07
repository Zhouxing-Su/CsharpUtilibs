using System.Threading;


namespace IDeal.Szx.CsharpUtilibs.Sys.Concurrency {
    public static class Threads {
        public delegate bool IsTaskTaken();
        public delegate void UserTask(IsTaskTaken isTaskTaken);

        /// <summary> workers scramble for tasks. </summary>
        /// <remarks>call `isTaskTaken` in `userTask` to avoid repeating.</remarks>
        /// <example>
        /// HashSet<int> s = new HashSet<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        /// Util.TaskTaker.run(4, (isTaskTaken) => {
        ///     foreach (var i in s) {
        ///         if (isTaskTaken()) { continue; }
        ///         Console.WriteLine(i);
        ///         Thread.Sleep(2000);
        ///     }
        /// });
        /// </example>
        public static void FightForTasks(int workerNum, UserTask userTask) {
            Thread[] workers = new Thread[workerNum];
            int gt = 0; // global task index.
            for (int w = 0; w < workerNum; ++w) {
                workers[w] = new Thread(() => {
                    int t = 0; // local task index.
                    userTask(() => {
                        if (t < gt) { ++t; return true; }
                        int ogt = gt; // old global task index.
                        return ogt != Interlocked.CompareExchange(ref gt, ogt + 1, t++);
                    });
                });
                workers[w].Start();
            }
            for (int w = 0; w < workerNum; ++w) { workers[w].Join(); }
        }
    }
}
