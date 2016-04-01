namespace XYS.Report
{
    public interface IReportHandler
    {
        string HandlerName { get; }
        IReportHandler Next { get; set; }
        HandlerResult ReportOption(IReportElement report);
    }
}
