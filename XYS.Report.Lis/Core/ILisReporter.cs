using System;

using XYS.Report.Lis.Model;
using XYS.Report.Lis.Repository;
namespace XYS.Report.Lis.Core
{
    public interface ILisReporter
    {
        IReporterRepository Repository { get; }

        #region
        void FillReport(ReportReportElement report, LisReportPK RK);
        #endregion

        #region
        bool OptionReport(ReportReportElement report);
        #endregion

    }
}