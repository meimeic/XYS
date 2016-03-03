using System;
using System.Xml;
namespace XYS.Report.Repository
{
   public interface IXmlRepositoryConfigurator
    {
       void Configure(XmlElement element);
    }
}
