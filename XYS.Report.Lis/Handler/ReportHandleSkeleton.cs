using System;
using System.Collections.Generic;

using log4net;

using XYS.Report;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public abstract class ReportHandleSkeleton : ILisReportHandle
    {
        #region 静态字段
        protected static ILog LOG = LogManager.GetLogger("LisReportHandle");
        #endregion

        #region 私有字段
        private IReportHandle m_nextHandle;
        #endregion

        #region 构造函数
        protected ReportHandleSkeleton()
        {
        }
        #endregion

        #region 实现IReportHandler接口
        public IReportHandle Next
        {
            get { return this.m_nextHandle; }
            set { this.m_nextHandle = value; }
        }
        public void ReportOption(IReportElement report)
        {
            OperateReport((ReportReportElement)report);
            //错误就退出
            if (report.HandleResult.ResultCode < 0)
            {
                LOG.Error("处理报告遇到错误，后续处理已终止！");
                return;
            }
            //
            if (this.Next != null)
            {
                LOG.Info("当前处理成功，继续处理。");
                this.Next.ReportOption(report);
            }
        }
        #endregion

        #region 抽象方法(处理元素)
        protected abstract void OperateReport(ReportReportElement report);
        #endregion

        #region 辅助方法
        protected bool IsExist(List<IFillElement> elementList)
        {
            if (elementList != null && elementList.Count > 0)
            {
                return true;
            }
            return false;
        }
        protected bool IsReport(Type type)
        {
            return typeof(ReportReportElement).Equals(type);
        }
        protected bool IsReport(IReportElement report)
        {
            return report is ReportReportElement;
        }
        protected void SetHandlerResult(HandleResult result, int code, string message)
        {
            result.ResultCode = code;
            result.Message = message;
        }
        protected void SetHandlerResult(HandleResult result, int code, Type type, string message)
        {
            SetHandlerResult(result, code, message);
            result.HandleType = type;
        }
        #endregion
    }
}
