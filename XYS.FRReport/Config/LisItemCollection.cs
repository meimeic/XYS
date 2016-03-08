using System;
using System.Configuration;

namespace XYS.FRReport.Config
{
     [ConfigurationCollection(typeof(LisItemSection), AddItemName = "lisItem")]
    public class LisItemCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LisItemSection();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LisItemSection)element).Name;
        }
        public new LisItemSection this[string name]
        {
            get
            {
                return BaseGet(name) as LisItemSection;
            }
        }
        public LisItemSection this[int index]
        {
            get
            {
                return BaseGet(index) as LisItemSection;
            }
        }
    }
}
