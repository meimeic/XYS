using System;
using System.Collections.Generic;

using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public class ReportCustomHandler : ReportHandlerSkeleton
    {
        #region 构造函数
        public ReportCustomHandler()
            : base()
        {
        }
        #endregion

        #region 实现父类抽象方法
        protected override void OperateReport(ReportReportElement report)
        {
            ReportCustomElement rce = null;
            List<AbstractFillElement> customList = report.GetReportItem(typeof(ReportCustomElement));
            if (IsExist(customList))
            {
                foreach (AbstractFillElement custom in customList)
                {
                    rce = custom as ReportCustomElement;
                    if (rce != null)
                    {
                        //custom项处理
                    }
                }
            }
            this.SetHandlerResult(report.HandleResult, 1, "there is no ReportCustomElement to handle and continue!");
        }
        #endregion

        #region custom内部处理逻辑
        #endregion
    }
}
