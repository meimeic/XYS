using System;
using System.Configuration;

namespace XYS.FR.Conf
{
     [ConfigurationCollection(typeof(LabModelCollection), AddItemName = "model")]
    public class LabModelCollection : ConfigurationElementCollection
    {
         protected override ConfigurationElement CreateNewElement()
         {
             return new LabModelSection();
         }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LabModelSection)element).Name;
        }

        public new LabModelSection this[string name]
        {
            get
            {
                return BaseGet(name) as LabModelSection;
            }
        }
        public LabModelSection this[int index]
        {
            get
            {
                return BaseGet(index) as LabModelSection;
            }
        }
    }
}