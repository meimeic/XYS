using System.Collections;
using System.Collections.Generic;
using System.Text;

using XYS.Model;
using XYS.Lis.Core;
using XYS.Lis.Model.Export;
namespace XYS.Lis.Export
{
    public abstract class ReportExportSkeleton : IReportExport
    {
        #region
        private readonly string m_exportName;
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
        public string export(ILisExportElement element)
        {
            if (element.ElementTag == ReportElementTag.ReportElement)
            {
                ReporterReport report = element as ReporterReport;
                if (report != null)
                {
                    OperateReport(report);
                }
            }
            return InnerExport(element);
        }
        public string export(List<ILisExportElement> exportElementList, ReportElementTag elementTag)
        {
            PreFilter(exportElementList, elementTag);
            return InnerExport(exportElementList, elementTag);
        }
        #endregion

        #region
        protected abstract string InnerExport(ILisExportElement exportElement);
        protected abstract string InnerExport(List<ILisExportElement> exportElementList, ReportElementTag elementTag);
        #endregion

        #region
        protected virtual void PreFilter(List<ILisExportElement> exportElementList, ReportElementTag elementTag)
        {
            if (exportElementList != null && exportElementList.Count > 0)
            {
                bool result;
                for (int i = exportElementList.Count - 1; i >= 0; i--)
                {
                    result = IsElementAndOperate(exportElementList[i], elementTag);
                    if (!result)
                    {
                        exportElementList.RemoveAt(i);
                    }
                }
            }
        }
        #endregion

        #region
        protected virtual bool IsElementAndOperate(ILisExportElement exportElement, ReportElementTag elementTag)
        {
            bool result = false;
            switch (elementTag)
            {
                case ReportElementTag.ReportElement:
                    if (exportElement is ReporterReport)
                    {
                        OperateReport(exportElement as ReporterReport);
                        result = true;
                    }
                    break;
                case ReportElementTag.InfoElement:
                    if (exportElement is ReporterInfo)
                    {
                        result = true;
                    }
                    break;
                case ReportElementTag.ItemElement:
                    if (exportElement is ReporterItem)
                    {
                        result = true;
                    }
                    break;
                case ReportElementTag.GraphElement:
                    if (exportElement is ReporterGraph)
                    {
                        result = true;
                    }
                    break;
                case ReportElementTag.CustomElement:
                    if (exportElement is ReporterCustom)
                    {
                        result = true;
                    }
                    break;
                case ReportElementTag.KVElement:
                    if (exportElement is ReporterKV)
                    {
                        result = true;
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
        #endregion

        #region
        protected virtual void OperateReport(ReporterReport report)
        {
            SortReport(report);
        }
        private void SortReport(ReporterReport report)
        {
            if (report.ItemList != null && report.ItemList.Count > 0)
            {
                report.ItemList.Sort();
            }
        }
        #endregion
    }
}
