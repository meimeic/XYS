using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Model;
using XYS.Report;
using XYS.Common;
using XYS.Report.DAL;
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
        public bool InitReports(List<IReportElement> reportList, Type reportType, List<ReportPK> keyList)
        {
            if (reportList != null)
            {
                IReportElement report = null;
                foreach (ReportPK rk in keyList)
                {
                    try
                    {
                        report = (IReportElement)reportType.Assembly.CreateInstance(reportType.FullName);
                        this.Reporter.FillReport(report, rk);
                        reportList.Add(report);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                return this.Reporter.OptionReport(reportList);
            }
            return false;
        }
        public bool InitReports(List<IReportElement> exportList, Type reportType, Require require)
        {
            List<ReportPK> keyList = GetReportKeyList(require);
            return InitReports(exportList, reportType, keyList);
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