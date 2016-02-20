using System;
using System.Collections.Generic;

using XYS.Lis.Model;
using XYS.Lis.Core;
namespace XYS.Lis.Handler
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
        public virtual HandlerResult ReportOptions(IReportElement reportElement)
        {
            bool result = OperateElement(reportElement);
            if (result)
            {
                return HandlerResult.Continue;
            }
            return HandlerResult.Fail;
        }
        public virtual HandlerResult ReportOptions(List<IReportElement> reportElementList)
        {
            OperateElementList(reportElementList);
            if (reportElementList.Count > 0)
            {
                return HandlerResult.Continue;
            }
            else
            {
                return HandlerResult.Fail;
            }
        }
        public virtual HandlerResult ReportOptions(List<IReportElement> reportElementList, Type type)
        {
            OperateElementList(reportElementList, type);
            if (reportElementList.Count > 0)
            {
                return HandlerResult.Continue;
            }
            else
            {
                return HandlerResult.Fail;
            }
        }
        
        public IReportHandler Next
        {
            get { return this.m_nextHandler; }
            set { this.m_nextHandler = value; }
        }
        public virtual string HandlerName
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_handlerName))
                {
                    return null;
                }
                else
                {
                    return this.m_handlerName.ToLower();
                }
            }
        }
        #endregion

        #region 抽象方法(处理元素)
        protected abstract bool OperateElement(IReportElement element);
        #endregion

        #region 受保护的虚方法
        protected virtual void OperateElementList(List<IReportElement> reportElementList)
        {
            bool result = false;
            if (reportElementList.Count > 0)
            {
                for (int i = reportElementList.Count - 1; i >= 0; i--)
                {
                    result = OperateElement(reportElementList[i]);
                    if (!result)
                    {
                        reportElementList.RemoveAt(i);
                    }
                }
            }
        }
        protected virtual void OperateElementList(List<IReportElement> reportElementList, Type type)
        {
            bool flag = false;
            bool result = false;
            if (reportElementList.Count > 0)
            {
                for (int i = reportElementList.Count - 1; i >= 0; i--)
                {
                    flag = IsElement(reportElementList[i], type);
                    if (flag)
                    {
                        result = OperateElement(reportElementList[i]);
                    }
                    if (!flag || !result)
                    {
                        reportElementList.RemoveAt(i);
                    }
                }
            }
        }
       
        protected virtual bool IsElement(IReportElement reportElement, ReportElementTag elementTag)
        {
            bool result = false;
            switch (elementTag)
            {
                case ReportElementTag.Report:
                    result = reportElement is ReportReportElement;
                    break;
                case ReportElementTag.Exam:
                    result = reportElement is ReportExamElement;
                    break;
                case ReportElementTag.Patient:
                    result = reportElement is ReportPatientElement;
                    break;
                case ReportElementTag.Item:
                    result = reportElement is ReportItemElement;
                    break;
                case ReportElementTag.Graph:
                    result = reportElement is ReportGraphElement;
                    break;
                case ReportElementTag.Custom:
                    result = reportElement is ReportCustomElement;
                    break;
                case ReportElementTag.Temp:
                    result = reportElement is ReportKVElement;
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }
        protected virtual bool IsElement(IReportElement reportElement, Type type)
        {
            return reportElement.GetType().Equals(type);
        }
        #endregion
    }
}
