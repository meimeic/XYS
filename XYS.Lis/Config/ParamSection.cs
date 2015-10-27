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
                return (ReportElementElementCollection)this["reportElements"];
            }
            private set { }
        }
        [ConfigurationProperty("sections", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(SectionElementCollection), AddItemName = "section")]
        public SectionElementCollection SectionCollection
        {
            get
            {
                return (SectionElementCollection)this["sections"];
            }
            private set { }
        }
        [ConfigurationProperty("parItems", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ParItemElementCollection), AddItemName = "parItem")]
        public ParItemElementCollection ParItemCollection
        {
            get
            {
                return (ParItemElementCollection)this["parItems"];
            }
            private set { }
        }
        [ConfigurationProperty("models", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ModelElementCollection), AddItemName = "model")]
        public ModelElementCollection ModelCollection
        {
            get
            {
                return (ModelElementCollection)this["models"];
            }
            private set { }
        }
    }
}
