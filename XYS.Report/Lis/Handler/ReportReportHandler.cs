using System;

using XYS.Util;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public class ReportReportHandler : ReportHandlerSkeleton
    {
        #region 构造函数
        public ReportReportHandler()
            : base()
        {
        }
        #endregion

        #region 实现父类方法
        protected override void OperateReport(ReportReportElement report, HandlerResult result)
        {
            //formmemo 处理
            if (report.SectionNo == 10)
            {
                if (report.FormMemo != null)
                {
                    report.FormMemo = report.FormMemo.Replace(";", SystemInfo.NewLine);
                }
            }

            //cid 处理
            if (report.CID != null)
            {
                report.CID = report.CID.Trim();
            }

            //reportitem排序
            report.ReportItemCollection.Sort();

            //返回值
            this.SetHandlerResult(result, 0, "handle ReportReportElement successfully and continue!");
            return;
        }
        #endregion

        #region 备注设置
        protected virtual void SetRemark(ReportReportElement rre)
        {
            //if (rre.RemarkFlag == 2 && rre.ReportPatient.ClinicName == ClinicType.clinic)
            //{
            //    rre.Remark = "带*为天津市临床检测中心认定的相互认可检验项目";
            //}
        }
        #endregion
    }
}
