using System;
using System.Collections.Generic;

using XYS.Model;

using XYS.Report.Lis.Core;
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

        public virtual HandlerResult ReportOptions(IReportElement reportElement)
        {
            bool result = false;
            if (IsReport(reportElement))
            {
                result = OperateReport((ReportReportElement)reportElement);
            }
            else
            {
                result = OperateElement(reportElement);
            }
            //
            if (result)
            {
                return HandlerResult.Continue;
            }
            return HandlerResult.Fail;
        }
        public virtual HandlerResult ReportOptions(List<IReportElement> reportElementList)
        {
            if (IsExist(reportElementList))
            {
                OperateElementList(reportElementList);
                if (IsExist(reportElementList))
                {
                    return HandlerResult.Continue;
                }
            }
            return HandlerResult.Fail;
        }
        #endregion

        #region 抽象方法(处理元素)
        protected abstract bool OperateElement(IReportElement element);
        protected abstract bool OperateReport(ReportReportElement report);
        #endregion

        #region 受保护的虚方法
        protected virtual void OperateElementList(List<IReportElement> reportElementList)
        {
            bool result = false;
            if (IsExist(reportElementList))
            {
                for (int i = reportElementList.Count - 1; i >= 0; i--)
                {
                    if (IsReport(reportElementList[i]))
                    {
                        result = OperateReport((ReportReportElement)reportElementList[i]);
                    }
                    else
                    {
                        result = OperateElement(reportElementList[i]);
                    }
                    //是否删除元素
                    if (!result)
                    {
                        reportElementList.RemoveAt(i);
                    }
                }
            }
        }
        #endregion

        #region 辅助方法
        protected bool IsExist(List<IReportElement> elementList)
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
        protected bool IsReport(IReportElement reportElement)
        {
            return reportElement is ReportReportElement;
        }
        protected bool IsElement(IReportElement reportElement, Type type)
        {
            return reportElement.GetType().Equals(type);
        }
        #endregion
    }
}
