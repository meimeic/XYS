using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace XYS.Report.WS
{
    /// <summary>
    /// ReportStatus 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class ReportStatus : System.Web.Services.WebService
    {

        [WebMethod]
        public string Hello()
        {
            return "Hello World";
        }

        [WebMethod]
        public string UpdateLabApplyInfo(string param)
        {
            return "status:OK";
        }
        [WebMethod]
        public string UpdateExamApplyInfo(string param)
        {
            return "status:OK";
        }
    }
}
