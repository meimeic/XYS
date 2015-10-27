using System.Configuration;
using System.Xml;

namespace XYS.Lis.Config
{
    public class ReportSectionHandler : IConfigurationSectionHandler
    {
        public ReportSectionHandler()
        {
 
        }
        public object Create(object parent, object configContext, XmlNode section)
        {
            return section;
        }
    }
}
