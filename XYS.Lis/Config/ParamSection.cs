using System;
using System.Configuration;

namespace XYS.Lis.Config
{
    public class ParamSection : ConfigurationSection
    {
        [ConfigurationProperty("reportElements", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ReportElementElementCollection), AddItemName = "reportElement")]
        public ReportElementElementCollection ReportElementCollection
        {
            get
            {
                return (ReportElementElementCollection)base["reportElements"];
            }
            private set { }
        }
        [ConfigurationProperty("reportSections", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ReportSectionElementCollection), AddItemName = "reportSection")]
        public ReportSectionElementCollection SectionCollection
        {
            get
            {
                return (ReportSectionElementCollection)base["reportSections"];
            }
            private set { }
        }
        [ConfigurationProperty("parItems", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ParItemElementCollection), AddItemName = "parItem")]
        public ParItemElementCollection ParItemCollection
        {
            get
            {
                return (ParItemElementCollection)base["parItems"];
            }
            private set { }
        }
        [ConfigurationProperty("reportModels", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ReportModelElementCollection), AddItemName = "reportModel")]
        public ReportModelElementCollection ModelCollection
        {
            get
            {
                return (ReportModelElementCollection)base["reportModels"];
            }
            private set { }
        }
    }
}
