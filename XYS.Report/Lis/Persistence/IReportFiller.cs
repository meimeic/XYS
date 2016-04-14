using System;

using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Persistence
{
    public interface IReportFiller
    {
        void FillReport(ReportReportElement report);
    }
}
