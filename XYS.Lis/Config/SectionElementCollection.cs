using System;
using System.Configuration;

namespace XYS.Lis.Config
{
    public class SectionElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SectionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as SectionElement).Name;
        }
        public new ReportElementElement this[string name]
        {
            get
            {
                return BaseGet(name) as ReportElementElement;
            }
        }
        public new ReportElementElement this[int index]
        {
            get
            {
                return BaseGet(index) as ReportElementElement;
            }
        }
        protected override string ElementName
        {
            get
            {
                return "section";
            }
        }
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }
    }
}
