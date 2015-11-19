using System.Collections;
using System.Collections.Generic;
using System.Text;

using XYS.Model;
using XYS.Lis.Model;
using XYS.Lis.Core;
namespace XYS.Lis.Export
{
    public abstract class ReportExportSkeleton : IReportExport
    {
        #region
        private readonly string m_exportName;
        #endregion

        #region
        private ExportTag m_exportTag;
        #endregion

        #region 构造函数
        protected ReportExportSkeleton(string name)
        {
            this.m_exportName = name;
        }
        #endregion

        #region 实例属性
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
            if (elementTag != ReportElementTag.NoneElement)
            {
                return this.ReportElementsExport(reportElementList, elementTag);
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region
        protected abstract string InnerElementExport(ILisReportElement reportElement);
        protected abstract string InnerReportExport(ReportReportElement rre);
        protected abstract string GetSeparateByTag(ReportElementTag elementTag);
        #endregion

        #region
        protected virtual string ReportElementsExport(List<ILisReportElement> reportElementList, ReportElementTag elementTag)
        {
            string temp;
            string separate;
            ReportReportElement rre;
            StringBuilder sb = new StringBuilder();
            if (reportElementList.Count > 0)
            {
                separate = GetSeparateByTag(elementTag);
                foreach (ILisReportElement reportElement in reportElementList)
                {
                    if (elementTag == ReportElementTag.ReportElement)
                    {
                        rre = reportElement as ReportReportElement;
                        temp = InnerReportExport(rre);
                    }
                    else
                    {
                        temp = InnerElementExport(reportElement);
                    }
                    //
                    if (temp != null && !temp.Equals(""))
                    {
                        sb.Append(temp);
                    }
                    if (separate != null && !separate.Equals(""))
                    {
                        sb.Append(separate);
                    }
                }
                if (sb.Length > separate.Length)
                {
                    sb.Remove(sb.Length - separate.Length, separate.Length);
                }
            }
            return sb.ToString();
        }
        protected virtual void PreFilter(ILisReportElement reportElement)
        {
        }
        protected virtual void PreFilter(List<ILisReportElement> reportElement, ReportElementTag elementTag)
        {
        }
        #endregion

        #region
        #endregion
    }
}
