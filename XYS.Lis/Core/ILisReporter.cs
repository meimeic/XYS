using System;
using System.Collections.Generic;

using XYS.Common;
using XYS.Model;
using XYS.Lis.Repository;
using XYS.Lis.Export;
namespace XYS.Lis.Core
{
    public interface ILisReporter
    {
        string ReporterName { get; }
        string StrategyName { get; protected set; }
        IReporterRepository Repository { get; }

        #region
       // ILisReportElement GetReportElement(ReportKey key, ReportElementTag elementTag, string fillerName);
        //List<ILisReportElement> GetReportElements(ReportKey key, ReportElementTag elementTag, string fillerName);
        void FillReportElement(ILisReportElement reportElement, ReportKey key);
        void FillReportElement(List<ILisReportElement> reportElement, ReportKey key, ReportElementTag elementTag);
        #endregion
       
        #region
        bool Option(ILisReportElement reportElement);
        bool Option(List<ILisReportElement> reportElementList,ReportElementTag elementTag);
        #endregion
      
        #region
        string Export(ILisReportElement reportElement);
        string Export(List<ILisReportElement> reportElementList);
        string Export(List<ILisReportElement> reportElementList,ReportElementTag elementTag);
        #endregion

    }
}
