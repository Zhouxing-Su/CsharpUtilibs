using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


namespace IDeal.Szx.CsharpUtilibs.Serialization {
    public static class Text {
        static readonly Encoding DefaultEncoding = Encoding.UTF8;

        public static Encoding GetEncoding(int codePage = 936) {
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding(codePage);
        }

        public static string ReadText(string path) {
            if (!File.Exists(path)) { return ""; }
            return File.ReadAllText(path, DefaultEncoding);
        }
        public static string[] ReadLines(string path) {
            if (!File.Exists(path)) { return new string[0]; }
            return File.ReadAllLines(path, DefaultEncoding);
        }
        public static void AppendText(string path, string contents) {
            File.AppendAllText(path, contents, DefaultEncoding);
        }
        public static void AppendLine(string path, string contents) {
            File.AppendAllText(path, contents + Environment.NewLine, DefaultEncoding);
        }
        public static void AppendLines(string path, IEnumerable<string> contents) {
            File.AppendAllLines(path, contents, DefaultEncoding);
        }
        public static void WriteText(string path, string contents) {
            File.WriteAllText(path, contents, DefaultEncoding);
        }
        public static void WriteLine(string path, string contents) {
            File.WriteAllText(path, contents + Environment.NewLine, DefaultEncoding);
        }
        public static void WriteLines(string path, IEnumerable<string> contents) {
            File.WriteAllLines(path, contents, DefaultEncoding);
        }

        //public static async void AppendAll(this StringBuilder sb, StreamReader sr, int msTimeout, int bufSize = 4096) { // non-blocking version.
        //    for (char[] buf = new char[bufSize]; ;) {
        //        var n = sr.ReadAsync(buf, 0, buf.Length);
        //        if (!n.Wait(msTimeout)) { break; }
        //        sb.Append(new Span<char>(buf, 0, await n));
        //    }
        //}
        public static void AppendAll(this StringBuilder sb, StreamReader sr, int msTimeout, int bufSize = 4096) { // non-blocking version.
            ThreadPool.QueueUserWorkItem(o => {
                try {
                    for (int c = 0; (c = sr.Read()) != -1; sb.Append((char)c)) { }
                } catch (Exception) { }
            });
        }
        public static void AppendAll(this StringBuilder sb, StreamReader sr) { // blocking version.
            try {
                for (int c = 0; (c = sr.Read()) != -1; sb.Append((char)c)) { }
            } catch (Exception) { }
        }
    }
}
