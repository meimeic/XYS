using System;
using System.Collections.Generic;

using XYS.Model;
namespace XYS.Report.Handler
{
    public interface IReportHandler
    {
        string HandlerName { get; }
        IReportHandler Next { get; set; }
        HandlerResult ReportOptions(IReportElement reportElement);
        HandlerResult ReportOptions(List<IReportElement> reportElementList);
        //HandlerResult ReportOptions(List<IReportElement> reportElementList, Type type);
    }
}
