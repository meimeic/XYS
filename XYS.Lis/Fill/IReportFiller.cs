using System.Collections.Generic;

using XYS.Lis.Core;
using XYS.Common;
using XYS.Lis.Model;
namespace XYS.Lis.Fill
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
        void Fill(ReportReportElement reportElement, ReportKey RK);
        //void Fill(List<IReportElement> reportElementList, List<ReportKey> RKList);
    }
}
