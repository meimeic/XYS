using System;
using System.Collections.Generic;

using XYS.Model;
using XYS.Common;
namespace XYS.Report
{
    public interface IReport
    {
        bool InitReport(IReportElement report, ReportPK key);
        bool InitReport(IReportElement report, Require require);
        bool InitReports(List<IReportElement> exportList, Type reportType, List<ReportPK> keyList);
        bool InitReports(List<IReportElement> exportList, Type reportType, Require require);
    }
}