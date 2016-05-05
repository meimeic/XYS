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

        #region 事件字段
        private event HandleReportError m_handleReportErrorEvent;
        private event HandleReportSuccess m_handleReportSuccessEvent;
        #endregion

        #region 构造函数
        protected ReportHandleSkeleton()
        {
        }
        #endregion

        #region 实现IReportHandler接口
        public event HandleReportError HandleReportErrorEvent
        {
            add { this.m_handleReportErrorEvent += value; }
            remove { this.m_handleReportErrorEvent -= value; }
        }
        public event HandleReportSuccess HandleReportSuccessEvent
        {
            add { this.m_handleReportSuccessEvent += value; }
            remove { this.m_handleReportSuccessEvent -= value; }
        }
        public abstract void ReportHandle(ReportReportElement report);
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
        protected void SetHandlerResult(HandleResult result, int code, Type type = null, Exception ex = null)
        {
            result.ResultCode = code;
            result.HandleType = type;
            result.Exception = ex;
        }
        #endregion

        #region 触发事件
        protected void OnhandleReportError(ReportReportElement report)
        {
            HandleReportError handler = this.m_handleReportErrorEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        protected void OnHandleReportSuccess(ReportReportElement report)
        {
            HandleReportSuccess handler = this.m_handleReportSuccessEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        #endregion
    }
}
