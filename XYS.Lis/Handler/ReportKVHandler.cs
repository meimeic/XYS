using System;
using System.Collections.Generic;

using XYS.Model;
using XYS.Lis.Core;

namespace XYS.Lis.Handler
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
        protected override bool OperateElement(ILisReportElement element, ReportElementTag elementTag)
        {
            return true;
        }
        #endregion

        //添加KV项的内部处理逻辑

    }
}
