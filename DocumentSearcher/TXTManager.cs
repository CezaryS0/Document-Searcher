using System.Collections.Generic;
using System.IO;

namespace Japanese_Helper
{
    public static class TXTManager
    {
        public static List<string> SearchTXTFile(string file, string keyword)
        {
            List<string> foundPhrases = new List<string>();
            using (var txt = new StreamReader(file))
            {
                string line = null;
                while ((line = txt.ReadLine()) != null)
                {
                    if (Tools.CompareStrings(line, keyword))
                    {
                        foundPhrases.Add(line);
                    }
                }
            }
            return foundPhrases;
        }
        public static void SaveToTXT(List<string> list, string file)
        {
            using (var txt = new StreamWriter(file, false))
            {
                foreach (var elem in list)
                {
                    txt.WriteLine(elem);
                }
            }
        }
    }
}
