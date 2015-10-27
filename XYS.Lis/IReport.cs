using System;
using System.Collections.Generic;

using XYS.Common;
using XYS.Lis.Core;
using XYS.Lis.Model;
namespace XYS.Lis
{
    public interface IReport
    {
        string ReportExport(ReportKey key);
        string ReportExport(LisSearchRequire require);
        string ReportExport(ReportReportElement reportElement);
        string ReportExport(List<ILisReportElement> reportElementList);
    }
}
