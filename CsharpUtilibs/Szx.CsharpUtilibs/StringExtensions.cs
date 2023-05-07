//#define UseMimeKit

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
#if UseMimeKit
using MimeKit;
#endif


namespace IDeal.Szx.CsharpUtilibs {
    public static class StringExtensions {
        public static string toString(this MemoryStream ms) {
            ms.Position = 0;
            using (StreamReader sr = new StreamReader(ms)) { return sr.ReadToEnd(); }
        }

        public static string removeSuffix(this string str, string suffix) {
            return (str.EndsWith(suffix) ? str.Substring(0, str.Length - suffix.Length) : str);
        }

        public static string quote(this string str) { return "\"" + str + "\""; }

        public static string subStr(this string str, int beginIndex, int endIndex) {
            return str.Substring(beginIndex, endIndex - beginIndex);
        }
        public static string subStr(this string str, int beginIndex, char delim) {
            int endIndex = str.IndexOf(delim);
            return str.subStr(beginIndex, (endIndex > beginIndex) ? endIndex : str.Length);
        }


        public static void save(this Attachment attachment, string filePath) {
            using (FileStream fs = File.Create(filePath)) {
                attachment.ContentStream.CopyTo(fs);
            }
        }
        public static string toString(this Attachment attachment) {
            using (MemoryStream ms = new MemoryStream()) {
                attachment.ContentStream.CopyTo(ms);
                return ms.toString();
            }
        }

#if UseMimeKit
        public static MemoryStream toStream(this MimePart attachment) {
            MemoryStream ms = new MemoryStream();
            attachment.Content.DecodeTo(ms);
            ms.Position = 0;
            return ms;
        }
#endif

        public static string toHtmlTable(this string s) {
            StringBuilder sb = new StringBuilder("<table border=1>");
            bool emptyLine = true;
            foreach (var c in s) {
                if ((c == '\r') || (c == '\n')) {
                    if (!emptyLine) {
                        sb.Append("</td></tr>");
                        emptyLine = true;
                    }
                    continue;
                }
                if (emptyLine) { sb.Append("<tr><td>"); }
                if (c == '\t') {
                    sb.Append("</td><td>");
                    continue;
                }
                emptyLine = false;
                sb.Append(c);
            }
            if (!emptyLine) { sb.Append("</td></tr>"); }
            sb.Append("</table>");
            return sb.ToString();
        }

        public static string toSafeCsvStr(this string s) {
            if (s.Length <= 0) { return "?"; }
            return s.Replace(",", "").Replace("\"", "").Replace("\t", "").Replace("\n", "").Replace("\r", "");
        }
    }
}
