using System.Collections.Generic;
using System.IO;

namespace Japanese_Helper
{
    public static class FileManager
    {
        private static readonly List<string> foundPhrases = new List<string>();
        public static List<string> FindPhrases(string path, string _keyword)
        {
            if (foundPhrases.Count > 0)
                foundPhrases.Clear();
            DirSearch(path, _keyword);
            return foundPhrases;
        }
        private static void InterpretDocXFile(string file, string keyword)
        {
            var list = DocXManager.SearchDocX(file, keyword);
            foreach (var elem in list)
            {
                foundPhrases.Add(elem);
            }
        }
        private static void InterpretTextFile(string file, string keyword)
        {
            var list = TXTManager.SearchTXTFile(file, keyword);
            foreach (var elem in list)
            {
                foundPhrases.Add(elem);
            }
        }
        private static void InterpretFile(string file, string keyword)
        {
            switch (Path.GetExtension(file))
            {
                case ".docx":
                    InterpretDocXFile(file, keyword);
                    break;
                case ".txt":
                case ".cpp":
                case ".c":
                case ".xml":
                case ".cfg":
                    InterpretTextFile(file, keyword);
                    break;
            }
        }
        private static void DirSearch(string path, string keyword)
        {
            foreach (string file in Directory.GetFiles(path))
            {
                InterpretFile(file, keyword);
            }

            foreach (string directory in Directory.GetDirectories(path))
            {
                DirSearch(directory, keyword);
            }
        }
    }
}
