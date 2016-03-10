using System;

using XYS.Common;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Core
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
        void Fill(ReportReportElement report, ReportPK RK);
        //void Fill(List<IReportElement> reportElementList, ReportPK RK, Type type);
    }
}
