using System;


namespace IDeal.Szx.CsharpUtilibs.Sys {
    static class Log {
        public static void print(string msg) { // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated
            Console.WriteLine($"{Chrono.FriendlyDateTime()} {msg}");
        }
    }
}
