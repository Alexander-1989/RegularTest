using System;
using System.IO;
using System.Drawing;
using System.Xml.Serialization;

namespace RegularTest.Service.Serializer
{
    [Serializable]
    public class Properties
    {
        public Point Location { get; set; } = new Point();
        public string Text { get; set; } = string.Empty;
        public string[] Items { get; set; } = new string[0];
    }

    internal class Config
    {
        private const string defaultName = "Config.xml";
        private readonly string fileName = null;
        private readonly XmlSerializer serializer = new XmlSerializer(typeof(Properties));
        public Properties Properties { get; set; } = new Properties();

        public Config()
        {
            fileName = Path.Combine(Environment.CurrentDirectory, defaultName);
        }

        public Config(string fileName)
        {
            this.fileName = Path.GetFullPath(fileName);
        }

        public bool FileExist
        {
            get
            {
                return File.Exists(fileName);
            }
        }

        public void ReadConfig()
        {
            try
            {
                if (File.Exists(fileName))
                {
                    using (StreamReader streamReader = new StreamReader(fileName))
                    {
                        Properties = serializer.Deserialize(streamReader) as Properties;
                    }
                }
            }
            catch (Exception) { }
        }

        public void WriteConfig()
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(fileName))
                {
                    serializer.Serialize(streamWriter, Properties);
                }
            }
            catch (Exception) { }
        }
    }
}