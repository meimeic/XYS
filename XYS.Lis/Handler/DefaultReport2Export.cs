using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using XYS.Common;
using XYS.Model;
using XYS.Lis.Core;
using XYS.Lis.Util;
using XYS.Lis.Model;
using XYS.Lis.Model.Export;

namespace XYS.Lis.Handler
{
    public class DefaultReport2Export : IModelConvert
    {
        private readonly Hashtable m_tag2ExportType;
        
        #region 构造函数
        public DefaultReport2Export()
        {
            this.m_tag2ExportType = new Hashtable(5);
        }
        #endregion
        
        #region 实现接口方法
        public bool Convert2Export(ILisReportElement reportElement, ILisExportElement exportElement)
        {
            if (reportElement.ElementTag == ReportElementTag.NoneElement)
            {
                return false;
            }
            if (reportElement.ElementTag == exportElement.ElementTag)
            {
                return IsElementAndConvert(reportElement, exportElement, reportElement.ElementTag);
            }
            return false;
        }
        public bool Convert2Export(List<ILisReportElement> reportElementList, List<ILisExportElement> exportElementList, ReportElementTag elementTag)
        {
            bool flag;
            Type exportType;
            ILisExportElement exportElement;
            if (reportElementList != null && reportElementList.Count > 0)
            {
                foreach (ILisReportElement reportElement in reportElementList)
                {
                    try
                    {
                        exportType = GetExportType(elementTag);
                        if (exportType != null)
                        {
                            exportElement = (ILisExportElement)Activator.CreateInstance(exportType);
                            flag = Convert2Export(reportElement, exportElement);
                            if (flag)
                            {
                                exportElementList.Add(exportElement);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region
        private bool IsElementAndConvert(ILisReportElement element, ILisExportElement exportElement, ReportElementTag elementTag)
        {
            bool result = false;
            switch (elementTag)
            {
                case ReportElementTag.ReportElement:
                    if (element is ReportReportElement && exportElement is ReporterReport)
                    {
                        ConvertElement(element, exportElement);
                        ConvertReport(element as ReportReportElement, exportElement as ReporterReport);
                        result = true;
                    }
                    break;
                case ReportElementTag.KVElement:
                    if (element is ReportKVElement && exportElement is ReporterKV)
                    {
                        ConvertElement(element, exportElement);
                        ConvertKV(element as ReportKVElement, exportElement as ReporterKV);
                        result = true;
                    }
                    break;
                case ReportElementTag.InfoElement:
                    if (element is ReportInfoElement && exportElement is ReporterInfo)
                    {
                        ConvertElement(element, exportElement);
                        result = true;
                    }
                    break;
                case ReportElementTag.ItemElement:
                    if (element is ReportItemElement && exportElement is ReporterItem)
                    {
                        ConvertElement(element, exportElement);
                        result = true;
                    }
                    break;
                case ReportElementTag.GraphElement:
                    if (element is ReportGraphElement && exportElement is ReporterGraph)
                    {
                        ConvertElement(element, exportElement);
                        result = true;
                    }
                    break;
                case ReportElementTag.CustomElement:
                    if (element is ReportCustomElement && exportElement is ReporterCustom)
                    {
                        ConvertElement(element, exportElement);
                        result = true;
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
        private void ConvertReport(ReportReportElement rre, ReporterReport rr)
        {
            ReportElementTag elementTag;
            List<ILisReportElement> reportElementList;
            List<ILisExportElement> exportElementList = new List<ILisExportElement>(20);
            Hashtable table = rre.ReportItemTable;
            //转换特殊数据
            foreach (DictionaryEntry de in table)
            {
                try
                {
                    elementTag = (ReportElementTag)de.Key;
                    reportElementList = rre.GetReportItem(elementTag);
                    //过滤的元素
                    if (elementTag == ReportElementTag.ReportElement || elementTag == ReportElementTag.NoneElement)
                    {
                        continue;
                    }
                    //
                    if (reportElementList.Count > 0)
                    {
                        //reportinfo 元素
                        if (elementTag == ReportElementTag.InfoElement)
                        {
                            ReportInfoElement infoElement = reportElementList[0] as ReportInfoElement;
                            if (infoElement != null)
                            {
                                ConvertElement(infoElement, rr.ReportInfo);
                            }
                        }
                        //其他集合元素
                        else
                        {
                            exportElementList.Clear();
                            Convert2Export(reportElementList, exportElementList, elementTag);
                            AddExportElement(rr, exportElementList, elementTag);
                        }
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            //转换父类检验项
            foreach (int parItem in rre.ParItemList)
            {
                rr.ParItemList.Add(parItem);
            }
        }
        private void ConvertKV(ReportKVElement rkve, ReporterKV rkv)
        {
            foreach (DictionaryEntry de in rkve.KVTable)
            {
                rkv.KVTable.Add(de.Key, de.Value);
            }
        }
        #endregion

        #region
        private bool IsElementAndAdd(ReporterReport rr, List<ILisExportElement> exportElementList, ReportElementTag elementTag)
        {
            bool result = false;
            switch (elementTag)
            {
                case ReportElementTag.ItemElement:
                    AddExportItem(rr, exportElementList);
                    result = true;
                    break;
                case ReportElementTag.GraphElement:
                    AddExportGraph(rr, exportElementList);
                    result = true;
                    break;
                case ReportElementTag.CustomElement:
                    AddExportCustom(rr, exportElementList);
                    result = true;
                    break;
                case ReportElementTag.KVElement:
                    AddExportKV(rr, exportElementList);
                    result = true;
                    break;
                default:
                    break;
            }
            return result;
        }
        private void AddExportElement(ReporterReport rr, List<ILisExportElement> exportElementList, ReportElementTag elementTag)
        {
            switch (elementTag)
            {
                case ReportElementTag.ItemElement:
                    AddExportItem(rr, exportElementList);
                    break;
                case ReportElementTag.GraphElement:
                    AddExportGraph(rr, exportElementList);
                    break;
                case ReportElementTag.CustomElement:
                    AddExportCustom(rr, exportElementList);
                    break;
                case ReportElementTag.KVElement:
                    AddExportKV(rr, exportElementList);
                    break;
                default:
                    break;
            }
        }
        private void AddExportItem(ReporterReport rr, List<ILisExportElement> exportElementList)
        {
            ReporterItem ri;
            if (exportElementList != null && exportElementList.Count > 0)
            {
                if (rr.ItemList == null)
                {
                    rr.ItemList = new List<ReporterItem>(16);
                }
                foreach (ILisExportElement exportElement in exportElementList)
                {
                    ri = exportElement as ReporterItem;
                    if (ri != null)
                    {
                        rr.ItemList.Add(ri);
                    }
                }
            }
        }
        private void AddExportGraph(ReporterReport rr, List<ILisExportElement> exportElementList)
        {
            ReporterGraph rg;
            if (exportElementList != null && exportElementList.Count > 0)
            {
                if (rr.GraphList == null)
                {
                    rr.GraphList = new List<ReporterGraph>(2);
                }
                foreach (ILisExportElement exportElement in exportElementList)
                {
                    rg = exportElement as ReporterGraph;
                    if (rg != null)
                    {
                        rr.GraphList.Add(rg);
                    }
                }
            }
        }
        private void AddExportCustom(ReporterReport rr, List<ILisExportElement> exportElementList)
        {
            ReporterCustom rc;
            if (exportElementList != null && exportElementList.Count > 0)
            {
                if (rr.CustomList == null)
                {
                    rr.CustomList = new List<ReporterCustom>(2);
                }
                foreach (ILisExportElement exportElement in exportElementList)
                {
                    rc = exportElement as ReporterCustom;
                    if (rc != null)
                    {
                        rr.CustomList.Add(rc);
                    }
                }
            }
        }
        private void AddExportKV(ReporterReport rr, List<ILisExportElement> exportElementList)
        {
            ReporterKV rkv;
            if (exportElementList != null && exportElementList.Count > 0)
            {
                if (rr.KVList == null)
                {
                    rr.KVList = new List<ReporterKV>(2);
                }
                foreach (ILisExportElement exportElement in exportElementList)
                {
                    rkv = exportElement as ReporterKV;
                    if (rkv != null)
                    {
                        rr.KVList.Add(rkv);
                    }
                }
            }
        }
        #endregion

        #region
        private void ConvertElement(ILisReportElement element, ILisExportElement exportElement)
        {
            ExportAttribute expAttr;
            Type elementType = element.GetType();
            Type exportType = exportElement.GetType();
            PropertyInfo[] props = elementType.GetProperties();
            PropertyInfo exportProp;
            if (props == null || props.Length == 0)
            {
                return;
            }
            //处理基本数据
            foreach (PropertyInfo p in props)
            {
                expAttr = (ExportAttribute)Attribute.GetCustomAttribute(p, typeof(ExportAttribute));
                if (expAttr != null && expAttr.IsConvert)
                {
                    exportProp = exportType.GetProperty(p.Name);
                    if (exportProp != null)
                    {
                        SetProp(p, element, exportProp, exportElement);
                    }
                }
            }
        }
        private void SetProp(PropertyInfo p, ILisReportElement element, PropertyInfo p1, ILisExportElement exportElement)
        {
            p1.SetValue(exportElement, p.GetValue(element, null), null);
        }
        #endregion

        #region 实例私有方法 处理 tag2exporttype;
        private Type GetExportType(ReportElementTag elementTag)
        {
            if (this.m_tag2ExportType.Count == 0)
            {
                InitTag2ExportTypeTable();
            }
            return this.m_tag2ExportType[elementTag] as Type;
        }
        private void InitTag2ExportTypeTable()
        {
            LisMap.InitTag2ExportTypeTable(this.m_tag2ExportType);
        }
        #endregion
    }
}
