using System.Collections;
using XYS.Model;
using XYS.Lis.Core;
namespace XYS.Lis.Handler
{
    public interface IReportHandler
    {
        string HandlerName { get; }
        HandlerResult ReportOptions(ILisReportElement reportElement);
        HandlerResult ReportOptions(Hashtable reportElementTable, ReportElementTag elementTag);
        IReportHandler Next { get; set; }
    }
}
