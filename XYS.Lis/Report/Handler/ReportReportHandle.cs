using System;

using XYS.Util;
using XYS.Lis.Report.Model;
namespace XYS.Lis.Report.Handler
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
        protected override void InnerHandle(ReportReportElement report)
        {
            LOG.Info("报告整体处理开始");

            //reportitem排序
            LOG.Info("报告项排序");
            report.ReportItemCollection.Sort();

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
