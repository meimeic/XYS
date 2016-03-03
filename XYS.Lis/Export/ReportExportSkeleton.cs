using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;
using XYS.Lis.Core;
using XYS.Lis.Model;
using XYS.Lis.Export.Model;
using XYS.Lis.Util;
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
            get { return this.m_exportName.ToLower(); }
        }
        public void export(ReportReportElement report, ReportReport export)
        {
            ExportReport(report, export);
            AfterExport(export);
        }

        public void export(List<ReportReportElement> reportElements, List<ReportReport> exportElements)
        {
            ReportReport er = null;
            foreach (ReportReportElement rre in reportElements)
            {
                er = new ReportReport();
                export(rre, er);
            }
        }
        #endregion

        #region
        protected abstract void ConvertGraph2Image(List<ILisReportElement> graphList, List<IExportElement> imageList);
        protected abstract void AfterExport(ReportReport export);
        #endregion

        #region
        protected virtual void ExportReport(ReportReportElement reportReport, ReportReport exportReport)
        {
            ExportElement(reportReport, exportReport);

            ExportElement(reportReport.ReportExam, exportReport.ReportExam);
            ExportElement(reportReport.ReportPatient, exportReport.ReportPatient);
            ConvertPatItemList(reportReport.ParItemList, exportReport.ParItemList);

            //
            string rName = null;
           //Type exportType = null;
            List<ILisReportElement> tempList = null;
            //List<IExportElement> exportList = null;
            foreach (DictionaryEntry de in reportReport.ReportItemTable)
            {
                rName = de.Key as string;
                tempList = de.Value as List<ILisReportElement>;
                if (rName != null && IsExist(tempList))
                {
                    // //图片项处理
                    // if (rName.Equals(typeof(ReportGraphElement).Name))
                    // {
                    //     ConvertGraph2Image(tempList, exportReport);
                    // }
                    //// 其他项处理
                    //else
                    //{
                    //exportType = GetExportType(rName);
                    //if (exportType != null)
                    //{
                    //    exportList = exportReport.GetReportItem(exportType.Name);
                    //    InnerExport(tempList, exportList, exportType);
                    //}
                    //}
                    InnerExport(tempList,exportReport,rName);
                }
            }
        }
        protected virtual void InnerExport(List<ILisReportElement> reportElementList, ReportReport exportReport, string reportElementName)
        {
            Type exportType = null;
            IExportElement exportElement = null;
            List<IExportElement> exportList = null;
            exportType = GetExportType(reportElementName);
            if (exportType != null)
            {
                exportList = exportReport.GetReportItem(exportType.Name);
                if (reportElementName.Equals(typeof(ReportGraphElement).Name))
                {
                    ConvertGraph2Image(reportElementList, exportList);
                }
                else if (reportElementName.Equals(typeof(ReportKVElement).Name))
                {
                    foreach (ILisReportElement re in reportElementList)
                    {
                        try
                        {
                            exportElement = (IExportElement)exportType.Assembly.CreateInstance(exportType.FullName);
                            ConvertKV2Custom(re, exportElement);
                            exportList.Add(exportElement);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    foreach (ILisReportElement re in reportElementList)
                    {
                        try
                        {
                            exportElement = (IExportElement)exportType.Assembly.CreateInstance(exportType.FullName);
                            ExportElement(re, exportElement);
                            exportList.Add(exportElement);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }
            }
        }
        #endregion

        #region
        protected virtual void ExportElement(ILisReportElement reportElement, IExportElement exportElement)
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
        protected void ConvertKV2Custom(ILisReportElement reportElement, IExportElement exportElement)
        {
            ReportCustom rc = exportElement as ReportCustom;
            ReportKVElement rkv = reportElement as ReportKVElement;
            if (rc != null && rkv != null)
            {
                rc.Name = rkv.Name;
                PropertyInfo exportProperty = null;
                string propertyName = null;
                foreach (DictionaryEntry de in rkv.KVTable)
                {
                    propertyName = de.Key as string;
                    if (!string.IsNullOrEmpty(propertyName))
                    {
                        exportProperty = rc.GetType().GetProperty(propertyName);
                        if (exportProperty != null)
                        {
                            SetProp(exportProperty, rc, de.Value);
                        }
                    }
                }
            }
        }
        protected void ConvertPatItemList(List<int> rList, List<int> eList)
        {
            eList.Clear();
            foreach (int re in rList)
            {
                eList.Add(re);
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
        protected void SetProp(PropertyInfo rp, ILisReportElement element, PropertyInfo ep, IExportElement exportElement)
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
                conAttrs = prop.GetCustomAttributes(typeof(FRConvertAttribute), true);
                if (conAttrs != null && conAttrs.Length > 0)
                {
                    return true;
                }
            }
            return false;
        }
        private bool IsExist(List<ILisReportElement> itemList)
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
                    InitElementTypeMap();
                }
                ElementType elementType = this.m_elementTypeMap[rName] as ElementType;
                if (elementType != null && elementType.ExportType != null)
                {
                    return elementType.ExportType;
                }
            }
            return null;
        }
        private bool IsReport(Type type)
        {
            return typeof(ReportReport).Equals(type);
        }
        private void InitElementTypeMap()
        {
            lock (this.m_elementTypeMap)
            {
                LisMap.ConfigureReportElementMap(this.m_elementTypeMap);
            }
        }
        #endregion
    }
}
