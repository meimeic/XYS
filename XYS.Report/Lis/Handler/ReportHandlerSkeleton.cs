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
        private readonly string m_handlerName;
        #endregion

        #region 构造函数
        protected ReportHandlerSkeleton(string name)
        {
            this.m_handlerName = name;
        }
        #endregion

        #region 实现IReportHandler接口
        public IReportHandler Next
        {
            get { return this.m_nextHandler; }
            set { this.m_nextHandler = value; }
        }
        public virtual string HandlerName
        {
            get { return this.m_handlerName; }
        }
        public virtual HandlerResult ReportOptions(IReportElement report)
        {
            ReportReportElement rep = report as ReportReportElement;
            if (rep != null)
            {
                    return  OperateReport(rep);
            }
            return new HandlerResult(0,"this object is not a report!");
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
