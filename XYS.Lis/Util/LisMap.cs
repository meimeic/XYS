using System;
using System.Xml;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using XYS.Lis.Config;
using XYS.Lis.Core;
using XYS.Common;
using XYS.Model;
using XYS.Lis.Model;

namespace XYS.Lis.Util
{
    public class LisMap
    {
        #region
        private static readonly Type declaringType = typeof(LisMap);
        #endregion

        #region
        private LisMap()
        { }
        #endregion

        #region

        public static void InitModelNo2ModelPathTable(Hashtable table)
        {
            table.Clear();
            string modelFileName;
            ReportModelMap modelMap = new ReportModelMap();
            ConfigureReportModelMap(modelMap);
            foreach (ReportModel rm in modelMap.AllModels)
            {
                modelFileName = SystemInfo.GetFileFullName(SystemInfo.GetReportModelFilePath(), rm.ModelPath);
                table.Add(rm.ModelNo, modelFileName);
            }
        }
        public static void InitParItem2ReportModelTable(Hashtable table)
        {
            table.Clear();
            ParItemMap parItemMap = new ParItemMap();
            ConfigureParItemMap(parItemMap);
            foreach (ParItem item in parItemMap.AllParItem)
            {
                table.Add(item.ParItemNo, item.ModelNo);
            }
        }
        public static void InitParItem2OrderNoTable(Hashtable table)
        {
            table.Clear();
            ParItemMap parItemMap = new ParItemMap();
            ConfigureParItemMap(parItemMap);
            foreach (ParItem item in parItemMap.AllParItem)
            {
                table.Add(item.ParItemNo, item.OrderNo);
            }
        }
        public static void InitSection2PrintModelTable(Hashtable table)
        {
            table.Clear();
            ReporterSectionMap sectionMap = new ReporterSectionMap();
            ConfigureReportSectionMap(sectionMap);
            foreach (ReporterSection section in sectionMap.AllReporterSection)
            {
                table.Add(section.SectionNo, section.ModelNo);
            }
        }
        public static void InitSection2OrderNoTable(Hashtable table)
        {
            table.Clear();
            ReporterSectionMap sectionMap = new ReporterSectionMap();
            ConfigureReportSectionMap(sectionMap);
            foreach (ReporterSection section in sectionMap.AllReporterSection)
            {
                table.Add(section.SectionNo, section.OrderNo);
            }
        }

        public static void InitParItem2NormalImageTable(Hashtable table)
        {
            byte[] imageArray;
            string imageFileName;
            table.Clear();
            ParItemMap parItemMap = new ParItemMap();
            ConfigureParItemMap(parItemMap);
            foreach (ParItem item in parItemMap.AllParItem)
            {
                if (item.ImageFlag)
                {
                    imageFileName = SystemInfo.GetFileFullName(SystemInfo.GetNormalImageFilePath(), item.ImagePath);
                    imageArray = SystemInfo.ReadImageFile(imageFileName);
                    table[item.ParItemNo] = imageArray;
                }
            }
        }

        public static void InitSection2ElementTypeTable(Hashtable table)
        {
            ReportElementTypeCollection retc;
            ReporterSectionMap sectionMap = new ReporterSectionMap();
            ReportElementTypeMap elementTypeMap = new ReportElementTypeMap();
            ConfigureReportSectionMap(sectionMap);
            ConfigureReportElementMap(elementTypeMap);
            foreach (ReporterSection rs in sectionMap.AllReporterSection)
            {
                retc = new ReportElementTypeCollection(4);
                if (rs.ElementNameList.Count == 0)
                {
                    retc.Add(ReportElementType.DEFAULTINFO);
                    retc.Add(ReportElementType.DEFAULTITEM);
                }
                else
                {
                    ReportElementType ret;
                    foreach (string name in rs.ElementNameList)
                    {
                        ret = elementTypeMap[name];
                        if (ret != null)
                        {
                            retc.Add(ret);
                        }
                    }
                }
                table[rs.SectionNo] = retc;
            }
        }

        #endregion

        #region
        private static void ConfigureReportModelMap(ReportModelMap modelMap)
        {
            XmlParamConfigurator config = new XmlParamConfigurator();
            XmlElement paramElement = XmlConfigurator.GetParamConfigurationElement();
            if (paramElement != null)
            {
                ReportReport.Debug(declaringType, "LisMap:configuring ReportModelMap");
                config.ConfigReportModelMap(paramElement, modelMap);
            }
        }
        private static void ConfigureReportElementMap(ReportElementTypeMap elementTypeMap)
        {
            XmlParamConfigurator config = new XmlParamConfigurator();
            XmlElement paramElement = XmlConfigurator.GetParamConfigurationElement();
            if (paramElement != null)
            {
                ReportReport.Debug(declaringType, "LisMap:configuring ReportElementTypeMap");
                config.ConfigReportElementMap(paramElement, elementTypeMap);
            }
        }
        private static void ConfigureReportSectionMap(ReporterSectionMap sectionMap)
        {
            XmlParamConfigurator config = new XmlParamConfigurator();
            XmlElement paramElement = XmlConfigurator.GetParamConfigurationElement();
            if (paramElement != null)
            {
                ReportReport.Debug(declaringType, "LisMap:configuring ReportSectionMap");
                config.ConfigSectionMap(paramElement, sectionMap);
            }
        }
        public static void ConfigureParItemMap(ParItemMap parItemMap)
        {
            XmlParamConfigurator config = new XmlParamConfigurator();
            XmlElement paramElement = XmlConfigurator.GetParamConfigurationElement();
            if (paramElement != null)
            {
                ReportReport.Debug(declaringType, "LisMap:configuring ParItemMap");
                config.ConfigParItemMap(paramElement, parItemMap);
            }
        }
        #endregion

