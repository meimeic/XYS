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
        public IReportHandler Next
        {
            get { return this.m_nextHandler; }
            set { this.m_nextHandler = value; }
        }
        public virtual string HandlerName
        {
            get { return this.m_handlerName; }
        }
        public virtual HandlerResult ReportOptions(ILisReportElement reportElement)
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
        //public virtual HandlerResult ReportOptions(List<ILisReportElement> reportElementList)
        //{

        //    if (IsExist(reportElementList))
        //    {
        //        bool result = false;
        //        for (int i = reportElementList.Count - 1; i >= 0; i--)
        //        {
        //            result = OperateReport(reportElementList[i]);
        //            if (!result)
        //            {
        //                reportElementList.RemoveAt(i);
        //            }
        //        }
        //    }
        //    if (IsExist(reportElementList))
        //    {
        //        return HandlerResult.Continue;
        //    }
        //    return HandlerResult.Fail;
        //}
        public virtual HandlerResult ReportOptions(List<ILisReportElement> reportElementList, Type type)
        {
            if (IsExist(reportElementList))
            {
                if (IsReport(type))
                {
                    bool flag = false;
                    bool result = false;
                    for (int i = reportElementList.Count - 1; i >= 0; i--)
                    {
                        flag = IsReport(reportElementList[i]);
                        if (flag)
                        {
                            result = OperateReport((ReportReportElement)reportElementList[i]);
                        }
                        if (!flag || !result)
                        {
                            reportElementList.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    OperateElementList(reportElementList, type);
                }
                if (reportElementList.Count > 0)
                {
                    return HandlerResult.Continue;
                }
            }
            return HandlerResult.Fail;
        }
        #endregion

        #region 抽象方法(处理元素)
        protected abstract bool OperateElement(ILisReportElement element);
        protected abstract bool OperateReport(ReportReportElement report);
        #endregion

        #region 受保护的虚方法
        protected virtual void OperateElementList(List<ILisReportElement> reportElementList)
        {
            bool result = false;
            if (IsExist(reportElementList))
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
        protected virtual void OperateElementList(List<ILisReportElement> reportElementList, Type type)
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
        #endregion

        #region 辅助方法
        protected bool IsExist(List<ILisReportElement> elementList)
        {
            if (elementList != null && elementList.Count > 0)
            {
                return true;
            }
            return false;
        }
        private bool IsReport(Type type)
        {
            return typeof(ReportReportElement).Equals(type);
        }
        private bool IsReport(ILisReportElement reportElement)
        {
            return IsElement(reportElement, typeof(ReportReportElement));
        }
        private bool IsElement(ILisReportElement reportElement, Type type)
        {
            return reportElement.GetType().Equals(type);
        }
        #endregion
    }
}
