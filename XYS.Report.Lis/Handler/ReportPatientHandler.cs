using System;
using System.Collections.Generic;

using XYS.Model;
using XYS.Report.Model.Lis;
namespace XYS.Report.Lis.Handler
{
    public class ReportPatientHandler : ReportHandlerSkeleton
    {
        #region 静态变量
        public static readonly string m_defaultHandlerName = "ReportPatientHandler";
        #endregion

        #region 构造函数
        public ReportPatientHandler()
            : this(m_defaultHandlerName)
        { }
        public ReportPatientHandler(string name)
            : base(name)
        { }
        #endregion

        #region 实现父类抽象方法
        protected override bool OperateElement(IReportElement element)
        {
            //元素级操作
            ReportPatientElement rpe = element as ReportPatientElement;
            if (rpe != null)
            {
                if (rpe.CID != null)
                {
                    rpe.CID.Trim();
                }
                return true;
            }
            return false;
        }
        protected override bool OperateReport(ReportReportElement rre)
        {
            //报告级操作
            return OperatePatient(rre);
        }
        #endregion

        #region
        protected virtual bool OperatePatient(ReportReportElement rre)
        {
            //中间级操作
            return OperateElement(rre.ReportPatient);
        }
        #endregion
    }
}
