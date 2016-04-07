using System;
using System.Collections.Generic;

using XYS.Report.Lis.Model;
namespace XYS.Report.Lis
{
    public interface IReport
    {
        string operate(ReportReportElement report);
        string operate(Require req, ReportReportElement report);
    }
}