using System;
using System.Configuration;

namespace XYS.FRReport.Config
{
    public class LisFRSection : ConfigurationSection
    {
        [ConfigurationProperty("lisGroups", IsDefaultCollection = true)]
        public LisGroupCollection GroupCollection
        {
            get
            {
                return (LisGroupCollection)base["lisGroups"];
            }
            private set { }
        }

        [ConfigurationProperty("lisItems", IsDefaultCollection = true)]
        public LisItemCollection ItemCollection
        {
            get
            {
                return (LisItemCollection)base["lisItems"];
            }
            private set { }
        }
    }
}
