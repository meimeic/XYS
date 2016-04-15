using System;
using System.Threading.Tasks;

using XYS.Report.Lis.Model;
namespace XYS.Report.Lis
{
    public delegate HandlerResult FillReport(ReportReportElement report);
    public delegate HandlerResult HandleReport(ReportReportElement report);
    public delegate HandlerResult SaveReport(ReportReportElement report);
    public delegate HandlerResult ExraOperateReport(ReportReportElement report);
    public delegate Task<HandlerResult> CallBackHandler(ReportReportElement report,Func<ReportReportElement,HandlerResult> callback);
    public interface IReporter
    {
        HandlerResult OperateReport(ReportReportElement report);
        async Task OperateReportAsync(ReportReportElement report, Action<ReportReportElement, HandlerResult> callback);
    }
}