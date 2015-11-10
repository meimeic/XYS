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
        public ReportImpl(ILisReporter reporter)
            : base(reporter)
        {
            this.m_reportKeyDAL = new LisReporterKeyDAL();
        }
        #endregion

        #region 实现IReport接口
        public string ReportExport(ReportKey key)
        {
            if (key != null)
            {
                ILisReportElement report = new ReportReportElement();
                this.Reporter.FillReportElement(report, key);
                this.Reporter.Option(report);
                return this.Reporter.Export(report);
            }
            else
            {
                return null;
            }
        }
        public virtual string ReportExport(LisSearchRequire require)
        {
            ReportKey key = GetReportKey(require);
            return ReportExport(key);
        }
        public string ReportExport(ReportReportElement reportElement)
        {
            return this.Reporter.Export(reportElement);
        }

        public string ReportExport(Hashtable reportElementTable)
        {
            if (reportElementTable.Count > 0)
            {
                return this.Reporter.Export(reportElementTable,ReportElementTag.ReportElement);
            }
            else
            {
                return null;
            }
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