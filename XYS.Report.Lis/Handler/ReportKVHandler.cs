using System;
using System.Collections.Generic;

using XYS.Model;
using XYS.Report.Model.Lis;
namespace XYS.Report.Lis.Handler
{
    public class ReportKVHandler : ReportHandlerSkeleton
    {
        #region 静态变量
        public static readonly string m_defaultHandlerName = "ReportKVHandler";
        #endregion

        #region 构造函数
        public ReportKVHandler()
            : this(m_defaultHandlerName)
        {
        }
        public ReportKVHandler(string handlerName)
            : base(handlerName)
        {
        }
        #endregion

        #region 实现父类抽象方法
        protected override bool OperateReport(ReportReportElement report)
        {
            return true;
        }
        protected override bool OperateElement(IReportElement element)
        {
            return true;
        }
        #endregion
    }
}
