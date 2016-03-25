using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Report.Lis;
using XYS.Report.Lis.Model;
using XYS.Report.Lis.Persistence;
using XYS.Report.Lis.Persistence.Mongo;

namespace XYS.Report.Lis.Core
{
    public class ReportImpl : IReport
    {
        #region
        private LisReportPKDAL m_reportKeyDAL;
        private readonly ILisReporter m_reporter;
        #endregion

        #region 构造函数
        public ReportImpl(ILisReporter reporter)
        {
            this.m_reporter = reporter;
            this.m_reportKeyDAL = new LisReportPKDAL();
        }
        #endregion

        #region 属性
        public ILisReporter Reporter
        {
            get { return this.m_reporter; }
        }
        public LisReportPKDAL ReportKeyDAL
        {
            get { return this.m_reportKeyDAL; }
            set { this.m_reportKeyDAL = value; }
        }
        #endregion

        #region 实现IReport接口
        public bool InitReport(ReportReportElement report, LisReportPK key)
        {
            this.Reporter.FillReport(report, key);
            return this.Reporter.OptionReport(report);
        }
        public bool InitReport(ReportReportElement report, Require require)
        {
            LisReportPK RK = GetReportKey(require);
            return InitReport(report, RK);
        }
        public bool InitReports(List<ReportReportElement> reportList, List<LisReportPK> keyList)
        {
            if (reportList != null)
            {
                reportList.Clear();
                bool result = false;
                ReportReportElement report = null;
                foreach (LisReportPK rk in keyList)
                {
                    report = new ReportReportElement();
                    this.Reporter.FillReport(report, rk);
                    result = this.Reporter.OptionReport(report);
                    if (result)
                    {
                        reportList.Add(report);
                    }
                }
                if (reportList.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public bool InitReports(List<ReportReportElement> reportList, Require require)
        {
            List<LisReportPK> keyList = GetReportKeyList(require);
            return InitReports(reportList, keyList);
        }
        #endregion

        #region 获取报告键
        protected virtual LisReportPK GetReportKey(Require require)
        {
            List<LisReportPK> result = this.ReportKeyDAL.GetReportKey(require);
            if (result.Count > 0)
            {
                return result[0];
            }
            return null;
        }
        protected virtual List<LisReportPK> GetReportKeyList(Require require)
        {
            return this.ReportKeyDAL.GetReportKey(require);
        }
        #endregion

        public void InsertToMongo(ReportReportElement report)
        {
            MongoService.Insert(report);
        }
    }
}