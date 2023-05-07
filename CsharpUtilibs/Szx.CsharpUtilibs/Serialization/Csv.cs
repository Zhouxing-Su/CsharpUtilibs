using System.IO;
using System.Text;


namespace IDeal.Szx.CsharpUtilibs.Serialization {
    public static class Csv {
        public static void Save(string filename, string[,] table, char delim) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < table.GetLength(0); ++i) {
                sb.Append($"{table[i, 0]}");
                for (int j = 1; j < table.GetLength(1); ++j) {
                    sb.Append($"{delim}{table[i, j]}");
                }
                sb.AppendLine();
            }
            Text.WriteText(filename, sb.ToString());
        }

        public static string[][] Load(string filename, char delim) {
            if (!File.Exists(filename)) { return null; }
            string[] lines = Text.ReadLines(filename);
            if (lines.Length == 0) { return null; }
            string[][] table = new string[lines.Length][];
            for (int i = 0; i < lines.Length; ++i) { table[i] = lines[i].Split(delim); }
            return table;
        }

    }
}
