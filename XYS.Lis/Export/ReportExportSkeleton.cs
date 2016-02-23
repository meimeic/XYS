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
        private readonly ElementTypeMap m_elementTypeMap;
        #endregion

        #region 构造函数
        protected ReportExportSkeleton(string name)
        {
            this.m_exportName = name;
            this.m_kv2Custom = new Hashtable(10);
            this.m_elementTypeMap = new ElementTypeMap(3);
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
        public void export(ReportReportElement report, ReportReport export)
        {
            ExportReport(report, export);
            AfterExport(export);
        }

        //public void export(List<IReportElement> reportElements, List<IExportElement> exportElements)
        //{
        //    throw new System.NotImplementedException();
        //}
        #endregion

        #region
        protected abstract void ConvertGraph2Image(List<IReportElement> graphList, ReportReport exportReport);
        protected abstract void AfterExport(ReportReport export);
        #endregion

        #region
        protected virtual void ExportReport(ReportReportElement reportReport, ReportReport exportReport)
        {
            ExportElement(reportReport, exportReport);
            ExportElement(reportReport.ReportExam, exportReport.ReportExam);
            ExportElement(reportReport.ReportPatient, exportReport.ReportPatient);

            //
            string rName = null;
            Type exportType = null;
            List<IReportElement> tempList = null;
            List<IExportElement> exportList=null;
            foreach (DictionaryEntry de in reportReport.ReportItemTable)
            {
                rName = de.Key as string;
                tempList = de.Value as List<IReportElement>;
                if (rName != null&&IsExist(tempList))
                {
                    if (rName.Equals(typeof(ReportGraphElement).Name))
                    {
                        ConvertGraph2Image(tempList, exportReport);
                    }
                    else
                    {
                        exportType=GetExportType(rName);
                        if(exportType!=null)
                        {
                            exportList=exportReport.GetReportItem(exportType.Name);
                            InnerExport(tempList, exportList, exportType);
                        }
                    }
                }
            }
        }
        protected virtual void InnerExport(List<IReportElement> reportElementList, List<IExportElement> exportElementList, Type exportType)
        {
            IExportElement exportElement = null;
            foreach (IReportElement re in reportElementList)
            {
                try
                {
                    exportElement = (IExportElement)exportType.Assembly.CreateInstance(exportType.FullName);
                    ConvertElement(re, exportElement);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
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
        //{
        //    ReportGraphElement rge = null;
        //    foreach (IReportElement re in graphList)
        //    {
        //        rge = re as ReportGraphElement;
        //        if (rge != null)
        //        {
        //            reportImage.Add(rge.GraphName, rge.GraphImage);
        //        }
        //    }
        //}
        //protected abstract void ConvertGraph2Image(ReportGraphElement rge, ReportImage reportImage);
        //{
        //    reportImage.Add(rge.GraphName, rge.GraphImage);
        //}
        #endregion

        #region
        protected void SetProp(PropertyInfo rp, IReportElement element, PropertyInfo ep, IExportElement exportElement)
        {
            ep.SetValue(exportElement, rp.GetValue(element, null), null);
        }
        protected void SetProp(PropertyInfo ep, IExportElement exportElement,object value)
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
        private Type GetExportType(string rName)
        {
            if (!string.IsNullOrEmpty(rName))
            {
                if (this.m_elementTypeMap.Count == 0)
                {
                    //初始化
                }
                ElementType elementType = this.m_elementTypeMap[rName] as ElementType;
                if (elementType != null && elementType.ExportType != null)
                {
                    return elementType.ExportType;
                }
            }
            return null;
        }
        #endregion
    }
}
