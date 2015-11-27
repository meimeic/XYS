using System.Collections.Generic;

using XYS.Model;
using XYS.Lis.Core;
using XYS.Lis.Export;

namespace XYS.Lis.Export
{
    public interface IReportExport
    {
        string ExportName { get; }
        ExportTag ExportTag { get;}
        string export(ILisExportElement element);
        string export(List<ILisExportElement> exportElementList, ReportElementTag elementTag);
    }
}
