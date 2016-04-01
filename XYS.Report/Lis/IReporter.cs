using System;

using XYS.Report.Lis.Model;

namespace XYS.Report.Lis
{
    public interface IReporter
    {

        #region
        void FillReport(ReportReportElement report, LisReportPK RK);
        #endregion

        #region
        bool OptionReport(ReportReportElement report);
        #endregion

    }
}