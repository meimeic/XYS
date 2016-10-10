using System;
using System.Web;
using System.Web.Services;

using XYS.Model;
using XYS.Mongo.Lab;
namespace XYS.Mongo
{
    /// <summary>
    /// LabMongo 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://mongo.xys.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class LabMongo : WebService
    {
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public void SaveToMongo(byte[] bytes)
        {
            LabService service = LabService.LService;
            LabReport report = TransHelper.DeserializeObject(bytes) as LabReport;
            if (report != null)
            {
                service.Save2Mongo(report);
            }
        }
    }
}
