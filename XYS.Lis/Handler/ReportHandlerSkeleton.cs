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
            if (name != null)
            {
                this.m_handlerName = name.ToLower();
            }
        }
        #endregion

        #region 实现IReportHandler接口
        public abstract HandlerResult ReportOptions(ILisReportElement reportElement);
        //public abstract HandlerResult ReportOptions(Hashtable reportElementTable, ReportElementTag elementTag);
        public abstract HandlerResult ReportOptions(List<ILisReportElement> reportElementList, ReportElementTag elementTag);

        public IReportHandler Next
        {
            get { return this.m_nextHandler; }
            set { this.m_nextHandler = value; }
        }
        public virtual string HandlerName
        {
            get { return this.m_handlerName; }
        }
        #endregion

        #region
        protected abstract bool IsElementAndOperate(ILisReportElement reportElement, ReportElementTag elementTag);
        protected abstract void OperateReport(ReportReportElement rre);
        #endregion

        #region
        protected virtual void OperateElementList(List<ILisReportElement> reportElementList, ReportElementTag elementTag)
        {
            bool result;
            ILisReportElement reportElement;
            for (int i = reportElementList.Count - 1; i >= 0; i++)
            {
                reportElement = reportElementList[i] as ILisReportElement;
                if (reportElement == null)
                {
                    reportElementList.RemoveAt(i);
                }
                else
                {
                    result = IsElementAndOperate(reportElement, elementTag);
                    if (!result)
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
                case ReportElementTag.ExamElement:
                    result = reportElement is ReportExamElement;
                    break;
                case ReportElementTag.PatientElement:
                    result = reportElement is ReportPatientElement;
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

        #region
        //protected virtual void OperateReportTable(Hashtable reportTable)
        //{
        //    if (reportTable.Count > 0)
        //    {
        //        ReportReportElement rre;
        //        foreach (object reportElement in reportTable.Values)
        //        {
        //            rre = reportElement as ReportReportElement;
        //            if (rre != null)
        //            {
        //                OperateReport(rre);
        //            }
        //        }
        //    }
        //}
        //protected virtual void OperateReportList(List<ILisReportElement> reportElementList)
        //{
        //    if (reportElementList.Count > 0)
        //    {
        //        ReportReportElement rre;
        //        foreach (ILisReportElement reportElement in reportElementList)
        //        {
        //            rre = reportElement as ReportReportElement;
        //            if (rre != null)
        //            {
        //                OperateReport(rre);
        //            }
        //        }
        //    }
        //}
        #endregion
    }
}
