using System;
using System.Configuration;

namespace XYS.FR.Service.Config
{
     [ConfigurationCollection(typeof(LisModelCollection), AddItemName = "model")]
    public class LisModelCollection : ConfigurationElementCollection
    {
         protected override ConfigurationElement CreateNewElement()
         {
             return new LisModelSection();
         }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LisModelSection)element).Name;
        }

        public new LisModelSection this[string name]
        {
            get
            {
                return BaseGet(name) as LisModelSection;
            }
        }
        public LisModelSection this[int index]
        {
            get
            {
                return BaseGet(index) as LisModelSection;
            }
        }
    }
}