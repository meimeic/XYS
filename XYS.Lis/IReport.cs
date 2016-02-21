using System;
using System.Collections.Generic;

using XYS.Common;
using XYS.Lis.Model;
using XYS.Lis.Core;
namespace XYS.Lis
{
    public interface IReport
    {
        void InitReport(ReportKey key, ReportReportElement report);
        void InitReport(LisSearchRequire require, ReportReportElement report);
        void InitReports(LisSearchRequire require, List<IReportElement> reportList);
        void InitReports(List<ReportKey> keyList, List<IReportElement> reportList);
        
        bool OperateReport(ReportReportElement report);
        bool OperateReport(List<IReportElement> reportList);
        
        void HandleReport(LisSearchRequire require, ReportReportElement report);
        void HandleReports(LisSearchRequire require, List<IReportElement> reportList);
        void HandleReports(List<ReportKey> keyList, List<IReportElement> reportList);
      
    }
}