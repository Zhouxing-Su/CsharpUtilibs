using System;
using System.Threading;


namespace IDeal.Szx.CsharpUtilibs.Sys.Concurrency {
    public class Listener {
        public static void waitTerminationCodeAsync(string code) {
            Thread waitTermination = new Thread(() => {
                while (Console.ReadLine() != code) { }
                Environment.Exit(0);
            });
            waitTermination.IsBackground = true;
            waitTermination.Start();
        }

        public static void waitTerminationCode(string code) {
            Thread.CurrentThread.IsBackground = true;
            while (Console.ReadLine() != code) { }
            Environment.Exit(0);
        }
    }
}
