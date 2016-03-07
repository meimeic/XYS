using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;
using XYS.Model;
using XYS.Common;
using XYS.FRReport.Model;
namespace XYS.FRReport.Convert.Lis
{
    public abstract class FRConvertSkeleton : IFRConvert
    {
        #region
        private readonly string m_converterName;
        private readonly Hashtable m_kv2Custom;
        //private readonly ElementTypeMap m_elementTypeMap;
        #endregion

        #region 构造函数
        protected FRConvertSkeleton(string name)
        {
            this.m_converterName = name;
            this.m_kv2Custom = new Hashtable(10);
            //this.m_elementTypeMap = new ElementTypeMap(3);
        }
        #endregion

        #region 实现IReportExport接口
        public virtual string ConverterName
        {
            get { return this.m_converterName.ToLower(); }
        }
        public void convert(IReportElement report, IFRExportElement export)
        {
            if (IsConvert(report, export))
            {
                InnerElementConvert(report, export);
            }
        }

        public void convert(List<IReportElement> reportElements, List<IFRExportElement> exportElements)
        {
            if (IsExist(reportElements))
            {
                if (exportElements == null)
                {
                    throw new ArgumentNullException();
                }
                Type exportType = null;
                foreach (IReportElement reportElement in reportElements)
                {
                    try
                    {
                        exportType = ExportType(reportElement.GetType());
                        IFRExportElement exportElement = (IFRExportElement)exportType.Assembly.CreateInstance(exportType.FullName);
                        InnerElementConvert(reportElement, exportElement);
                        exportElements.Add(exportElement);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }
        }
        #endregion

        #region
        protected abstract void InnerElementConvert(IReportElement report, IFRExportElement export);
        protected abstract void AfterConvert(IFRExportElement export);
        #endregion

        #region
        protected virtual void ExportElement(IReportElement reportElement, IFRExportElement exportElement)
        {
            Type reportType = reportElement.GetType();
            Type exportType = exportElement.GetType();
            PropertyInfo[] props = reportType.GetProperties();
            PropertyInfo ep = null;
            string epName = null;
            if (props == null || props.Length == 0)
            {
                return;
            }
            //处理基本数据
            foreach (PropertyInfo rp in props)
            {
                epName = ExportName(rp);
                if (!string.IsNullOrEmpty(epName))
                {
                    ep = exportType.GetProperty(epName);
                    if (ep != null)
                    {
                        SetProp(rp, reportElement, ep, exportElement);
                    }
                }
            }
        }
        protected void SetProp(PropertyInfo rp, IReportElement element, PropertyInfo ep, IFRExportElement exportElement)
        {
            ep.SetValue(exportElement, rp.GetValue(element, null), null);
        }
        protected void SetProp(PropertyInfo ep, IExportElement exportElement, object value)
        {
            ep.SetValue(exportElement, value, null);
        }
        #endregion

        #region
        private bool IsConvert(IReportElement report, IFRExportElement export)
        {
            if (report != null && export != null)
            {
                string exportTypeName = ExportName(report.GetType());
                if (!string.IsNullOrEmpty(exportTypeName))
                {
                    Type convertType = SystemInfo.GetTypeFromString(exportTypeName, true, true);
                    return SystemInfo.IsType(convertType, export.GetType());
                }
            }
            return false;
        }
        private bool IsConvert(PropertyInfo prop)
        {
            object[] conAttrs = null;
            if (prop != null)
            {
                conAttrs = prop.GetCustomAttributes(typeof(ExportAttribute), true);
                if (conAttrs != null && conAttrs.Length > 0)
                {
                    return true;
                }
            }
            return false;
        }
        private bool IsConvert(Type type)
        {
            object[] conAttrs = null;
            if (type != null)
            {
                conAttrs = type.GetCustomAttributes(typeof(ExportAttribute), true);
                if (conAttrs != null && conAttrs.Length > 0)
                {
                    return true;
                }
            }
            return false;
        }
        private string ExportName(PropertyInfo prop)
        {
            object[] conAttrs = null;
            if (prop != null)
            {
                conAttrs = prop.GetCustomAttributes(typeof(ExportAttribute), true);
                if (conAttrs != null && conAttrs.Length > 0)
                {
                    ExportAttribute frcAttr = conAttrs[0] as ExportAttribute;
                    if (frcAttr != null)
                    {
                        return frcAttr.Name;
                    }
                }
            }
            return null;
        }
        private string ExportName(Type type)
        {
            object[] conAttrs = null;
            if (type != null)
            {
                conAttrs = type.GetCustomAttributes(typeof(ExportAttribute), true);
                if (conAttrs != null && conAttrs.Length > 0)
                {
                    ExportAttribute frcAttr = conAttrs[0] as ExportAttribute;
                    if (frcAttr != null)
                    {
                        return frcAttr.Name;
                    }
                }
            }
            return null;
        }
        private Type ExportType(Type type)
        {
            string exportName = ExportName(type);
            Type exportType = SystemInfo.GetTypeFromString(exportName, true, true);
            return exportType;
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