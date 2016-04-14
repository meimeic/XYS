namespace XYS.Report
{
    public interface IReportHandler
    {
        IReportHandler Next { get; set; }
        HandlerResult ReportOption(IReportElement report);
    }
}