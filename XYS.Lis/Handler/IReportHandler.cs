using System;
using System.Collections.Generic;

using XYS.Lis.Model;
namespace XYS.Lis.Handler
{
    public interface IReportHandler
    {
        string HandlerName { get; }
        IReportHandler Next { get; set; }
        HandlerResult ReportOptions( ReportReportElement reportElement);
        HandlerResult ReportOptions(List<ReportReportElement> reportElementList);
        //HandlerResult ReportOptions(List<IReportElement> reportElementList, Type type);
    }
}
