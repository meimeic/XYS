using System;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis
{
    public delegate void HandleReportError(ReportReportElement report);
    public delegate void HandleReportSuccess(ReportReportElement report);
    public interface ILisReportHandle
    {
        event HandleReportError HandleReportErrorEvent;
        event HandleReportSuccess HandleReportSuccessEvent;

        void ReportHandle(ReportReportElement report);
    }
}
