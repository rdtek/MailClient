using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MailClient
{
    public class AppConfigManager 
    {
        [NonSerialized]
        public static string DefaultConfigFileName = @"C:\temp\appconfig.xml";

        public static AppConfigInstance Load()
        {
            return Load(DefaultConfigFileName);
        }

        public static AppConfigInstance Load(string configFileName)
        {
            var config = new AppConfigInstance();

            if (File.Exists(configFileName))
            {
                using (FileStream stream = File.OpenRead(configFileName))
                {
                    config = new XmlSerializer(typeof(AppConfigInstance))
                        .Deserialize(stream) as AppConfigInstance;
                }

            }

            return config;
        }

        public static void Save(AppConfigInstance config)
        {
            Save(config, DefaultConfigFileName);
        }

        public static void Save(AppConfigInstance config, string configFileName)
        {
            if (File.Exists(configFileName)) File.Delete(configFileName);

            using (FileStream stream = new FileStream(configFileName, FileMode.Create)) {
                new XmlSerializer(typeof(AppConfigInstance)).Serialize(stream, config);
            }
        }
    }

    [Serializable]
    public class AppConfigInstance
    {
        public string Host { get; set; }
        public string FromEmailAddress { get; set; }
    }
}
