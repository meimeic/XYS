using System;
using System.Collections.Generic;

using XYS.Report.Lis.Core;
namespace XYS.Report.Lis
{
    public interface IReport
    {
        bool InitReport(ILisReportElement report, Require require);
        bool InitReport(ILisReportElement report, LisReportPK key);
        
        bool InitReports(List<ILisReportElement> reportList, Require require);
        bool InitReports(List<ILisReportElement> reportList, List<LisReportPK> keyList);
    }
}