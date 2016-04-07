using System;
using System.Collections.Generic;

using XYS.Report.Lis.Model;
using XYS.Report.Lis.Persistence;
namespace XYS.Report.Lis
{
    public class ReportImpl:ReportWrapper,IReport
    {
        #region 字段
        private LisReportPKDAL m_pkDAL;
        #endregion

        #region 构造函数
        public ReportImpl(IReporter reporter)
            : base(reporter)
        {
            this.m_pkDAL = new LisReportPKDAL();
        }
        #endregion

        #region 属性
        public LisReportPKDAL PKDAL
        {
            get { return this.m_pkDAL; }
        }
        #endregion

        #region 实现ireport接口
        public string operate(ReportReportElement report)
        {
            return this.Reporter.OperateReport(report);
        }

        public string operate(Require req, ReportReportElement report)
        {
            InitReportPK(req, report);
            return operate(report);
        }
        #endregion

        #region 私有方法
        private void InitReportPK(Require req,ReportReportElement report)
        {
            this.PKDAL.InitReportKey(req,report);
        }
        #endregion
    }
}
