namespace Japanese_Helper
{
    public static class Tools
    {
        public static bool CompareStrings(string w1, string w2)
        {
            if (w1.Contains('’'))
            {
                string changed = w1.Replace('’', '\'');
                if (changed.Contains(FirstUppercase(w2)) || changed.Contains(FirstLowerCase(w2)))
                {
                    return true;
                }
            }
            return w1.Contains(FirstUppercase(w2)) || w1.Contains(FirstLowerCase(w2));
        }
        private static string FirstUppercase(string keyword)
        {
            return char.ToUpper(keyword[0]) + keyword.Substring(1);
        }
        private static string FirstLowerCase(string keyword)
        {
            return char.ToLower(keyword[0]) + keyword.Substring(1);
        }
    }
}
