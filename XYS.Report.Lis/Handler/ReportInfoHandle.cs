using System;
using XYS.Util;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public class ReportInfoHandle : ReportHandleSkeleton
    {
        public ReportInfoHandle()
            : base()
        {
        }
        public override void ReportHandle(ReportReportElement report)
        {
            LOG.Info("报告基本信息处理");
            //formmemo 处理
            if (report.ReportInfo.SectionNo == 11)
            {
                LOG.Info("备注信息处理");
                if (report.ReportInfo.Memo != null)
                {
                    report.ReportInfo.Memo = report.ReportInfo.Memo.Replace(";", SystemInfo.NewLine);
                }
            }
            //cid 处理
            if (report.ReportInfo.CID != null)
            {
                LOG.Info("身份证信息处理");
                report.ReportInfo.CID = report.ReportInfo.CID.Trim();
            }
        }
    }
}
