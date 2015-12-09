using System;
using System.Collections.Generic;

using XYS.Model;
using XYS.Lis.Core;
using XYS.Common;
namespace XYS.Lis.Fill
{
    public interface IReportFiller
    {
        string FillerName { get; }
        void Fill(ILisReportElement reportElement, ReportKey PK);
        void Fill(List<ILisReportElement> reportElementList, ReportKey PK, ReportElementTag elementTag);
    }
}
