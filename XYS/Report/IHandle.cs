using System;
using System.Collections.Generic;

namespace XYS.Report
{
    public interface IHandle
    {
        bool HandleElement(IFillElement element, IReportKey RK);
        bool HandleElement(List<IFillElement> elements, IReportKey RK, Type type);
    }
}