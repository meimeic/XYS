using System;
using System.Collections.Generic;

using XYS.Model;
using XYS.Common;
namespace XYS.Report.Lis
{
    public interface IReport
    {
        bool InitReport(IReportElement report, ReportPK key);
        bool InitReport(IReportElement report, Require require);
        bool InitReports(List<IReportElement> reportList, List<ReportPK> keyList);
        bool InitReports(List<IReportElement> reportList, Require require);
    }
}