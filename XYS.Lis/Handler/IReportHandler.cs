using System.Collections;
using System.Collections.Generic;

using XYS.Lis.Core;
namespace XYS.Lis.Handler
{
    public interface IReportHandler
    {
        string HandlerName { get; }
        HandlerResult ReportOptions(IReportElement reportElement);
        HandlerResult ReportOptions(List<IReportElement> reportElementList);
        IReportHandler Next { get; set; }
    }
}
