using System;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace IDeal.Szx.CsharpUtilibs.Sys.Concurrency {
    public static class Signal {
        public enum Type : uint {
            Interrupt = CtrlTypes.CTRL_C_EVENT,
            Break = CtrlTypes.CTRL_BREAK_EVENT,
        }

        //public static bool Send(Process p, int msTimeout = 0) {
        //    SetConsoleCtrlHandler(null, true);
        //    Processes.run("SendSignal", $"{p.Id}");
        //    bool r = p.WaitForExit(msTimeout);
        //    SetConsoleCtrlHandler(null, false);
        //    return r;
        //}
        //public static bool Send(Process p, int msTimeout, Type signal) {
        //    SetConsoleCtrlHandler(null, true);
        //    Processes.run("SendSignal", $"{p.Id} {(uint)signal}");
        //    bool r = p.WaitForExit(msTimeout);
        //    SetConsoleCtrlHandler(null, false);
        //    return r;
        //}


        [Obsolete("It will interrupt the invoker's process!", true)]
        public static void Send(Process proc, int msTimeout = 0, Type signal = Type.Interrupt) {
            FreeConsole();
            if (AttachConsole((uint)proc.Id)) { // this does not require the console window to be visible.
                SetConsoleCtrlHandler(null, true); // disable Ctrl-C handling for ourselves.

                GenerateConsoleCtrlEvent((uint)signal, 0);
                FreeConsole(); // avoid terminating ourselves if `proc` is killed by others.
                proc.WaitForExit(msTimeout); // avoid terminating ourselves.

                SetConsoleCtrlHandler(null, false); // re-enable Ctrl-C handling or any subsequently started programs will inherit the disabled state.
            }
        }


        private enum CtrlTypes : uint {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT,
        }

        private delegate bool ConsoleCtrlDelegate(uint CtrlType);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AttachConsole(uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool Add);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, uint dwProcessGroupId);
    }
}
