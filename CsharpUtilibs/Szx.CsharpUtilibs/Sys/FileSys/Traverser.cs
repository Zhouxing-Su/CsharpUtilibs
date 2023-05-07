using System.Collections.Generic;
using System.IO;


namespace IDeal.Szx.CsharpUtilibs.Sys.FileSys {
    public static class Traverser {
        public delegate void ManipulateFile(FileInfo file);
        public delegate bool ManipulateDir(DirectoryInfo dir); // return false if there is no need to look into it.


        public static void TraverseDirectory(string dirPath, ManipulateFile fileOp, ManipulateDir dirOp) {
            DirectoryInfo root = new DirectoryInfo(dirPath);
            TraverseDirectory(root, fileOp, dirOp);
        }

        public static void TraverseDirectory(DirectoryInfo root, ManipulateFile fileOp, ManipulateDir dirOp) {
            foreach (var dir in root.GetDirectories()) {
                if (dirOp(dir)) { TraverseDirectory(dir.FullName, fileOp, dirOp); }
            }
            foreach (var file in root.GetFiles()) { fileOp(file); }
        }

        public static List<string> ListFiles(string dir, bool recursive = false) {
            List<string> fileList = new List<string>();
            TraverseDirectory(dir, (f) => {
                fileList.Add(f.Name);
            }, (d) => { return recursive; });
            return fileList;
        }

        public static void MoveFile(string sourceFileName, string destFileName) {
            if (!File.Exists(sourceFileName)) { return; }
            if (File.Exists(destFileName)) { File.Delete(destFileName); }
            File.Move(sourceFileName, destFileName);
        }
    }
}
