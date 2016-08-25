using System;

using XYS.Report;
using XYS.Lis.Report.Persistent;
namespace XYS.Lis.Report.Handler
{
    public class GeneCustomHandle : HandleSkeleton
    {
        #region 构造函数
        public GeneCustomHandle(LabReportDAL dal)
            : base(dal)
        {
        }
        #endregion

        protected override bool InnerHandle(IFillElement element, IReportKey RK)
        {
            element.ReportID = RK.ID;
            return true;
        }

        protected override bool InnerHandle(System.Collections.Generic.List<IFillElement> elements, IReportKey RK)
        {
            return true;
        }
    }
}
