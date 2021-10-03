using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xceed.Document.NET;
using Xceed.Words.NET;
namespace Japanese_Helper
{
    public static class DocXManager
    {
        private static string keyword = "";
        private static readonly List<string> FoundSentences = new List<string>();
        private static readonly object Lock = new object();
        private static void SearchDocX(string file)
        {
            var document = DocX.Load(file);
            Parallel.ForEach(document.Tables, table =>
            {
                foreach (var row in table.Rows)
                {
                    string onerow = "";
                    foreach (var p in row.Paragraphs)
                    {
                        if (p.Text != "")
                            onerow += '|' + p.Text;
                    }
                    if (onerow.Contains(keyword))
                    {
                        lock (Lock)
                        {
                            FoundSentences.Add(onerow);
                        }
                    }
                }
            });
            Parallel.ForEach(document.Paragraphs, p =>
            {
                if (p.ParentContainer == ContainerType.Body)
                {
                    if (p.Text.Contains(keyword))
                    {
                        lock (Lock)
                        {
                            FoundSentences.Add(p.Text);
                        }
                    }
                }
            });
        }
        private static void DirSearch(string path)
        {
            foreach (string file in Directory.GetFiles(path, "*.docx"))
            {
                SearchDocX(file);
            }

            foreach (string directory in Directory.GetDirectories(path))
            {
                DirSearch(directory);
            }
        }
        public static List<string> FindSentences(string path, string _keyword)
        {
            keyword = _keyword;
            if (FoundSentences.Count > 0)
                FoundSentences.Clear();
            DirSearch(path);
            return FoundSentences;
        }
        public static void SaveToDocx(string filename)
        {
            var document = DocX.Create(filename);
            Formatting formatting = new Formatting
            {
                FontFamily = new Font(Config.font.Name),
                Size = Convert.ToInt32(Config.font.Size)
            };
            foreach (var elem in FoundSentences)
            {
                document.InsertParagraph(elem, false, formatting);
            }
            document.Save();
        }
    }
}
