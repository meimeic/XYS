using System;
using System.Configuration;


namespace XYS.Lis.Config
{
    public class ReportElementElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ReportElementElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as ReportElementElement).Name;
        }
        public new ReportElementElement this[string name]
        {
            get
            {
                return BaseGet(name) as ReportElementElement;
            }
        }
        public ReportElementElement this[int index]
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
                return "reportElement";
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
