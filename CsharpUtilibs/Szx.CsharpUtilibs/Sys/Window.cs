using System;
using System.Runtime.InteropServices;


namespace IDeal.Szx.CsharpUtilibs.Sys {
    public static class Window {
        #region Flash
        [Flags]
        public enum FalshType : uint {
            FlashStop = 0x0,

            FlashCaption = 0x1,
            FlashTray = 0x2, // flash the taskbar button.
            FlashAll = 0x3,

            FlashTimer = 0x4, // flash continuously, until the `FlashStop` flag is set.
            FlashTimerNoForeground = 0xC, // flash continuously until the window comes to the foreground.

            Default = FlashAll | FlashTimerNoForeground
        }

        public static bool Flash(IntPtr hWnd, FalshType type = FalshType.Default) {
            FLASHWINFO fInfo = new FLASHWINFO();
            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.hwnd = hWnd;
            fInfo.dwFlags = (uint)type;
            fInfo.uCount = UInt32.MaxValue;
            fInfo.dwTimeout = 0;
            return FlashWindowEx(ref fInfo);
        }
        public static bool Flash(FalshType type = FalshType.Default) {
            return Flash(ThisHwnd(), type);
        }

        public static IntPtr ThisHwnd() {
            string oldTitle = Console.Title;
            Console.Title += ":szx@" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            IntPtr hwnd = FindWindow(null, Console.Title);
            Console.Title = oldTitle;
            return hwnd;
        }


        private struct FLASHWINFO {
            /// <summary> struct size. </summary>
            public uint cbSize;
            /// <summary> window handle to flash. </summary>
            public IntPtr hwnd;
            /// <summary> flash type. </summary>
            public uint dwFlags;
            /// <summary> flash count. </summary>
            public uint uCount;
            /// <summary> blink rate in milliseconds. zero for default cursor blink rate. </summary>
            public uint dwTimeout;
        }

        [DllImport("user32.dll")]
        private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        [DllImport("user32.dll")]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        #endregion Flash

        #region ErrDialog
        public static void DisableWindowsErrorReportingDialog() {
            SetErrorMode(GetErrorMode() | ErrorModes.NoWerDialog);
        }

        [Flags]
        private enum ErrorModes : uint {
            SYSTEM_DEFAULT = 0x0,
            SEM_FAILCRITICALERRORS = 0x0001,
            SEM_NOALIGNMENTFAULTEXCEPT = 0x0004,
            SEM_NOGPFAULTERRORBOX = 0x0002,
            SEM_NOOPENFILEERRORBOX = 0x8000,

            NoWerDialog = SEM_NOGPFAULTERRORBOX | SEM_FAILCRITICALERRORS | SEM_NOOPENFILEERRORBOX,
        }

        [DllImport("kernel32.dll")]
        private static extern ErrorModes GetErrorMode();
        [DllImport("kernel32.dll")]
        private static extern ErrorModes SetErrorMode(ErrorModes mode);
        #endregion ErrDialog
    }
}
