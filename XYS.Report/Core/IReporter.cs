using System;
using System.Collections.Generic;

using XYS.Model;
using XYS.Common;
using XYS.Report.Repository;
namespace XYS.Report.Core
{
    public interface IReporter
    {
        IReporterRepository Repository { get; }
        #region
        void FillReport(IReportElement report, ReportKey RK);
        #endregion
        
        //报告元素处理
        #region
        bool OptionReport(IReportElement report);
        bool OptionReport(List<IReportElement> reportList);
        #endregion
    }
}