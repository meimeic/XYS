using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;
using XYS.Report;
using XYS.Report.DAL;
using XYS.Report.Model;
namespace XYS.Report.Core
{
    public class ReportImpl : ReporterWrapperImpl, IReport
    {
        #region
        private LisReportPKDAL m_reportKeyDAL;
        #endregion

        #region 构造函数
        public ReportImpl(IReporter reporter)
            : base(reporter)
        {
            this.m_reportKeyDAL = new LisReportPKDAL();
        }
        #endregion

        #region 属性
        public LisReportPKDAL ReportKeyDAL
        {
            get { return this.m_reportKeyDAL; }
            set { this.m_reportKeyDAL = value; }
        }
        #endregion

        #region 实现IReport接口
        public bool InitReport(ReportReportElement report, ReporterKey key)
        {
            this.Reporter.FillReport(report, key);
            return this.Reporter.OptionReport(report);
        }
        public bool InitReport(ReportReportElement report, LisRequire require)
        {
            ReporterKey RK = GetReportKey(require);
            return InitReport(report, RK);
        }
        public bool InitReports(List<ILisReportElement> reportList, List<ReporterKey> keyList)
        {
            if (reportList != null)
            {
                ReportReportElement rre = null;
                foreach (ReporterKey rk in keyList)
                {
                    rre = new ReportReportElement();
                    this.Reporter.FillReport(rre, rk);
                    reportList.Add(rre);
                }
                return this.Reporter.OptionReport(reportList);
            }
            return false;
        }

        public bool InitReports(List<ILisReportElement> exportList, LisRequire require)
        {
            List<ReporterKey> keyList = GetReportKeyList(require);
            return InitReports(exportList, keyList);
        }
        #endregion

        #region 获取报告键
        protected virtual List<ReporterKey> GetReportKeyList(LisRequire require)
        {
            return this.ReportKeyDAL.GetReportKey(require);
        }
        protected virtual ReporterKey GetReportKey(LisRequire require)
        {
            List<ReporterKey> result = this.ReportKeyDAL.GetReportKey(require);
            if (result.Count > 0)
            {
                return result[0];
            }
            return null;
        }
        #endregion
    }
}