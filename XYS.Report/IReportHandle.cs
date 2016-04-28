namespace XYS.Report
{
    public delegate void HandleErrorHandler(IReportElement report);
    public delegate void HandleCompletedHandler(IReportElement report);
    public interface IReportHandle
    {
        event HandleErrorHandler HandleErrorEvent;
        event HandleCompletedHandler HandleCompletedEvent;

        IReportHandle Next { get; set; }
        void ReportOption(IReportElement report);
    }
}