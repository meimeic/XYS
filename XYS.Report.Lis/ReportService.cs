using System;
using System.Collections.Generic;

using XYS.Report.Lis.Model;
namespace XYS.Report.Lis
{
    public class ReportService
    {
        #region 静态只读字段
        private static readonly ReportService ServiceInstance;
        #endregion

        #region
        private readonly Reporter m_reporter;
        private readonly ReportPKService m_PKService;
        #endregion

        #region
        static ReportService()
        {
            ServiceInstance = new ReportService();
        }
        private ReportService()
        {
            this.m_reporter = new DefaultReporter();
            this.m_PKService = new ReportPKService();
        }
        #endregion

        #region
        public static ReportService LisService
        {
            get { return ServiceInstance; }
        }
        #endregion

        #region
        protected Reporter Reporter
        {
            get { return this.m_reporter; }
        }
        protected ReportPKService PKService
        {
            get { return this.m_PKService; }
        }
        #endregion

        #region 报告主键值获取
        public void InitReportPK(Require req, List<ReportPK> PKList)
        {
            this.PKService.InitReportPK(req,PKList);
        }
        public void InitReportPK(string where, List<ReportPK> PKList)
        {
            this.PKService.InitReportPK(where, PKList);
        }
        #endregion

        #region 报告处理
        public void InitReport(ReportReportElement report)
        {

        }
        public void SaveReport(ReportReportElement report)
        {

        }
        public void InitThenSave(ReportReportElement report)
        {

        }
        #endregion

    }
}
