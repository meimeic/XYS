using System;
using System.Collections.Generic;

using XYS.Model;
using XYS.Common;
namespace XYS.Report.Fill
{
    public enum FillTypeTag
    {
        DB = 1,
        WebService,
        WebAPI,
        None
    }
    public interface IReportFiller
    {
        string FillerName { get; }
        void Fill(IReportElement reportElement, ReportPK RK);
        void Fill(List<IReportElement> reportElementList, ReportPK RK, Type type);
    }
}
