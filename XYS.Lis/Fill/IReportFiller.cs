using System;
using System.Collections.Generic;

using XYS.Lis.Core;
using XYS.Common;
namespace XYS.Lis.Fill
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
        void Fill(IReportElement reportElement, ReportKey RK);
        void Fill(List<IReportElement> reportElementList, ReportKey RK, Type type);
    }
}
