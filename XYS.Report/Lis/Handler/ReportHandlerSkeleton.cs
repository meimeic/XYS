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
        public virtual HandlerResult ReportOption(IReportElement report)
        {
            ReportReportElement rep = report as ReportReportElement;
            if (rep != null)
            {
                HandlerResult result = OperateReport(rep);
                if (result.Code == -1)
                {
                    return result;
                }

                if (this.Next != null)
                {
                    return this.Next.ReportOption(report);
                }
                else
                {
                    return new HandlerResult(1, this.GetType(), "this handlers have been fully successfully executed ", rep.PK);
                }
            }
            else
            {
                return new HandlerResult(-1, this.GetType(), "this object is not a report!");
            }
        }
        #endregion

        #region 抽象方法(处理元素)
        protected abstract HandlerResult OperateReport(ReportReportElement report);
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
        #endregion
    }
}
