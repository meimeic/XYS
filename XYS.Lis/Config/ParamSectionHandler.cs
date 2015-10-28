using System.Xml;
using System.Configuration;


namespace XYS.Lis.Config
{
    public class ParamSectionHandler : IConfigurationSectionHandler
    {
        public ParamSectionHandler()
        {
        }
        public object Create(object parent, object configContext, XmlNode section)
        {
            return section;
        }
    }
}
