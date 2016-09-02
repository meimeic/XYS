using System;
using System.Web;
using System.Web.SessionState;

using log4net.Config;

using XYS.Mongo.Lab;
namespace XYS.Mongo
{
    public class Global : System.Web.HttpApplication
    {
        public static LabService DBService
        {
            get { return LabService.LService; }
        }
        protected void Application_Start(object sender, EventArgs e)
        {
            XmlConfigurator.Configure();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}