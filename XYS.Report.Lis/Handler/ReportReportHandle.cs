using System;

using XYS.Util;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public class ReportReportHandle : ReportHandleSkeleton
    {
        #region 构造函数
        public ReportReportHandle()
            : base()
        {
        }
        #endregion

        #region 实现父类方法
        protected override void OperateReport(ReportReportElement report)
        {
            LOG.Info("报告基本处理");
            //formmemo 处理
            if (report.SectionNo == 10)
            {
                LOG.Info("备注信息处理");
                if (report.FormMemo != null)
                {
                    report.FormMemo = report.FormMemo.Replace(";", SystemInfo.NewLine);
                }
            }

            //cid 处理
            if (report.CID != null)
            {
                LOG.Info("身份证信息处理");
                report.CID = report.CID.Trim();
            }

            //reportitem排序
            LOG.Info("报告项排序");
            report.ReportItemCollection.Sort();

            //
            LOG.Info("设置报告ID以及报告状态");
            report.ReportID = report.ReportPK.ID;
            report.ActiveFlag = 1;

            //返回值
            this.SetHandlerResult(report.HandleResult, 70, "handle ReportReportElement successfully and continue!");
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
