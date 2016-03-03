using System;
using System.Collections.Generic;

using XYS.Common;
using XYS.Lis.Model;
using XYS.Lis.Repository;
namespace XYS.Lis.Core
{
    public interface IReporter
    {
        IReporterRepository Repository { get; }
        #region
        void FillReport(ReportReportElement report, ReportKey RK);
        #endregion
        
        //报告元素处理
        #region
        bool OptionReport(ReportReportElement report);
        bool OptionReport(List<ILisReportElement> reportList);
        #endregion
    }
}