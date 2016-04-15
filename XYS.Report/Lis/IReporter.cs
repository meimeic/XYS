using System;
using System.Threading.Tasks;

using XYS.Report.Lis.Model;
namespace XYS.Report.Lis
{
    public delegate HandlerResult FillReport(ReportReportElement report);
    public delegate HandlerResult HandleReport(ReportReportElement report);
    public delegate HandlerResult SaveReport(ReportReportElement report);
    public delegate HandlerResult ExraOperateReport(ReportReportElement report);
    public interface IReporter
    {
        HandlerResult OperateReport(ReportReportElement report);
        async Task<ReportReportElement> OperateReportAsync(ReportReportElement report);
    }
}