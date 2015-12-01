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
        #region 实例方法
        private LisReporterKeyDAL m_reportKeyDAL;
        #endregion

        #region
        public ReportImpl(ILisReporter reporter)
            : base(reporter)
        {
            this.m_reportKeyDAL = new LisReporterKeyDAL();
        }
        #endregion

        #region 实现IReport接口
        public void InitReport(ReportKey key, ReportReportElement report)
        {
            this.Reporter.FillReportElement(report,key);
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
            throw new NotImplementedException();
        }

        public bool OperateReport(ReportReportElement report)
        {
            throw new NotImplementedException();
        }

        public bool OperateReport(List<ILisReportElement> reportList)
        {
            throw new NotImplementedException();
        }

        public void HandleReport(LisSearchRequire require, ReportReportElement report)
        {
            throw new NotImplementedException();
        }

        public void HandleReports(List<ReportKey> keyList, List<ILisExportElement> reportList)
        {
            throw new NotImplementedException();
        }

        public void HandleReports(LisSearchRequire require, List<ILisExportElement> reportList)
        {
            throw new NotImplementedException();
        }
        #endregion
       
        #region
        protected virtual List<ReportKey> GetReportKeyList(LisSearchRequire require)
        {
            return this.m_reportKeyDAL.GetReportKey(require);
        }
        protected virtual ReportKey GetReportKey(LisSearchRequire require)
        {
            List<ReportKey> result = this.m_reportKeyDAL.GetReportKey(require);
            if (result.Count > 0)
            {
                return result[0];
            }
            return null;
        }
        
        #endregion

    }
}