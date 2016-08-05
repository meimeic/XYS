using System;
using System.Configuration;

namespace XYS.FR.Service.Config
{
    public class LisSection : ConfigurationSection
    {
        [ConfigurationProperty("groups", IsDefaultCollection = true)]
        public LisGroupCollection GroupCollection
        {
            get
            {
                return (LisGroupCollection)base["groups"];
            }
            private set { }
        }

        [ConfigurationProperty("items", IsDefaultCollection = true)]
        public LisItemCollection ItemCollection
        {
            get
            {
                return (LisItemCollection)base["items"];
            }
            private set { }
        }

        [ConfigurationProperty("models", IsDefaultCollection = true)]
        public LisModelCollection ItemCollection
        {
            get
            {
                return (LisModelCollection)base["models"];
            }
            private set { }
        }
    }
}