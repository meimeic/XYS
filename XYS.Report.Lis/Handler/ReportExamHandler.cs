using System;
using System.Collections.Generic;

using XYS.Util;

using XYS.Report.Lis.Core;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public class ReportExamHandler : ReportHandlerSkeleton
    {
        #region 静态变量
        public static readonly string m_defaultHandlerName = "ReportExamHandler";
        #endregion

        #region 构造函数
        public ReportExamHandler()
            : this(m_defaultHandlerName)
        {
        }
        public ReportExamHandler(string name)
            : base(name)
        {
        }
        #endregion
        
        #region 实现父类抽象方法
        protected override bool OperateElement(ILisReportElement element)
        {
            //元素级别处理
            ReportExamElement ree = element as ReportExamElement;
            if (ree != null)
            {
                //处理代码
                if (ree.SectionNo == 10)
                {
                    if (ree.FormMemo != null)
                    {
                        ree.FormMemo = ree.FormMemo.Replace(";", SystemInfo.NewLine);
                    }
                }
                return true;
            }
            return false;
        }
        protected override bool OperateReport(ReportReportElement report)
        {
            //顶级处理
            return OperateElement(report.ReportExam);
        }
        #endregion

        #region 内部处理逻辑
     
        #endregion
    }
}
