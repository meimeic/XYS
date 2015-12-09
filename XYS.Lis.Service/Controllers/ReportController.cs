using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using XYS.Lis.Service.Models.Report;
using XYS.Lis.Service.Common;
using XYS.Lis.Core;
using XYS.Lis;
using XYS.Lis.Model;

namespace XYS.Lis.Service.Controllers
{
     [RoutePrefix("api")]
    public class ReportController : ApiController
    {
         public IEnumerable<IReportModel> GetAllReport()
         {
             return null;
         }
         //获取指定id的报告
          [Route("report/all/{id}")]
          [HttpGet]
         public IReportModel GetReport(string id)
         {
             ReportReport rr = new ReportReport();
             return rr;
         }
         //获取指定患者的报告
         [Route("report/{patient}/{visit:int?}")]
         [HttpGet]
         public IEnumerable<IReportModel> GetReportList([FromUri] string patient,[FromUri] int visit=-1)
         {
             List<IReportModel> reportList = new List<IReportModel>(10);
             LisSearchRequire require = new LisSearchRequire(10, 365);
             require.EqualFields.Add("patno", patient);
             //住院
             if (visit > 0)
             {
                 require.EqualFields.Add("hospitalizedTimes", visit);
             }
             //门诊
             if(visit==0)
             {
                 //
             }
             ReportCommon.ReportOperate.SetReportList(reportList, require);
             return reportList;
         }
        
         //条件查询
         [Route("report/all")]
         [HttpGet]
         public IEnumerable<IReportModel> QueryReport([FromUri] string serialno)
         {
             List<IReportModel> reportList = new List<IReportModel>(10);
             ReportCommon.ReportOperate.SetReportListBySerailNo(reportList, serialno);
             return reportList;
         }

    }
}
