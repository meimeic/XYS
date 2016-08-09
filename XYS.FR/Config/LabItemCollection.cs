using System;
using System.Configuration;

namespace XYS.FR.Config
{
    [ConfigurationCollection(typeof(LabItemSection), AddItemName = "item")]
    public class LabItemCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LabItemSection();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LabItemSection)element).Name;
        }
        public new LabItemSection this[string name]
        {
            get
            {
                return BaseGet(name) as LabItemSection;
            }
        }
        public LabItemSection this[int index]
        {
            get
            {
                return BaseGet(index) as LabItemSection;
            }
        }
    }
}
