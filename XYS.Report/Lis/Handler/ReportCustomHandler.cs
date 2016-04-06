using System;
using System.Collections.Generic;

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
        protected override HandlerResult OperateReport(ReportReportElement report)
        {
            ReportCustomElement rce = null;
            List<AbstractSubFillElement> customList = report.GetReportItem(typeof(ReportCustomElement));
            if (IsExist(customList))
            {
            }
            return new HandlerResult();
        }
        #endregion

        //可以添加一些custom项的内部处理逻辑
        #region 内部处理逻辑
        protected void ConvertCustom2KV(ReportCustomElement rce, Dictionary<string,string> kv)
        {

        }
        #endregion
    }
}
