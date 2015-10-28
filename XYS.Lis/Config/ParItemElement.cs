using System;
using System.Configuration;

namespace XYS.Lis.Config
{
    public class ParItemElement : ConfigurationElement
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
        [ConfigurationProperty("imageFlag", IsRequired = false)]
        public bool CommonImageFlag
        {
            get
            {
                string s = this["imageFlag"] as string;
                if (s != null && !s.Equals(""))
                {
                    return bool.Parse(s);
                }
                else
                {
                    return false;
                }
            }
            private set { }
        }
        [ConfigurationProperty("imagePath", IsRequired = false)]
        public string CommonImagePath
        {
            get
            {
                return this["imagePath"] as string;
            }
            private set { }
        }
    }
}
