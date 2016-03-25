using System;

using XYS.Util;
using XYS.Report.Lis.Util;
using XYS.Report.Lis.Core;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public class ReportReportHandler : ReportHandlerSkeleton
    {
        #region 变量
        private static readonly string m_defaultHandlerName = "ReportReportHandler";
        #endregion

        #region 构造函数
        public ReportReportHandler()
            : this(m_defaultHandlerName)
        {
        }
        public ReportReportHandler(string handlerName)
            : base(handlerName)
        {
        }
        #endregion

        #region 实现父类方法
        protected override bool OperateReport(ReportReportElement rre)
        {
            //formmemo 处理
            if (rre.SectionNo == 10)
            {
                if (rre.FormMemo != null)
                {
                    rre.FormMemo = rre.FormMemo.Replace(";", SystemInfo.NewLine);
                }
            }
           //cid 处理
            if (rre.CID != null)
            {
                rre.CID=rre.CID.Trim();
            }
            return true;
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

        #region

        #endregion
    }
}
