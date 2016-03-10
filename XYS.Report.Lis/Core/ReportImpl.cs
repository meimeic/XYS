using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Model;
using XYS.Common;

using XYS.Report.Lis;
using XYS.Report.Lis.DAL;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Core
{
    public class ReportImpl : IReport
    {
        #region
        private LisReportPKDAL m_reportKeyDAL;
        private readonly IReporter m_reporter;
        #endregion

        #region 构造函数
        public ReportImpl(IReporter reporter)
        {
            this.m_reporter = reporter;
            this.m_reportKeyDAL = new LisReportPKDAL();
        }
        #endregion

        #region 属性
        public IReporter Reporter
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
        public bool InitReport(IReportElement report, ReportPK key)
        {
            this.Reporter.FillReport(report, key);
            return this.Reporter.OptionReport(report);
        }
        public bool InitReport(IReportElement report, Require require)
        {
            ReportPK RK = GetReportKey(require);
            return InitReport(report, RK);
        }
        public bool InitReports(List<IReportElement> reportList, List<ReportPK> keyList)
        {
            if (reportList != null)
            {
                bool result = false;
                ReportReportElement report = null;
                foreach (ReportPK rk in keyList)
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
        public bool InitReports(List<IReportElement> reportList, Require require)
        {
            List<ReportPK> keyList = GetReportKeyList(require);
            return InitReports(reportList, keyList);
        }
        #endregion

        #region 获取报告键
        protected virtual List<ReportPK> GetReportKeyList(Require require)
        {
            return this.ReportKeyDAL.GetReportKey(require);
        }
        protected virtual ReportPK GetReportKey(Require require)
        {
            List<ReportPK> result = this.ReportKeyDAL.GetReportKey(require);
            if (result.Count > 0)
            {
                return result[0];
            }
            return null;
        }
        #endregion
    }
}