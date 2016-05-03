namespace XYS.Report
{
    public interface IReportHandle
    {
        IReportHandle Next { get; set; }
        void ReportOption(IReportElement report);
    }
}