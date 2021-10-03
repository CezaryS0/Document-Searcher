using System;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
namespace Japanese_Helper
{
    [Serializable]
    public class Data
    {
        public string Path;
        public string fontName;
        public float fontSize;
        public Data()
        {

        }
        public Data(string _Path, string _fontName, float _fontSize)
        {
            Path = _Path;
            fontName = _fontName;
            fontSize = _fontSize;
        }
    }
    public static class Config
    {
        public static string Path = "";
        public static Font font = null;
    }
    public static class Settings
    {
        private static readonly string configPath = "config/config.cfg";
        private static void CreateDirectories()
        {
            if (!Directory.Exists("config"))
                Directory.CreateDirectory("config");
        }
        public static void LoadPathFromFile()
        {
            if (File.Exists(configPath))
            {
                var serializer = new XmlSerializer(typeof(Data));
                using (var reader = new StreamReader(configPath))
                {
                    var data = (Data)serializer.Deserialize(reader);
                    Config.Path = data.Path;
                    Config.font = new Font(data.fontName, data.fontSize);
                }
            }

        }
        public static void SaveConfig()
        {
            CreateDirectories();
            var data = new Data(Config.Path, Config.font.Name, Config.font.Size);
            var serializer = new XmlSerializer(typeof(Data));
            using (var writer = new StreamWriter(configPath, false))
            {
                serializer.Serialize(writer, data);

            }
        }
    }
}
