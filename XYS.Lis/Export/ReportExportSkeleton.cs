using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using XYS.Common;
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
        //protected virtual void PreFilter(List<ILisExportElement> exportElementList, ReportElementTag elementTag)
        //{
        //    if (exportElementList != null && exportElementList.Count > 0)
        //    {
        //        bool result;
        //        for (int i = exportElementList.Count - 1; i >= 0; i--)
        //        {
        //            result = IsElementAndOperate(exportElementList[i], elementTag);
        //            if (!result)
        //            {
        //                exportElementList.RemoveAt(i);
        //            }
        //        }
        //    }
        //}
        #endregion

        #region
        protected virtual void ExportReport(ReportReportElement reportReport, ReportReport exportReport)
        {

        }
        protected virtual void ExportElement(IReportElement reportElement, IExportElement exportElement)
        {
        }
        #endregion

        #region
        protected void ConvertElement(IReportElement reportElement, IExportElement exportElement)
        {
            Type reportType = reportElement.GetType();
            Type exportType = exportElement.GetType();
            PropertyInfo[] props = reportType.GetProperties();
            PropertyInfo ep;
            if (props == null || props.Length == 0)
            {
                return;
            }
            //处理基本数据
            foreach (PropertyInfo rp in props)
            {
                if (IsConvert(rp))
                {
                    ep = exportType.GetProperty(rp.Name);
                    if (ep != null)
                    {
                        SetProp(rp, reportElement, ep, exportElement);
                    }
                }
            }
        }
        #endregion

        #region
        private void SetProp(PropertyInfo rp, IReportElement element, PropertyInfo ep, IExportElement exportElement)
        {
            ep.SetValue(exportElement, rp.GetValue(element, null), null);
        }
        #endregion

        #region
        private bool IsConvert(PropertyInfo prop)
        {
            object[] conAttrs = null;
            if (prop != null)
            {
                conAttrs = prop.GetCustomAttributes(typeof(ConvertAttribute), true);
                if (conAttrs != null && conAttrs.Length > 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
