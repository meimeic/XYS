using System;
using System.Xml;
namespace XYS.Report.Repository
{
    public interface IXmlRepositoryConfigurator
    {
        string XmlConfigTag { get; }
        void Configure(XmlElement element);
    }
}