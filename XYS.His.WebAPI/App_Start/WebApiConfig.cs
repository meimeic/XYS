using System;
using System.Linq;
using System.Web.Http;
using System.Collections.Generic;
using System.Net.Http.Formatting;

using Newtonsoft.Json.Serialization;
namespace XYS.His.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            JsonMediaTypeFormatter jf = config.Formatters.JsonFormatter;
            jf.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            JsonMediaTypeFormatter fjf = config.Formatters.OfType<JsonMediaTypeFormatter>().First();

            fjf.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}