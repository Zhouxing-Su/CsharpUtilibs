using System;


namespace IDeal.Szx.CsharpUtilibs.Sys {
    public static class Chrono {
        // https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
        public static string CompactDateTime(DateTime dt) { return dt.ToString("yyyyMMddHHmmss"); }
        public static string CompactDateTime() { return CompactDateTime(DateTime.Now); }
        public static string FriendlyDateTime(DateTime dt) { return dt.ToString("yyyy-MM-dd HH:mm:ss.fff"); }
        public static string FriendlyDateTime() { return FriendlyDateTime(DateTime.Now); }
    }
}
