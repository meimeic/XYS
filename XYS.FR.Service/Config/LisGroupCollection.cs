using System;
using System.Configuration;

namespace XYS.FR.Service.Config
{
    [ConfigurationCollection(typeof(LisGroupSection), AddItemName = "group")]
    public class LisGroupCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LisGroupSection();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LisGroupSection)element).Name;
        }
        public new LisGroupSection this[string name]
        {
            get
            {
                return BaseGet(name) as LisGroupSection;
            }
        }
        public LisGroupSection this[int index]
        {
            get
            {
                return BaseGet(index) as LisGroupSection;
            }
        }
    }
}
