using System;
using System.Collections.Generic;

using XYS.Report;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public abstract class ReportHandlerSkeleton : IReportHandler
    {
        #region 私有字段
        private IReportHandler m_nextHandler;
        #endregion

        #region 构造函数
        protected ReportHandlerSkeleton()
        {
        }
        #endregion

        #region 实现IReportHandler接口
        public IReportHandler Next
        {
            get { return this.m_nextHandler; }
            set { this.m_nextHandler = value; }
        }
        public virtual void ReportOption(IReportElement report, HandlerResult result)
        {
            ReportReportElement rep = report as ReportReportElement;
            if (rep != null)
            {
                OperateReport(rep, result);
                //错误就退出
                if (result.Code == -1)
                {
                    return;
                }
                //
                if (this.Next != null)
                {
                    this.Next.ReportOption(report, result);
                }
                else
                {
                    return;
                }
            }
            else
            {
                this.SetHandlerResult(result, -1, this.GetType(), "this object is not a report!");
                return;
            }
        }
        #endregion

        #region 抽象方法(处理元素)
        protected abstract void OperateReport(ReportReportElement report, HandlerResult result);
        #endregion

        #region 辅助方法
        protected bool IsExist(List<AbstractSubFillElement> elementList)
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

        protected void SetHandlerResult(HandlerResult result, int code, string message)
        {
            result.Code = code;
            result.Message = message;
        }
        protected void SetHandlerResult(HandlerResult result, int code, Type type, string message)
        {
            SetHandlerResult(result, code, message);
            result.FinalType = type;
        }
        protected void SetHandlerResult(HandlerResult result, int code, Type type, string message, IReportKey key)
        {
            SetHandlerResult(result, code, type, message);
            result.ReportKey = key;
        }
        #endregion
    }
}
