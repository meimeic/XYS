using System;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
using System.Web.Services;

using XYS.Util;
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
    public class ReportStatus : WebService
    {
        private object lockObject = new object();
        protected void WriteLog(string message)
        {
            lock (lockObject)
            {
                var file = System.IO.File.AppendText("D:\\log.txt");
                file.WriteLine(message);
                file.Close();
            }
        }
        [WebMethod]
        public string Hello()
        {
            WriteLog(string.Format("主线程({0})开始", SystemInfo.CurrentThreadId));
            return "Hello World";
        }

        [WebMethod]
        public void UpdateLabApplyInfo(string param)
        {

        }
    }
}
