using System;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public class ReportHeadHandler : ReportHandlerSkeleton
    {
        #region
        public ReportHeadHandler()
            : base()
        {

        }
        #endregion

        #region
        protected override HandlerResult OperateReport(ReportReportElement report)
        {
            return new HandlerResult(0, "the ReportHeadHandler do nothing just only the head,so continue!");
        }
        #endregion
    }
}
