using System;
using System.Xml;
namespace XYS.Repository
{
    public interface IXmlRepositoryConfigurator
    {
        void Configure(XmlElement element);
    }
}