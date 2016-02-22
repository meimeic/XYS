using System.Collections;
using System.Collections.Generic;
using System.Text;

using XYS.Lis.Core;
using XYS.Lis.Model;
using XYS.Lis.Export.Model;
namespace XYS.Lis.Export
{
    public abstract class ReportExportSkeleton : IReportExport
    {
        #region
        private readonly string m_exportName;
        #endregion

        #region 构造函数
        protected ReportExportSkeleton(string name)
        {
            this.m_exportName = name;
        }
        #endregion

        #region 实现IReportExport接口
        public virtual string ExportName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.m_exportName))
                {
                    return this.m_exportName.ToLower();
                }
                return null;
            }
        }
        public void export(IReportElement reportElement, IExportElement exportElement)
        {
            throw new System.NotImplementedException();
        }

        public void export(List<IReportElement> reportElements, List<IExportElement> exportElements)
        {
            throw new System.NotImplementedException();
        }
        //public string export(IReportElement element)
        //{
        //    if (element.ElementTag == ReportElementTag.Report)
        //    {
        //        ReportReportElement report = element as ReportReportElement;
        //        if (report != null)
        //        {
        //            OperateReport(report);
        //        }
        //    }
        //    return InnerExport(element);
        //}
        //public string export(List<IReportElement> exportElementList)
        //{
        //    PreFilter(exportElementList);
        //    return InnerExport(exportElementList, elementTag);
        //}
        #endregion

        #region
        protected abstract string InnerExport(IReportElement exportElement);
        protected abstract string InnerExport(List<IExportElement> exportElementList, ReportElementTag elementTag);
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
        protected virtual void ExportReport(ReportReportElement reportReport, ReportReport exportReport)
        {

        }
        protected virtual void ExportElement(IReportElement reportReport, IExportElement exportElement)
        {

        }
        #endregion

        #region
        #endregion

        #region
        #endregion
    }
}
