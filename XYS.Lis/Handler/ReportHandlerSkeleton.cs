using System.Collections;
using System.Collections.Generic;

using XYS.Model;
using XYS.Lis.Core;
using XYS.Lis.Model;
namespace XYS.Lis.Handler
{
    public abstract class ReportHandlerSkeleton : IReportHandler
    {
        #region 私有字段
        private IReportHandler m_nextHandler;
        private readonly string m_handlerName;
        #endregion

        #region
        protected ReportHandlerSkeleton(string name)
        {
            this.m_handlerName = name;
        }
        #endregion

        #region 实现IReportHandler接口
        public virtual HandlerResult ReportOptions(ILisReportElement reportElement)
        {
            bool flag = IsElement(reportElement, reportElement.ElementTag);
            bool result = OperateElement(reportElement, reportElement.ElementTag);
            if (flag && result)
            {
                return HandlerResult.Continue;
            }
            return HandlerResult.Fail;
        }
        public virtual HandlerResult ReportOptions(List<ILisReportElement> reportElementList, ReportElementTag elementTag)
        {
            if (elementTag != ReportElementTag.NoneElement)
            {
                OperateElementList(reportElementList, elementTag);
                if (reportElementList.Count > 0)
                {
                    return HandlerResult.Continue;
                }
                else
                {
                    return HandlerResult.Fail;
                }
            }
            return HandlerResult.Fail;
        }
        public IReportHandler Next
        {
            get { return this.m_nextHandler; }
            set { this.m_nextHandler = value; }
        }
        public virtual string HandlerName
        {
            get { return this.m_handlerName.ToLower(); }
        }
        #endregion

        #region 抽象方法(处理元素)
        protected abstract bool OperateElement(ILisReportElement element, ReportElementTag elementTag);
        #endregion

        #region 受保护的虚方法
        protected virtual void OperateElementList(List<ILisReportElement> reportElementList, ReportElementTag elementTag)
        {
            bool flag = false;
            bool result = false;
            for (int i = reportElementList.Count - 1; i >= 0; i--)
            {
                flag = IsElement(reportElementList[i], elementTag);
                if (flag)
                {
                    result = OperateElement(reportElementList[i], elementTag);
                }
                if (!flag || !result)
                {
                    reportElementList.RemoveAt(i);
                }
            }
        }
        protected virtual bool IsElement(ILisReportElement reportElement, ReportElementTag elementTag)
        {
            bool result = false;
            switch (elementTag)
            {
                case ReportElementTag.ReportElement:
                    result = reportElement is ReportReportElement;
                    break;
                case ReportElementTag.InfoElement:
                    result = reportElement is ReportInfoElement;
                    break;
                case ReportElementTag.ItemElement:
                    result = reportElement is ReportItemElement;
                    break;
                case ReportElementTag.GraphElement:
                    result = reportElement is ReportGraphElement;
                    break;
                case ReportElementTag.CustomElement:
                    result = reportElement is ReportCustomElement;
                    break;
                case ReportElementTag.KVElement:
                    result = reportElement is ReportKVElement;
                    break;
                case ReportElementTag.NoneElement:
                    result = false;
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }
        #endregion
    }
}
