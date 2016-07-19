using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;

using XYS.Lis.Report.Model;
namespace XYS.Lis.FRService
{
    /// <summary>
    /// PrintService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class PrintService : WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public bool PrintReport(LabReport report)
        {
            return true;
        }
    }
}
