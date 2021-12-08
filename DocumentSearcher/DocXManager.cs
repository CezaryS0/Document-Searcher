using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xceed.Document.NET;
using Xceed.Words.NET;
namespace Japanese_Helper
{
    public static class DocXManager
    {
        private static readonly object Lock = new object();
        public static List<string> SearchDocX(string file, string keyword)
        {
            List<string> foundPhrases = new List<string>();
            try
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
                        if (Tools.CompareStrings(onerow, keyword))
                        {
                            lock (Lock)
                            {
                                foundPhrases.Add(onerow);
                            }
                        }
                    }
                });
                Parallel.ForEach(document.Paragraphs, p =>
                {
                    if (p.ParentContainer == ContainerType.Body)
                    {
                        if (Tools.CompareStrings(p.Text, keyword))
                        {
                            lock (Lock)
                            {
                                foundPhrases.Add(p.Text);
                            }
                        }
                    }
                });
                document.Dispose();
            }
            catch
            {
               
            }
           
            return foundPhrases;
        }
        public static void SaveToDocx(List<string> foundPhrases, string filename)
        {
            var document = DocX.Create(filename);
            Formatting formatting = new Formatting
            {
                FontFamily = new Font(Config.font.Name),
                Size = Convert.ToInt32(Config.font.Size)
            };
            foreach (var elem in foundPhrases)
            {
                document.InsertParagraph(elem, false, formatting);
            }
            document.Save();
            document.Dispose();
        }
    }
}
