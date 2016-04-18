using System;
using System.Collections.Generic;

using XYS.Report.Lis.Model;
namespace XYS.Report.Lis
{
    public interface IReport
    {
        void Operate(ReportReportElement report, HandleResult result);
        void OperateAsync(List<ReportReportElement> reportList, Action<ReportReportElement, HandleResult> callbak = null);
        
        List<ReportReportElement> GetReports(Require req);
        List<ReportReportElement> GetReports(string where);
        
        List<LisReportPK> GetReportPK(Require req);
        List<LisReportPK> GetReportPK(string where);
    }
}