using XYS.Model;
using XYS.Repository;
namespace XYS.Common
{
    public interface IReporter
    {
        IReporterRepository Repository { get; }

        #region
        void FillReport(IReportElement report, ReportPK RK);
        #endregion

        #region
        bool OptionReport(IReportElement report);
        #endregion
    }
}