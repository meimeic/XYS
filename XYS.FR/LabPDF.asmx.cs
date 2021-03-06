﻿using System;
using System.Web;
using System.Web.Services;

using XYS.Model;
using XYS.Report;

using XYS.FR.Lab;
using XYS.FR.Util;
namespace XYS.FR
{
    /// <summary>
    /// PDF 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://fr.xys.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class LabPDF : WebService
    {
        [WebMethod]
        public string HelloWorld()
        {
            //PDFTest test = new PDFTest();
            //test.Test();
            DataStruct.InitXMLStruct();
            return "Hello World";
        }
        [WebMethod]
        public void PrintPDF(byte[] bytes)
        {
            LabService service = Global.LabPDF;
            LabReport report = TransHelper.DeserializeObject(bytes) as LabReport;
            if (report != null)
            {
                service.HandleReport(report);
            }
        }
    }
}