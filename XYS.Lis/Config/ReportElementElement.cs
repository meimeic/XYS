using System;
using System.Configuration;

namespace XYS.Lis.Config
{
    public class ReportElementElement : ConfigurationElement
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
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get
            {
                return this["type"] as string;
            }
            private set { }
        }
        [ConfigurationProperty("explain", IsRequired = false)]
        public string Explain
        {
            get
            {
                return this["explain"] as string;
            }
            private set { }
        }
    }
}
