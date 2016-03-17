using System;

using XYS.Report.Lis.Repository;
namespace XYS.Report.Lis.Core
{
    public interface ILisReporter
    {
        IReporterRepository Repository { get; }

        #region
        void FillReport(ILisReportElement report, LisReportPK RK);
        #endregion

        #region
        bool OptionReport(ILisReportElement report);
        #endregion

        bool IsLisReport(ILisReportElement report);
    }
}