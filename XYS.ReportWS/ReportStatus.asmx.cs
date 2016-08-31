using System;
using System.Web;
using System.Web.Services;

namespace XYS.ReportWS
{
    /// <summary>
    /// ReportStatus 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://report.xys.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class ReportStatus : WebService
    {

        [WebMethod]
        public string Hello()
        {
            return "Hello World";
        }

        [WebMethod]
        public void UpdateLabApplyInfo(string param)
        {
            LisService serivce = Global.ReportService;
            serivce.Handle(param);
        }
    }
}