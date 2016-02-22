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
        private readonly Hashtable m_kv2Custom;
        #endregion

        #region 构造函数
        protected ReportExportSkeleton(string name)
        {
            this.m_exportName = name;
            this.m_kv2Custom = new Hashtable(10);
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
            foreach(DictionaryEntry de in )
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
        protected void ConvertKV2Custom(ReportKVElement rkv,ReportCustom rc)
        {
            PropertyInfo exportProperty = null;
            string propertyName = null;
            foreach (DictionaryEntry de in rkv.KVTable)
            {
                propertyName = this.m_kv2Custom[de.Key] as string;
                if (!string.IsNullOrEmpty(propertyName))
                {
                    exportProperty = rc.GetType().GetProperty(propertyName);
                    if (exportProperty != null)
                    {
                        SetProp(exportProperty,rc,de.Value);
                    }
                }
            }
        }
        protected void ConvertGraph2Image(List<IReportElement> graphList,ReportImage reportImage)
        {
            ReportGraphElement rge = null;
            foreach (IReportElement re in graphList)
            {
                rge = re as ReportGraphElement;
                if (rge != null)
                {
                    reportImage.Add(rge.GraphName, rge.GraphImage);
                }
            }
        }
        protected void ConvertGraph2Image(ReportGraphElement rge, ReportImage reportImage)
        {
            reportImage.Add(rge.GraphName, rge.GraphImage);
        }
        #endregion

        #region
        private void SetProp(PropertyInfo rp, IReportElement element, PropertyInfo ep, IExportElement exportElement)
        {
            ep.SetValue(exportElement, rp.GetValue(element, null), null);
        }
        private void SetProp(PropertyInfo ep, IExportElement exportElement,object value)
        {
            ep.SetValue(exportElement, value, null);
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
        private bool IsExist(List<IReportElement> itemList)
        {
            if (itemList != null && itemList.Count > 0)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
