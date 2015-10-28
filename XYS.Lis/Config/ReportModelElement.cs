using System;
using System.Configuration;

namespace XYS.Lis.Config
{
    public class ReportModelElement : ConfigurationElement
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
        [ConfigurationProperty("modelPath", IsRequired = true)]
        public string ModelPath
        {
            get
            {
                return this["modelPath"] as string;
            }
            private set { }
        }
    }
}
