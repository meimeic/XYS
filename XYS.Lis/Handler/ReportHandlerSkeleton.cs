using System.Collections.Generic;

using XYS.Model;
using XYS.Lis.Core;
using XYS.Lis.Model;
namespace XYS.Lis.Handler
{
    public abstract class ReportHandlerSkeleton:IReportHandler
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
            get { return this.m_handlerName; }
        }
        #endregion
        
        #region
        protected virtual void OperateReportList(List<ILisReportElement> reportElementList)
        {
            if (reportElementList.Count > 0)
            {
                ReportReportElement rre;
                foreach (ILisReportElement reportElement in reportElementList)
                {
                    rre = reportElement as ReportReportElement;
                    if (rre != null)
                    {
                        OperateReport(rre);
                    }
                }
            }
        }
        protected abstract void OperateReport(ReportReportElement rre);
        #endregion
    }
}
