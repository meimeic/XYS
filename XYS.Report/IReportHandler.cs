namespace XYS.Report
{
    public interface IReportHandler
    {
        IReportHandler Next { get; set; }
        void ReportOption(IReportElement report,HandlerResult result);
    }
}