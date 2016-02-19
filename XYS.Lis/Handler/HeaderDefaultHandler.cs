using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using XYS.Lis.Core;
using XYS.Model;
using XYS.Lis.Core;
using XYS.Lis.Util;
namespace XYS.Lis.Handler
{
    /// <summary>
    /// 通用处理，删除各个集合中错误类型对象
    /// </summary>
    public class HeaderDefaultHandler : ReportHandlerSkeleton
    {
        #region 静态变量
        public static readonly string m_defaultHandlerName = "HeaderDefaultHandler";
        #endregion

        #region 构造函数
        public HeaderDefaultHandler()
            : this(m_defaultHandlerName)
        {

        }
        public HeaderDefaultHandler(string handlerName)
            : base(handlerName)
        {

        }
        #endregion

        #region 实现父类受保护的抽象方法
        protected override bool OperateElement(ILisReportElement element, ReportElementTag elementTag)
        {
            return true;
        }
        #endregion
    }
}
