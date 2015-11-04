using System.Collections.Generic;
using System.Text;

using XYS.Model;
using XYS.Lis.Model;
using XYS.Lis.Core;
namespace XYS.Lis.Export
{
    public abstract class ReportExportSkeleton:IReportExport
    {
        #region
        private readonly string m_exportName;
        #endregion

        #region
        private ExportTag m_exportTag;
        private string m_reportInfoSeparator;
        #endregion

        #region
        protected ReportExportSkeleton(string name)
        {
            this.m_exportName = name.ToLower();
        }
        #endregion

        #region
        public virtual string ReportInfoSeparator
        {
            get { return this.m_reportInfoSeparator; }
            set { this.m_reportInfoSeparator = value; }
        }
        #endregion
        
        #region 实现IReportExport接口
        public virtual string ExportName
        {
            get { return this.m_exportName.ToLower(); }
        }
        public virtual ExportTag ExportTag
        {
            get { return this.m_exportTag; }
            protected set { this.m_exportTag = value; }
        }
      
        public virtual string export(ILisReportElement element)
        {
            PreFilter(element);
            if (element.ElementTag == ReportElementTag.ReportElement)
            {
                ReportReportElement rre = element as ReportReportElement;
                return InnerReportExport(rre);
            }
            else
            {
                return InnerElementExport(element);
            }
        }
        public virtual string export(List<ILisReportElement> reportElementList, ReportElementTag elementTag)
        {
            if (elementTag == ReportElementTag.ReportElement)
            {
                return this.ReportListExport(reportElementList);
            }
            else
            {
                return InnerElementListExport(reportElementList);
            }
        }
        #endregion

        #region
        protected abstract string InnerElementListExport(List<ILisReportElement> elementList);
        protected abstract string InnerElementExport(ILisReportElement reportElement);
        protected abstract string InnerReportExport(ReportReportElement rre);
        #endregion
        
        #region
        protected virtual void PreFilter(ILisReportElement reportElement)
        {
        }
        protected virtual string ReportListExport(List<ILisReportElement> reportList)
        {
            StringBuilder sb = new StringBuilder();
            if (reportList.Count > 0)
            {
                ReportReportElement rre;
                string temp;
                foreach (ILisReportElement reportElement in reportList)
                {
                    rre = reportElement as ReportReportElement;
                    if (rre != null)
                    {
                        temp=InnerReportExport(rre);
                        sb.Append(temp);
                        sb.Append(this.ReportInfoSeparator);
                    }
                }
            }
            return sb.ToString();
        }
        #endregion
    }
}
