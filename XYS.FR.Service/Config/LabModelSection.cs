using System;
using System.Configuration;

namespace XYS.FR.Service.Config
{
    public class LabModelSection : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return this["name"] as string;
            }
            private set { }
        }

        [ConfigurationProperty("alias", IsRequired = false)]
        public string Alias
        {
            get
            {
                return this["alias"] as string;
            }
            private set { }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public int? Value
        {
            get
            {
                return (int?)this["value"];
            }
            private set { }
        }
        
        [ConfigurationProperty("modelName", IsRequired = true)]
        public string ModelName
        {
            get
            {
                return this["modelName"] as string;
            }
            private set { }
        }
    }
}