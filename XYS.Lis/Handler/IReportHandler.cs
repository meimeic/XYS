using System.Collections.Generic;
using XYS.Model;
using XYS.Lis.Core;
namespace XYS.Lis.Handler
{
    public interface IReportHandler
    {
        string HandlerName { get; }
        HandlerResult ReportOptions(ILisReportElement reportElement);
        HandlerResult ReportOptions(List<ILisReportElement> reportElementList, ReportElementTag elementTag);
        IReportHandler Next { get; set; }
    }
}
