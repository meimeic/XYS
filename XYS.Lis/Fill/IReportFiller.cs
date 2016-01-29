using System;
using System.Collections.Generic;

using XYS.Lis.Core;
using XYS.Common;
namespace XYS.Lis.Fill
{
    public interface IReportFiller
    {
        string FillerName { get; }
        void Fill(IReportElement reportElement, ReportKey PK);
        void Fill(List<IReportElement> reportElementList, ReportKey PK, ReportElementTag elementTag);
    }
}
