using System;
using System.Collections.Generic;

using XYS.Model;
using XYS.Common;
using XYS.Repository;
namespace XYS.Report.Lis.Core
{
    public interface ILisReporter:IReporter
    {
        IReporterRepository Repository { get; }

        #region
        void FillReport(IReportElement report, ReportPK RK);
        #endregion

        //报告元素处理
        #region
        bool OptionReport(IReportElement report);
        bool OptionReport(List<IReportElement> reportList);
        #endregion
    }
}