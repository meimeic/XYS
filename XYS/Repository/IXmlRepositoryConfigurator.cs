using System;
using System.Xml;
namespace XYS.Lis.Repository
{
   public interface IXmlRepositoryConfigurator
    {
       void Configure(XmlElement element);
    }
}
