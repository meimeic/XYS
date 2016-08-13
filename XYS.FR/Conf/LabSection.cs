using System;
using System.Configuration;

namespace XYS.FR.Conf
{
    public class LabSection : ConfigurationSection
    {
        [ConfigurationProperty("groups", IsDefaultCollection = true)]
        public LabGroupCollection GroupCollection
        {
            get
            {
                return (LabGroupCollection)base["groups"];
            }
            private set { }
        }

        [ConfigurationProperty("items", IsDefaultCollection = true)]
        public LabItemCollection ItemCollection
        {
            get
            {
                return (LabItemCollection)base["items"];
            }
            private set { }
        }

        [ConfigurationProperty("models", IsDefaultCollection = true)]
        public LabModelCollection ModelCollection
        {
            get
            {
                return (LabModelCollection)base["models"];
            }
            private set { }
        }
    }
}