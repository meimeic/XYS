using System;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using XYS.FR.Lab;
namespace XYS.FR
{
    public class Global : System.Web.HttpApplication
    {
        public static LabService LabPDF
        {
            get { return LabService.PService; }
        }
        protected void Application_Start(object sender, EventArgs e)
        {

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