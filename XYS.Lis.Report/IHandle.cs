using System;
using System.Collections.Generic;

using XYS.Report;
namespace XYS.Lis.Report
{
    public interface IHandle
    {
        bool InitElement(IFillElement element, ReportPK RK);
        bool InitElement(List<IFillElement> elements, ReportPK RK, Type type);
    }
}