using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Lis.Core;
using XYS.Model;
using XYS.Lis.Model;
using XYS.Lis.Util;

namespace XYS.Lis.Handler
{
    public class ReportGraphHandler:ReportHandlerSkeleton
    {
        private static readonly string m_defaultHandlerName = "ReportGraphHandler";
     
        #region
        public ReportGraphHandler()
            : this(m_defaultHandlerName)
        {
        }
        public ReportGraphHandler(string handlerName)
            : base(handlerName)
        {
        }
        #endregion

        #region 实现父类继承接口的抽象方法
        public override HandlerResult ReportOptions(ILisReportElement reportElement)
        {
            return HandlerResult.Continue;
        }
        public override HandlerResult ReportOptions(List<ILisReportElement> reportElementList, ReportElementTag elementTag)
        {
            if (elementTag == ReportElementTag.ReportElement || elementTag == ReportElementTag.GraphElement)
            {
                OperateElementList(reportElementList, elementTag);
                return HandlerResult.Continue;
            }
            else
            {
                return HandlerResult.Continue;
            }
        }
        #endregion

        #region 实现父类抽象方法
        protected override void OperateElement(ILisReportElement element, ReportElementTag elementTag)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region
        protected void OperateReport(ReportReportElement rre)
        {
            List<ILisReportElement> reportElementList = rre.GetReportItem(ReportElementTag.GraphElement);
            if (reportElementList.Count > 0)
            {
                OperateElementList(reportElementList, ReportElementTag.GraphElement);
            }
        }
        protected virtual void OperateGraph(ReportGraphElement rge)
        {
        }
        #endregion
    }
}
