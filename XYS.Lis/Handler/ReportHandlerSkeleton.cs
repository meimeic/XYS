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
        public abstract HandlerResult ReportOptions(ILisReportElement reportElement);
        public abstract HandlerResult ReportOptions(List<ILisReportElement> reportElementList, ReportElementTag elementTag);

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

        #region
        protected abstract void OperateElement(ILisReportElement element,ReportElementTag elementTag);
        #endregion

        #region
        protected virtual void OperateElementList(List<ILisReportElement> reportElementList, ReportElementTag elementTag)
        {
            bool result;
            ILisReportElement reportElement;
            for (int i = reportElementList.Count - 1; i >= 0; i--)
            {
                reportElement = reportElementList[i] as ILisReportElement;
                if (reportElement == null)
                {
                    reportElementList.RemoveAt(i);
                }
                else
                {
                    result = IsElement(reportElement, elementTag);
                    if (result)
                    {
                        OperateElement(reportElement, elementTag);
                    }
                    else
                    {
                        reportElementList.RemoveAt(i);
                    }
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
