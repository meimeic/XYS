using System;
using System.Collections.Generic;

using XYS.Report;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public abstract class ReportHandleSkeleton : IReportHandle
    {
        #region 私有字段
        private IReportHandle m_nextHandler;
        #endregion

        #region 构造函数
        protected ReportHandleSkeleton()
        {
        }
        #endregion

        #region 实现IReportHandler接口
        public IReportHandle Next
        {
            get { return this.m_nextHandler; }
            set { this.m_nextHandler = value; }
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
                    return;
                }
                //
                if (this.Next != null)
                {
                    this.Next.ReportOption(report);
                }
                else
                {
                    this.SetHandlerResult(rep.HandleResult, 10, "handle report successfully complete!");
                }
            }
            else
            {
                this.SetHandlerResult(report.HandleResult, -11, this.GetType(), "this object is not a lis report!");
            }
        }
        #endregion

        #region 抽象方法(处理元素)
        protected abstract void OperateReport(ReportReportElement report);
        #endregion

        #region 辅助方法
        protected bool IsExist(List<AbstractFillElement> elementList)
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
