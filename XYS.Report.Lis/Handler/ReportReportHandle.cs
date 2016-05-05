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
        public override void ReportHandle(ReportReportElement report)
        {
            LOG.Info("开始报告整体处理");

            //reportitem排序
            LOG.Info("报告项排序");
            report.ReportItemCollection.Sort();

            //
            LOG.Info("设置报告ID以及报告状态");
            report.ReportID = report.ReportPK.ID;
            report.ActiveFlag = 1;

            this.OnHandleSuccess(report);
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

        #region 触发事件
        #endregion
    }
}
