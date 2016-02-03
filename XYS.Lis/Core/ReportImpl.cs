using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Common;
using XYS.Model;
using XYS.Lis;
using XYS.Lis.Model;
using XYS.Lis.DAL;

namespace XYS.Lis.Core
{
    public class ReportImpl : ReporterWrapperImpl, IReport
    {
        #region
        private LisReporterKeyDAL m_reportKeyDAL;
        #endregion

        #region
        public ReportImpl(IReporter reporter)
            : base(reporter)
        {
            this.m_reportKeyDAL = new LisReporterKeyDAL();
        }
        #endregion
        public LisReporterKeyDAL ReportKeyDAL
        {
            get { return this.m_reportKeyDAL; }
            set { this.m_reportKeyDAL = value; }
        }
        #region
         
        #endregion

        #region 实现IReport接口
        public void InitReport(ReportKey key, ReportReportElement report)
        {
            this.Reporter.FillReportElement(report, key);
        }
        public void InitReport(LisSearchRequire require, ReportReportElement report)
        {
            ReportKey key = GetReportKey(require);
            this.InitReport(key, report);
        }

        public void InitReports(LisSearchRequire require, List<ILisReportElement> reportList)
        {
            List<ReportKey> keyList = GetReportKeyList(require);
            this.InitReports(keyList, reportList);
        }
        public void InitReports(List<ReportKey> keyList, List<ILisReportElement> reportList)
        {
            ReportReportElement rre;
            if (keyList != null && keyList.Count > 0)
            {
                foreach (ReportKey key in keyList)
                {
                    rre = new ReportReportElement();
                    InitReport(key, rre);
                    reportList.Add(rre);
                }
            }
        }

        public bool OperateReport(ReportReportElement report)
        {
            return this.Reporter.Option(report);
        }

        public bool OperateReport(List<ILisReportElement> reportList)
        {
            return this.Reporter.Option(reportList,ReportElementTag.Report);
        }

        public void HandleReport(LisSearchRequire require, ReportReportElement report)
        {
            this.InitReport(require, report);
            this.OperateReport(report);
        }

        public void HandleReports(List<ReportKey> keyList, List<ILisReportElement> reportList)
        {
            this.InitReports(keyList, reportList);
            this.OperateReport(reportList);
        }

        public void HandleReports(LisSearchRequire require, List<ILisReportElement> reportList)
        {
            List<ReportKey> keyList = GetReportKeyList(require);
            this.HandleReports(keyList, reportList);
        }
        #endregion
       
        #region
        protected virtual List<ReportKey> GetReportKeyList(LisSearchRequire require)
        {
            return this.ReportKeyDAL.GetReportKey(require);
        }
        protected virtual ReportKey GetReportKey(LisSearchRequire require)
        {
            List<ReportKey> result = this.ReportKeyDAL.GetReportKey(require);
            if (result.Count > 0)
            {
                return result[0];
            }
            return null;
        }
        
        #endregion

    }
}