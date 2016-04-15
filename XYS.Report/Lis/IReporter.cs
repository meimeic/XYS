using System;
using System.Threading.Tasks;

using XYS.Report.Lis.Model;
namespace XYS.Report.Lis
{
    //public delegate HandlerResult FillReport(ReportReportElement report);
    //public delegate HandlerResult HandleReport(ReportReportElement report);
    //public delegate HandlerResult SaveReport(ReportReportElement report);
    public delegate Task ExraOperateReport(ReportReportElement report,HandlerResult result);
    public interface IReporter
    {
        void OperateReport(ReportReportElement report, HandlerResult result);
        async Task OperateReportAsync(ReportReportElement report, HandlerResult result, Action<ReportReportElement, HandlerResult> callback = null);

    }
}