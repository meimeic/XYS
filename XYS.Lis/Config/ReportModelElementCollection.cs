using System;
using System.Configuration;


namespace XYS.Lis.Config
{
    public class ReportModelElementCollection: ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new ReportModelElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as ReportModelElement).Name;
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
                return "reportModel";
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
