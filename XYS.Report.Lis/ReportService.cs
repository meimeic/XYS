using System;
using System.Collections.Generic;

using XYS.Report.Lis.Model;
using XYS.Report.Lis.Handler;
using XYS.Report.Lis.Persistent;
namespace XYS.Report.Lis
{
    public class ReportService
    {
        #region 静态只读字段
        private static readonly ReportService ServiceInstance;
        #endregion

        #region
        private  ReportPKService m_PKService;
        private  ReportHandleService m_handleService;
        private ReportMongoService m_mongoService;
        #endregion

        #region
        static ReportService()
        {
            ServiceInstance = new ReportService();
        }
        private ReportService()
        {
            this.Init();
        }
        #endregion

        #region
        public static ReportService LisService
        {
            get { return ServiceInstance; }
        }
        #endregion

        #region
        protected ReportPKService PKService
        {
            get { return this.m_PKService; }
        }
        protected ReportHandleService HandleService
        {
            get { return this.m_handleService; }
        }
        protected ReportMongoService MongoService
        {
            get { return this.m_mongoService; }
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
            this.HandleService.HandleReport(report);
        }
        public void SaveReport(ReportReportElement report)
        {
            this.MongoService.InsertReport(report);
        }
        public void SaveReportCurrently(ReportReportElement report)
        {
            this.MongoService.InsertReportCurrently(report);
        }
        public void InitThenSave(ReportReportElement report)
        {
            this.InitReport(report);
            this.SaveReport(report);
        }
        public void InitThenSaveCurrently(ReportReportElement report)
        {
            this.InitReport(report);
            this.SaveReportCurrently(report);
        }
        #endregion

        #region
        private void Init()
        {
            this.m_PKService = new ReportPKService();
            this.m_handleService = new ReportHandleService();
            this.m_mongoService = new ReportMongoService();
        }
        #endregion

    }
}
