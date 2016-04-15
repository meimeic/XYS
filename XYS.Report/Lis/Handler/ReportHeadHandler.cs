﻿using System;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public class ReportHeadHandler : ReportHandlerSkeleton
    {
        #region 构造函数
        public ReportHeadHandler()
            : base()
        {
        }
        #endregion

        #region 实现父类抽象方法
        protected override void OperateReport(ReportReportElement report, HandlerResult result)
        {
            this.SetHandlerResult(result, 0, "the ReportHeadHandler do nothing just only the head,so continue!");
            return;
        }
        #endregion
    }
}