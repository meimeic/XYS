using System.Collections.Generic;
using XYS.Model;
using XYS.Lis.Core;
using XYS.Lis.Export;

namespace XYS.Lis.Export
{
    public interface IReportExport
    {
        string ExportName { get; }
        ExportTag ExportTag { get; protected set; }
        string export(ILisReportElement element);
        string export(List<ILisReportElement> reportElementList, ReportElementTag elementTag);
    }
}
