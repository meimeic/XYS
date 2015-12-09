using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using XYS.Model;
using XYS.Lis.Core;
using XYS.Lis.Model;
using XYS.Common;
using XYS.Lis.Service.Models.Report;

namespace XYS.Lis.Service.Common
{
    public class ModelConvert : IModelConvert
    {
        private readonly Hashtable m_tag2ExportType;

        #region 构造函数
        public ModelConvert()
        {
            this.m_tag2ExportType = new Hashtable(5);
        }
        #endregion

        #region 实现接口方法
        public bool Convert2Export(ILisReportElement reportElement, IReportModel exportElement)
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
        public bool Convert2Export(List<ILisReportElement> reportElementList, List<IReportModel> exportElementList, ReportElementTag elementTag)
        {
            bool flag;
            Type exportType;
            IReportModel exportElement;
            if (reportElementList != null && reportElementList.Count > 0)
            {
                foreach (ILisReportElement reportElement in reportElementList)
                {
                    try
                    {
                        exportType = GetExportType(elementTag);
                        if (exportType != null)
                        {
                            exportElement = (IReportModel)Activator.CreateInstance(exportType);
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
        private bool IsElementAndConvert(ILisReportElement element, IReportModel exportElement, ReportElementTag elementTag)
        {
            bool result = false;
            switch (elementTag)
            {
                case ReportElementTag.ReportElement:
                    if (element is ReportReportElement && exportElement is ReportReport)
                    {
                        ConvertElement(element, exportElement);
                        ConvertReport(element as ReportReportElement, exportElement as ReportReport);
                        result = true;
                    }
                    break;
                case ReportElementTag.KVElement:
                    if (element is ReportKVElement && exportElement is ReportKV)
                    {
                        ConvertElement(element, exportElement);
                        ConvertKV(element as ReportKVElement, exportElement as ReportKV);
                        result = true;
                    }
                    break;
                case ReportElementTag.InfoElement:
                    if (element is ReportInfoElement && exportElement is ReportInfo)
                    {
                        ConvertElement(element, exportElement);
                        result = true;
                    }
                    break;
                case ReportElementTag.ItemElement:
                    if (element is ReportItemElement && exportElement is ReportItem)
                    {
                        ConvertElement(element, exportElement);
                        result = true;
                    }
                    break;
                case ReportElementTag.GraphElement:
                    if (element is ReportGraphElement && exportElement is ReportGraph)
                    {
                        ConvertElement(element, exportElement);
                        result = true;
                    }
                    break;
                case ReportElementTag.CustomElement:
                    if (element is ReportCustomElement && exportElement is ReportCustom)
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
        private void ConvertReport(ReportReportElement rre, ReportReport rr)
        {
            ReportElementTag elementTag;
            List<ILisReportElement> reportElementList;
            List<IReportModel> exportElementList = new List<IReportModel>(20);
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
        private void ConvertKV(ReportKVElement rkve, ReportKV rkv)
        {
            foreach (DictionaryEntry de in rkve.KVTable)
            {
                rkv.KVTable.Add(de.Key, de.Value);
            }
        }
        #endregion

        #region
        private bool IsElementAndAdd(ReportReport rr, List<IReportModel> exportElementList, ReportElementTag elementTag)
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
        private void AddExportElement(ReportReport rr, List<IReportModel> exportElementList, ReportElementTag elementTag)
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
        private void AddExportItem(ReportReport rr, List<IReportModel> exportElementList)
        {
            ReportItem ri;
            if (exportElementList != null && exportElementList.Count > 0)
            {
                if (rr.ItemList == null)
                {
                    rr.ItemList = new List<ReportItem>(16);
                }
                foreach (IReportModel exportElement in exportElementList)
                {
                    ri = exportElement as ReportItem;
                    if (ri != null)
                    {
                        rr.ItemList.Add(ri);
                    }
                }
            }
        }
        private void AddExportGraph(ReportReport rr, List<IReportModel> exportElementList)
        {
            ReportGraph rg;
            if (exportElementList != null && exportElementList.Count > 0)
            {
                if (rr.GraphList == null)
                {
                    rr.GraphList = new List<ReportGraph>(2);
                }
                foreach (IReportModel exportElement in exportElementList)
                {
                    rg = exportElement as ReportGraph;
                    if (rg != null)
                    {
                        rr.GraphList.Add(rg);
                    }
                }
            }
        }
        private void AddExportCustom(ReportReport rr, List<IReportModel> exportElementList)
        {
            ReportCustom rc;
            if (exportElementList != null && exportElementList.Count > 0)
            {
                if (rr.CustomList == null)
                {
                    rr.CustomList = new List<ReportCustom>(2);
                }
                foreach (IReportModel exportElement in exportElementList)
                {
                    rc = exportElement as ReportCustom;
                    if (rc != null)
                    {
                        rr.CustomList.Add(rc);
                    }
                }
            }
        }
        private void AddExportKV(ReportReport rr, List<IReportModel> exportElementList)
        {
            ReportKV rkv;
            if (exportElementList != null && exportElementList.Count > 0)
            {
                if (rr.KVList == null)
                {
                    rr.KVList = new List<ReportKV>(2);
                }
                foreach (IReportModel exportElement in exportElementList)
                {
                    rkv = exportElement as ReportKV;
                    if (rkv != null)
                    {
                        rr.KVList.Add(rkv);
                    }
                }
            }
        }
        #endregion

        #region
        private void ConvertElement(ILisReportElement element, IReportModel exportElement)
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
        private void SetProp(PropertyInfo p, ILisReportElement element, PropertyInfo p1, IReportModel exportElement)
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
            //
            lock (this.m_tag2ExportType)
            {
                this.m_tag2ExportType[ReportElementTag.ReportElement] = typeof(ReportReport);
                this.m_tag2ExportType[ReportElementTag.InfoElement] = typeof(ReportInfo);
                this.m_tag2ExportType[ReportElementTag.ItemElement] = typeof(ReportItem);
                this.m_tag2ExportType[ReportElementTag.GraphElement] = typeof(ReportGraph);
                this.m_tag2ExportType[ReportElementTag.KVElement] = typeof(ReportKV);
                this.m_tag2ExportType[ReportElementTag.CustomElement] = typeof(ReportCustom);
            }
        }
        #endregion
    }
}
