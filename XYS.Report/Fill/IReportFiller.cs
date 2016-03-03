using System;
using System.Collections.Generic;

using XYS.Report.Core;
using XYS.Util;
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
        void Fill(ILisReportElement reportElement, ReportKey RK);
        void Fill(List<ILisReportElement> reportElementList, ReportKey RK, Type type);
    }
}
