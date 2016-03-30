﻿using System;
using System.Collections.Generic;

using XYS.Report.Lis.Core;
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
        protected override bool OperateReport(ReportReportElement report)
        {
            ReportCustomElement rce = null;
            List<ILisReportElement> customList = report.GetReportItem(typeof(ReportCustomElement));
            if (IsExist(customList))
            {
                foreach (ILisReportElement custom in customList)
                {
                    rce = custom as ReportCustomElement;
                    if (rce != null)
                    {
                        ConvertCustom2KV(rce, report.ReportKVCollection);
                    }
                }
            }
            report.RemoveReportItem(typeof(ReportCustomElement));
            return true;
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
