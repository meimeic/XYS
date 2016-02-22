using System.Collections.Generic;

using XYS.Lis.Core;
namespace XYS.Lis.Export
{
    public interface IReportExport
    {
        string ExportName { get; }
        void export(IReportElement reportElement, IExportElement exportElement);
        void export(List<IReportElement> reportElements, List<IExportElement> exportElements);
    }
}
