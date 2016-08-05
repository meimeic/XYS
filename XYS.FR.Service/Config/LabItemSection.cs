using System;
using System.Configuration;

namespace XYS.FR.Service.Config
{
    public class LabItemSection : ConfigurationElement
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

        [ConfigurationProperty("modelNo", IsRequired = false)]
        public int? ModelNo
        {
            get
            {
                return (int?)this["modelNo"];
            }
            private set { }
        }

        [ConfigurationProperty("orderNo", IsRequired = false)]
        public int? OrderNo
        {
            get
            {
                return (int?)this["orderNo"];
            }
            private set { }
        }
    }
}
