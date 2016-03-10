using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Core
{
    public interface IReportHandler
    {
        string HandlerName { get; }
        IReportHandler Next { get; set; }
        HandlerResult ReportOptions(ReportReportElement report);
        //HandlerResult ReportOptions(List<IReportElement> reportElementList);
        //HandlerResult ReportOptions(List<IReportElement> reportElementList, Type type);
    }
}
