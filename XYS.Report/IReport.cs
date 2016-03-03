using System;
using System.Collections.Generic;

using XYS.Util;
using XYS.Model;
using XYS.Common;
namespace XYS.Report
{
    public interface IReport
    {
        //void InitReport(ReportKey key, ReportReportElement report);
        //void InitReport(LisSearchRequire require, ReportReportElement report);
        //void InitReports(LisSearchRequire require, List<IReportElement> reportList);
        //void InitReports(List<ReportKey> keyList, List<IReportElement> reportList);
        //bool OperateReport(ReportReportElement report);
        //bool OperateReport(List<IReportElement> reportList);
        //void HandleReport(LisSearchRequire require, ReportReportElement report);
        //void HandleReports(LisSearchRequire require, List<IReportElement> reportList);
        //void HandleReports(List<ReportKey> keyList, List<IReportElement> reportList);
        bool InitReport(IReportElement report, ReportPK key);
        bool InitReport(IReportElement report, LisRequire require);
        bool InitReports(List<IReportElement> exportList, List<ReportKey> keyList);
        bool InitReports(List<IReportElement> exportList, LisRequire require);
    }
}