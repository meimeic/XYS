using System;
using System.Xml;

namespace XYS.Lis.Config
{
    public class XmlDBConfigurator
    {
        private static readonly string CONFIGURATION_TAG = "lis-db";
        private static readonly string CONNECTION_TAG = "connStr";

        private static readonly string NAME_ATTR = "name";
        private static readonly string CONNECTIONSTRING_ATTR = "connectionString";
        private static readonly string PROVIDERNAME = "providerName";

        public static string GetConnectionString()
        {
            XmlElement dbElement = GetTargetElement(CONNECTION_TAG);
            if (dbElement != null)
            {
                if (dbElement.LocalName == CONNECTION_TAG)
                {
                    return dbElement.GetAttribute(CONNECTIONSTRING_ATTR);
                }
            }
            return null;
        }
        private static XmlElement GetTargetElement(string targetTag)
        {
            XmlElement configElement = XmlConfigurator.GetParamConfigurationElement(CONFIGURATION_TAG);
            if (configElement != null)
            {
                foreach (XmlNode node in configElement.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        if (node.LocalName == targetTag)
                        {
                            return node as XmlElement;
                        }
                    }
                }
            }
            return null;
        }
    }
}
