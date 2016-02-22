using System;
using System.Collections.Generic;

using XYS.Lis.Core;
namespace XYS.Lis.Handler
{
    public interface IReportHandler
    {
        string HandlerName { get; }
        IReportHandler Next { get; set; }
        HandlerResult ReportOptions(IReportElement reportElement);
        HandlerResult ReportOptions(List<IReportElement> reportElementList);
        HandlerResult ReportOptions(List<IReportElement> reportElementList, Type type);
    }
}
