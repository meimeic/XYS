using System;
using System.Collections.Generic;

using XYS.Report.Lis.Core;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis
{
    public interface IReport
    {
        bool InitReport( ReportReportElement report, Require require);
        bool InitReport(ReportReportElement report, LisReportPK key);

        bool InitReports(List<ReportReportElement> reportList, Require require);
        bool InitReports(List<ReportReportElement> reportList, List<LisReportPK> keyList);

        void InsertToMongo(ReportReportElement report);
    }
}