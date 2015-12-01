using System;
using System.Collections.Generic;

using XYS.Model;
using XYS.Common;
using XYS.Lis.Repository;
namespace XYS.Lis.Core
{
    public interface ILisReporter
    {
        string ReporterName { get; }
        string StrategyName { get; }
        IReporterRepository Repository { get; }

        #region
        void FillReportElement(ILisReportElement reportElement, ReportKey key);
        void FillReportElement(List<ILisReportElement> reportElementList, ReportKey key, ReportElementTag elementTag);
        #endregion

        #region
        bool Option(ILisReportElement reportElement);
        bool Option(List<ILisReportElement> reportElementList, ReportElementTag elementTag);
        #endregion
    }
}