using System;
using System.Collections.Generic;

using XYS.Report.Core;
using XYS.Report.Model;
namespace XYS.Report.Handler
{
    public interface IReportHandler
    {
        string HandlerName { get; }
        IReportHandler Next { get; set; }
        HandlerResult ReportOptions(ILisReportElement reportElement);
       // HandlerResult ReportOptions(List<ILisReportElement> reportElementList);
        HandlerResult ReportOptions(List<ILisReportElement> reportElementList, Type type);
    }
}
