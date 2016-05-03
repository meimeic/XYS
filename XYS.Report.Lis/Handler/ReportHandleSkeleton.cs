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

        #region 私有事件
        private event HandleErrorHandler m_handleErrorEvent;
        private event HandleCompletedHandler m_handleCompletedEvent;
        #endregion

        #region 构造函数
        protected ReportHandleSkeleton()
        {
        }
        #endregion

        #region 实现IReportHandler接口
        public event HandleErrorHandler HandleErrorEvent
        {
            add { this.m_handleErrorEvent += value; }
            remove { this.m_handleErrorEvent -= value; }
        }
        public event HandleCompletedHandler HandleCompletedEvent
        {
            add { this.m_handleCompletedEvent += value; }
            remove { this.m_handleCompletedEvent -= value; }
        }

        public IReportHandle Next
        {
            get { return this.m_nextHandle; }
            set { this.m_nextHandle = value; }
        }
        public virtual void ReportOption(IReportElement report)
        {
            ReportReportElement rep = report as ReportReportElement;
            if (rep != null)
            {
                OperateReport(rep);
                //错误就退出
                if (rep.HandleResult.ResultCode < 0)
                {
                    this.OnErrorOccured(report);
                    LOG.Error("处理报告遇到错误，后续处理已终止！");
                    return;
                }
                //
                if (this.Next != null)
                {
                    LOG.Info("当前处理成功，继续处理。");
                    this.Next.ReportOption(report);
                }
                else
                {
                    this.OnHandleCompleted(report);
                    LOG.Info("处理报告完成。");
                    return;
                }
            }
            else
            {
                this.SetHandlerResult(report.HandleResult, -11, this.GetType(), "this object is not a lis report!");
                LOG.Error("当前要处理的内容不是lis报告！");
                this.OnErrorOccured(report);
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

        #region 触发事件
        protected void OnErrorOccured(IReportElement report)
        {
            HandleErrorHandler handler = this.m_handleErrorEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        protected void OnHandleCompleted(IReportElement report)
        {
            HandleCompletedHandler handler = this.m_handleCompletedEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        #endregion
    }
}
