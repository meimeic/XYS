using System;
using System.Collections.Generic;

using XYS.Common;
using XYS.Lis.Repository;
namespace XYS.Lis.Core
{
    public interface IReporter
    {
        IReporterRepository Repository { get; }
        //void InitExport(ReportReport export,ReportKey RK);
        //void InitExport(List<ReportReport> exportList, List<ReportKey> RKList);
        //报告元素填充
        #region
        void FillReportElement(ILisReportElement reportElement, ReportKey key);
        void FillReportElement(List<ILisReportElement> reportElementList, ReportKey key,Type type);
        #endregion
        
        //报告元素处理
        #region
        bool Option(ILisReportElement reportElement);
        bool Option(List<ILisReportElement> reportElementList, Type type);
        #endregion
    }
}