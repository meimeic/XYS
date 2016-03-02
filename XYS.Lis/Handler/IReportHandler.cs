using System;
using System.Collections.Generic;

using XYS.Lis.Core;
using XYS.Lis.Model;
namespace XYS.Lis.Handler
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
