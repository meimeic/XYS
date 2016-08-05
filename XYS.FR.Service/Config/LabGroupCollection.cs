using System;
using System.Configuration;

namespace XYS.FR.Service.Config
{
    [ConfigurationCollection(typeof(LabGroupSection), AddItemName = "group")]
    public class LabGroupCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LabGroupSection();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LabGroupSection)element).Name;
        }
        public new LabGroupSection this[string name]
        {
            get
            {
                return BaseGet(name) as LabGroupSection;
            }
        }
        public LabGroupSection this[int index]
        {
            get
            {
                return BaseGet(index) as LabGroupSection;
            }
        }
    }
}
