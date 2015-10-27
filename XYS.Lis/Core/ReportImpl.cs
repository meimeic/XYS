using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Common;
using XYS.Lis;
using XYS.Lis.DAL;
using XYS.Lis.Export;

namespace XYS.Lis.Core
{
    public class ReportImpl:ReporterWrapperImpl,IReport
    {
        #region
        private ExportTag m_export2PDF;
        private ExportTag m_export2Json;
        private ExportTag m_export2Html;
        private ExportTag m_export2Xml;
        private LisReporterKeyDAL m_reportKeyDAL;
        #endregion
        #region
        public ReportImpl(ILisReporter reporter)
            : base(reporter)
        {
            this.m_export2PDF = ExportTag.PDF;
            this.m_export2Json = ExportTag.JSON;
            this.m_export2Html = ExportTag.HTML;
            this.m_export2Xml = ExportTag.XML;
            this.m_reportKeyDAL = new LisReporterKeyDAL();
        }
        #endregion
        
        #region

        public virtual string Report2PDF(LisSearchRequire require)
        {
            ReportKey key = GetReportKey(require);
            if (key != null)
            {
                return Reporter.Export(key, this.m_export2PDF);
            }
            else
            {
                return null;
            }
        }

        public virtual string Report2Json(LisSearchRequire require)
        {
            ReportKey key = GetReportKey(require);
            if (key != null)
            {
                return Reporter.Export(key, this.m_export2Json);
            }
            else
            {
                return null;
            }
        }
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