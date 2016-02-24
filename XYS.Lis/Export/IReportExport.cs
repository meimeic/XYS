using System.Collections.Generic;

using XYS.Lis.Model;
using XYS.Lis.Export.Model;
namespace XYS.Lis.Export
{
    public interface IReportExport
    {
        string ExportName { get; }
        void export(ReportReportElement report, ReportReport export);
        void export(List<ReportReportElement> reportElements, List<ReportReport> exportElements);
    }
}
