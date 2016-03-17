using System;
using System.Xml;
namespace XYS.Report.Lis.Repository
{
    public interface IXmlRepositoryConfigurator
    {
        void Configure(XmlElement element);
    }
}