        #region
        //public static void InitDataSetXmlStruct()
        //{
        //    string fileFullName = SystemInfo.GetFileFullName(SystemInfo.GetDataStructFilePath(), "ReportTables.frd");
        //    if (ELEMENT_TYPE_MAP.Count == 0)
        //    {
        //        ConfigureReportElementMap();
        //    }
        //    ReportReport.Debug(declaringType, "LisMap:Start--make the data struct xml file " + fileFullName + " by report elemnt");
        //    XmlDocument doc = new XmlDocument();
        //    XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
        //    doc.AppendChild(dec);
        //    XmlNode root = doc.CreateNode(XmlNodeType.Element, ROOT_TAG, null);
        //    foreach (ReportElementType element in ELEMENT_TYPE_MAP.AllElementTypes)
        //    {
        //        switch (element.ElementTag)
        //        {
        //            case ReportElementTag.ReportElement:
        //            case ReportElementTag.InfoElement:
        //            case ReportElementTag.ItemElement:
        //            case ReportElementTag.CustomElement:
        //                XMLTools.ConvertObj2Xml(doc, root, element.ExportType);
        //                break;
        //            case ReportElementTag.GraphElement:
        //                XMLTools.Image2Xml(doc, root);
        //                break;
        //            case ReportElementTag.KVElement:
        //            case ReportElementTag.NoneElement:
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    doc.AppendChild(root);
        //    if (SystemInfo.IsFileExist(fileFullName))
        //    {
        //        if (!SystemInfo.DeleteFile(fileFullName))
        //        {
        //            throw new Exception("can not delete the file " + fileFullName);
        //        }
        //    }
        //    try
        //    {
        //        doc.Save(fileFullName);
        //        ReportReport.Debug(declaringType, "LisMap:End--make the data struct xml file " + fileFullName + " by report elemnt");
        //    }
        //    catch (Exception ex)
        //    {
        //        ReportReport.Error(declaringType, "LisMap:" + ex.Message);
        //        throw ex;
        //    }
        //}
        //public static void SetDataSet(DataSet ds)
        //{
        //    if (ELEMENT_TYPE_MAP.Count == 0)
        //    {
        //        ConfigureReportElementMap();
        //    }
        //    foreach (ReportElementType element in ELEMENT_TYPE_MAP.AllElementTypes)
        //    {
        //        switch (element.ElementTag)
        //        {
        //            case ReportElementTag.ReportElement:
        //            case ReportElementTag.InfoElement:
        //            case ReportElementTag.ItemElement:
        //            case ReportElementTag.CustomElement:
        //                ConvertObj2Table(ds, element.ExportType);
        //                break;
        //            case ReportElementTag.GraphElement:
        //                Image2Table(ds);
        //                break;
        //            case ReportElementTag.KVElement:
        //            case ReportElementTag.NoneElement:
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}
        //private static void ConvertObj2Table(DataSet ds, Type elementType)
        //{
        //    // ExportAttribute cxa;
        //    DataTable dt = new DataTable();
        //    dt.TableName = elementType.Name;
        //    PropertyInfo[] props = elementType.GetProperties();
        //    if (props == null || props.Length == 0)
        //    {
        //        return;
        //    }
        //    foreach (PropertyInfo pro in props)
        //    {
        //        //cxa = (ExportAttribute)Attribute.GetCustomAttribute(pro, typeof(ExportAttribute));
        //        //if (cxa != null && cxa.IsConvert)
        //        if (pro.PropertyType == typeof(string) || pro.PropertyType == typeof(int) || pro.PropertyType == typeof(DateTime) || pro.PropertyType == typeof(byte[]))
        //        {
        //            dt.Columns.Add(pro.Name, pro.PropertyType);
        //        }
        //    }
        //    ds.Tables.Add(dt);
        //}
        //private static void Image2Table(DataSet ds)
        //{
        //    DataTable dt = new DataTable();
        //    dt.TableName = XMLTools.Image_Table_Name;
        //    for (int i = 0; i < XMLTools.Image_Column_Count; i++)
        //    {
        //        dt.Columns.Add(XMLTools.Image_Column_Prex + i, SystemInfo.GetTypeFromString(XMLTools.Image_Column_DataType, true, true));
        //    }
        //    ds.Tables.Add(dt);
        //}
        #endregion
    }
}