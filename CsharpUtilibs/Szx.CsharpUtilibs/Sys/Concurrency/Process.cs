using System;
using System.Diagnostics;


namespace IDeal.Szx.CsharpUtilibs.Sys.Concurrency {
    public static class Processes {
        // [Blocking][NoWindow][InterceptOutput]
        public static string runRead(string fileName, string arguments = "") {
            ProcessStartInfo psi = new ProcessStartInfo(fileName, arguments);
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            using (Process p = Process.Start(psi)) {
                p.WaitForExit();
                return p.StandardOutput.ReadToEnd();
            }
        }
        // [NonBlocking][NoWindow]
        public static Process runAsync(string fileName, string arguments = "") {
            ProcessStartInfo psi = new ProcessStartInfo(fileName, arguments);
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            return Process.Start(psi);
        }
        // [Blocking][NoWindow][GetExitCode]
        public static int run(string fileName, string arguments = "") {
            using (Process p = runAsync(fileName, arguments)) {
                p.WaitForExit();
                return p.ExitCode;
            }
        }
        // [Blocking][ShowWindow]
        public static Process runUI(string fileName, string arguments = "") {
            Process p = Process.Start(fileName, arguments);
            p.WaitForExit();
            return p;
        }
        // [NonBlocking][ShowWindow]
        public static Process runUIAsync(string fileName, string arguments = "") {
            return Process.Start(fileName, arguments);
        }
    }
}
