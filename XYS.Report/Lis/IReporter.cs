using System;

using XYS.Report.Lis.Model;
namespace XYS.Report.Lis
{
    public interface IReporter
    {
        string OperateReport(ReportReportElement report);
    }
}