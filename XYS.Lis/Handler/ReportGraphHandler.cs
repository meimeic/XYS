using System;
using System.Collections.Generic;
using XYS.Lis.Core;
using XYS.Model;
using XYS.Lis.Model;

namespace XYS.Lis.Handler
{
    public class ReportGraphHandler:ReportHandlerSkeleton
    {
        private static readonly string m_defaultHandlerName = "ReportGraphHandler";


        public ReportGraphHandler()
            : this(m_defaultHandlerName)
        {
        }
        public ReportGraphHandler(string handlerName)
            : base(handlerName)
        {
        }
        #region
        public override HandlerResult ReportOptions(ILisReportElement reportElement)
        {
            if (reportElement.ElementTag == ReportElementTag.ReportElement)
            {
                ReportReportElement rre = reportElement as ReportReportElement;
                if (rre != null)
                {
                    OperateReport(rre);
                }
                return HandlerResult.Continue;
            }
            else if (reportElement.ElementTag == ReportElementTag.GraphElement)
            {
                //处理代码
                ReportGraphElement rgie = reportElement as ReportGraphElement;
                OperateGraphItem(rgie);
                return HandlerResult.Continue;
            }
            else
            {
                return HandlerResult.Continue;
            }
        }
        public override HandlerResult ReportOptions(List<ILisReportElement> reportElementList, ReportElementTag elementTag)
        {
            if (elementTag == ReportElementTag.ReportElement)
            {
                OperateReportList(reportElementList);
                return HandlerResult.Continue;
            }
            else if (elementTag == ReportElementTag.GraphElement)
            {
                OperateGraphItems(reportElementList);
                return HandlerResult.Continue;
            }
            else
            {
                return HandlerResult.Continue;
            }
        }
        #endregion

        #region
        protected override void OperateReport(ReportReportElement rre)
        {
            OperateGraphItems(rre.GraphItemList);
        }
        #endregion
        #region
        protected virtual void OperateGraphItems(List<ILisReportElement> graphList)
        {
            if (graphList.Count > 0)
            {
                //
                ReportGraphElement rge;
                for (int i = graphList.Count - 1; i >= 0; i--)
                {
                    rge = graphList[i] as ReportGraphElement;
                    if (rge != null)
                    {
                        OperateGraphItem(rge);
                    }
                }
            }
        }
        protected virtual void OperateGraphItem(ReportGraphElement rge)
        {

        }
        #endregion
    }
}
