using System;
using System.Collections.Generic;

using XYS.Model;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public class ReportCustomHandler : ReportHandlerSkeleton
    {
        #region 只读字段
        private static readonly string m_defaultHandlerName = "ReportCustomHandler";
        #endregion

        #region 构造函数
        public ReportCustomHandler()
            : this(m_defaultHandlerName)
        {
        }
        public ReportCustomHandler(string handlerName)
            : base(handlerName)
        {
        }
        #endregion

        #region 实现父类抽象方法
        protected override bool OperateElement(IReportElement element)
        {
            ReportCustomElement rce = element as ReportCustomElement;
            if (rce != null)
            {
                //ReportCustomElement 处理
                return true;
            }
            return false;
        }
        protected override bool OperateReport(ReportReportElement report)
        {
            //
            return OperateCustom(report);
        }
        #endregion
        
        //可以添加一些custom项的内部处理逻辑
        #region 内部处理逻辑
        private bool OperateCustom(ReportReportElement rre)
        {
            return true;
        }
        #endregion
    }
}
