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
        private static readonly ReporterSectionMap SECTION_MAP = new ReporterSectionMap();
        private static readonly ParItemMap PARITEM_MAP = new ParItemMap();
        private static readonly ReportModelMap MODEL_MAP = new ReportModelMap();
        private static readonly ReportElementTypeMap ELEMENT_TYPE_MAP = new ReportElementTypeMap();
        private static readonly string ROOT_TAG = "Dictionary";
        #endregion

        #region
        //private static ParamSection paramSection = (ParamSection)ConfigurationManager.GetSection("lis-param");
        #endregion

        #region
        private LisMap()
        { }
        //static LisMap()
        //{
        //    ConfigureReportSectionMap();
        //    ConfigureParItemMap();
        //    ConfigureReportElementMap();
        //    ConfigureReportModelMap();
        //}
        #endregion

        #region
        public static ReporterSection GetSection(int sectionNo)
        {
            if (SECTION_MAP.Count == 0)
            {
                //
                ConfigureReportSectionMap();
            }
            return SECTION_MAP[sectionNo];
        }
        public static ParItem GetParItem(int parItemNo)
        {
            if (PARITEM_MAP.Count == 0)
            {
                //
                ConfigureParItemMap();
            }
            return PARITEM_MAP[parItemNo];
        }
        public static ReportModel GetReportModel(int modelNo)
        {
            if (MODEL_MAP.Count == 0)
            {
                ConfigureReportModelMap();
            }
            return MODEL_MAP[modelNo];
        }
        public static ReportElementType GetElementType(string elementName)
        {
            if (ELEMENT_TYPE_MAP.Count == 0)
            {
                //初始化
                ConfigureReportElementMap();
            }
            return ELEMENT_TYPE_MAP[elementName];
        }

        public static void InitModelNo2ModelPathTable(Hashtable table)
        {
            table.Clear();
            string modelFileName;
            if (MODEL_MAP.Count == 0)
            {
                ConfigureReportModelMap();
            }
            foreach (ReportModel rm in MODEL_MAP.AllModels)
            {
                modelFileName = SystemInfo.GetFileFullName(SystemInfo.GetReportModelFilePath(), rm.ModelPath);
                table.Add(rm.ModelNo, modelFileName);
            }
        }
        public static void InitParItem2ReportModelTable(Hashtable table)
        {
            table.Clear();
            if (PARITEM_MAP.Count == 0)
            {
                //
                ConfigureParItemMap();
            }
            foreach (ParItem item in PARITEM_MAP.AllParItem)
            {
                table.Add(item.ParItemNo, item.ModelNo);
            }
        }
        public static void InitParItem2OrderNoTable(Hashtable table)
        {
            table.Clear();
            if (PARITEM_MAP.Count == 0)
            {
                //
                ConfigureParItemMap();
            }
            foreach (ParItem item in PARITEM_MAP.AllParItem)
            {
                table.Add(item.ParItemNo, item.OrderNo);
            }
        }
        public static void InitSection2PrintModelTable(Hashtable table)
        {
            table.Clear();
            if (SECTION_MAP.Count == 0)
            {
                //
                ConfigureReportSectionMap();
            }
            foreach (ReporterSection section in SECTION_MAP.AllReporterSection)
            {
                table.Add(section.SectionNo, section.ModelNo);
            }
        }
        public static void InitSection2OrderNoTable(Hashtable table)
        {
            table.Clear();
            if (SECTION_MAP.Count == 0)
            {
                //
                ConfigureReportSectionMap();
            }
            foreach (ReporterSection section in SECTION_MAP.AllReporterSection)
            {
                table.Add(section.SectionNo, section.OrderNo);
            }
        }

        public static void InitParItem2NormalImageTable(Hashtable table)
        {
            table.Clear();
            if (PARITEM_MAP.Count == 0)
            {
                ConfigureParItemMap();
            }
            byte[] imageArray;
            string imageFileName;
            foreach (ParItem item in PARITEM_MAP.AllParItem)
            {
                if (item.ImageFlag)
                {
                    imageFileName = SystemInfo.GetFileFullName(SystemInfo.GetNormalImageFilePath(), item.ImagePath);
                    imageArray = SystemInfo.ReadImageFile(imageFileName);
                    table[item.ParItemNo] = imageArray;
                }
            }
        }

        public static void InitSection2ElementTypeTable(Hashtable table, int sectionNo)
        {
            ReporterSection rs = GetSection(sectionNo);
            if (rs == null)
            {
                return;
            }
            HashSet<ReportElementType> elementSet = new HashSet<ReportElementType>();
            if (rs.ElementNameList.Count == 0)
            {
                elementSet.Add(ReportElementType.DEFAULTINFO);
                elementSet.Add(ReportElementType.DEFAULTITEM);
            }
            else
            {
                ReportElementType ret;
                foreach (string name in rs.ElementNameList)
                {
                    ret = GetElementType(name);
                    if (ret != null)
                    {
                        elementSet.Add(ret);
                    }
                }
            }
            table[sectionNo] = elementSet;
        }
        public static ReportElementTypeCollection GetSection2ElementTypeCollcetion(int sectionNo)
        {

            ReporterSection rs = GetSection(sectionNo);
            if (rs == null)
            {
                return null;
            }
            ReportElementTypeCollection retc = new ReportElementTypeCollection();
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
                    ret = GetElementType(name);
                    if (ret != null)
                    {
                        retc.Add(ret);
                    }
                }
            }
            return retc;
        }
        #endregion

        #region 未使用到的代码
        //private static void ConfigureModel()
        //{
        //    ReportModelElement modelElement;
        //    ReportModelElementCollection modelCollection = paramSection.ModelCollection;
        //    foreach (ConfigurationElement element in modelCollection)
        //    {
        //        modelElement = element as ReportModelElement;
        //        if (modelElement != null)
        //        {
        //            ConfigureModel(modelElement);
        //        }
        //    }
        //}
        //private static void ConfigureModel(ReportModelElement model)
        //{
        //    if (model.Value != null && model.ModelPath != null)
        //    {
        //        string name = model.Name;
        //        int modelNo = (int)model.Value;
        //        string path = model.ModelPath;
        //        ReportModel reportModel = new ReportModel(modelNo, path, name);
        //        MODEL_MAP.Add(reportModel);
        //    }
        //}
        //private static void ConfigureSection()
        //{
        //    ReportSectionElement sectionElement;
        //    ReportSectionElementCollection sectionElementCollection = paramSection.SectionCollection;
        //    Console.WriteLine(paramSection.ToString());
        //    foreach (ConfigurationElement element in sectionElementCollection)
        //    {
        //        sectionElement = element as ReportSectionElement;
        //        if (sectionElement != null)
        //        {
        //            ConfigureSection(sectionElement);
        //        }
        //    }
        //}
        //private static void ConfigureSection(ReportSectionElement sectionElement)
        //{
        //    if (sectionElement.Value != null)
        //    {
        //        ReporterSection section = new ReporterSection((int)sectionElement.Value, sectionElement.Name);
        //        if (sectionElement.ModelNo != null)
        //        {
        //            section.ModelNo = (int)sectionElement.ModelNo;
        //        }
        //        if (sectionElement.OrderNo != null)
        //        {
        //            section.OrderNo = (int)sectionElement.OrderNo;
        //        }
        //        if (sectionElement.ReportElementFlags != null && !sectionElement.ReportElementFlags.Equals(""))
        //        {
        //            string[] reportElements = sectionElement.ReportElementFlags.Split(new char[] { ',' });
        //            section.ClearElementNameList();
        //            foreach (string element in reportElements)
        //            {
        //                section.AddElementName(element);
        //            }
        //        }
        //        SECTION_MAP.Add(section);
        //    }
        //}
        //private static void ConfigureParItem()
        //{
        //    ParItemElement parItemElement;
        //    ParItemElementCollection parItemCollection = paramSection.ParItemCollection;
        //    foreach (ConfigurationElement element in parItemCollection)
        //    {
        //        parItemElement = element as ParItemElement;
        //        if (parItemElement != null)
        //        {
        //            ConfigureParItem(parItemElement);
        //        }
        //    }
        //}
        //private static void ConfigureParItem(ParItemElement parItemElement)
        //{
        //    if (parItemElement.Value != null)
        //    {
        //        ParItem parItem = new ParItem((int)parItemElement.Value, parItemElement.Name);
        //        if (parItemElement.ModelNo != null)
        //        {
        //            parItem.ModelNo = (int)parItemElement.ModelNo;
        //        }
        //        if (parItemElement.OrderNo != null)
        //        {
        //            parItem.OrderNo = (int)parItemElement.OrderNo;
        //        }
        //        parItem.ImageFlag = parItemElement.CommonImageFlag;
        //        parItem.ImagePath = parItemElement.CommonImagePath;
        //        PARITEM_MAP.Add(parItem);
        //    }
        //}
        //private static void ConfigureReportElement()
        //{
        //    ReportElementElement reportElement;
        //    ReportElementElementCollection reportElementCollection = paramSection.ReportElementCollection;
        //    foreach (ConfigurationElement element in reportElementCollection)
        //    {
        //        reportElement = element as ReportElementElement;
        //        if (reportElement != null)
        //        {
        //            ConfigureReportElement(reportElement);
        //        }
        //    }
        //}
        //private static void ConfigureReportElement(ReportElementElement reportElement)
        //{
        //    if (reportElement.Type != null && !reportElement.Type.Equals(""))
        //    {
        //        ReportElementType elementType = new ReportElementType(reportElement.Type);
        //        elementType.ElementName = reportElement.Name;
        //        if (reportElement.Value != null)
        //        {
        //            elementType.ElementTag = GetElementTag((int)reportElement.Value);
        //        }
        //        ELEMENT_TYPE_MAP.Add(elementType);
        //    }
        //}
        //private static ReportElementTag GetElementTag(int tag)
        //{
        //    if (tag <= 0 || tag > 7)
        //    {
        //        return ReportElementTag.NoneElement;
        //    }
        //    else
        //    {
        //        return (ReportElementTag)tag;
        //    }
        //}
        #endregion

        #region
        private static void ConfigureReportModelMap()
        {
            XmlParamConfigurator config = new XmlParamConfigurator();
            XmlElement paramElement = XmlConfigurator.GetParamConfigurationElement();
            if (paramElement != null)
            {
                ReportReport.Debug(declaringType, "LisMap:configuring ReportModelMap");
                config.ConfigReportModelMap(paramElement, MODEL_MAP);
            }
        }
        private static void ConfigureReportElementMap()
        {
            XmlParamConfigurator config = new XmlParamConfigurator();
            XmlElement paramElement = XmlConfigurator.GetParamConfigurationElement();
            if (paramElement != null)
            {
                ReportReport.Debug(declaringType, "LisMap:configuring ReportElementTypeMap");
                config.ConfigReportElementMap(paramElement, ELEMENT_TYPE_MAP);
            }
        }
        private static void ConfigureReportSectionMap()
        {
            XmlParamConfigurator config = new XmlParamConfigurator();
            XmlElement paramElement = XmlConfigurator.GetParamConfigurationElement();
            if (paramElement != null)
            {
                ReportReport.Debug(declaringType, "LisMap:configuring ReportSectionMap");
                config.ConfigSectionMap(paramElement, SECTION_MAP);
            }
        }
        public static void ConfigureParItemMap()
        {
            XmlParamConfigurator config = new XmlParamConfigurator();
            XmlElement paramElement = XmlConfigurator.GetParamConfigurationElement();
            if (paramElement != null)
            {
                ReportReport.Debug(declaringType, "LisMap:configuring ParItemMap");
                config.ConfigParItemMap(paramElement, PARITEM_MAP);
            }
        }
        #endregion

        #region
        public static void InitDataSetXmlStruct()
        {
            string fileFullName = SystemInfo.GetFileFullName(SystemInfo.GetDataStructFilePath(), "ReportTables.frd");
            if (ELEMENT_TYPE_MAP.Count == 0)
            {
                ConfigureReportElementMap();
            }
            ReportReport.Debug(declaringType, "LisMap:Start--make the data struct xml file " + fileFullName + " by report elemnt");
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(dec);
            XmlNode root = doc.CreateNode(XmlNodeType.Element, ROOT_TAG, null);
            foreach (ReportElementType element in ELEMENT_TYPE_MAP.AllElementTypes)
            {
                switch (element.ElementTag)
                {
                    case ReportElementTag.ReportElement:
                    case ReportElementTag.InfoElement:
                    case ReportElementTag.ItemElement:
                    case ReportElementTag.CustomElement:
                        XMLTools.ConvertObj2Xml(doc, root, element.ElementType);
                        break;
                    case ReportElementTag.GraphElement:
                        XMLTools.Image2Xml(doc, root);
                        break;
                    case ReportElementTag.NoneElement:
                        break;
                    default:
                        break;
                }
            }
            doc.AppendChild(root);
            if (SystemInfo.IsFileExist(fileFullName))
            {
                if (!SystemInfo.DeleteFile(fileFullName))
                {
                    throw new Exception("can not delete the file " + fileFullName);
                }
            }
            try
            {
                doc.Save(fileFullName);
                ReportReport.Debug(declaringType, "LisMap:End--make the data struct xml file " + fileFullName + " by report elemnt");
            }
            catch (Exception ex)
            {
                ReportReport.Error(declaringType, "LisMap:" + ex.Message);
                throw ex;
            }
        }
        public static void SetDataSet(DataSet ds)
        {
            if (ELEMENT_TYPE_MAP.Count == 0)
            {
                ConfigureReportElementMap();
            }
            foreach (ReportElementType element in ELEMENT_TYPE_MAP.AllElementTypes)
            {
                switch (element.ElementTag)
                {
                    case ReportElementTag.ReportElement:
                    case ReportElementTag.InfoElement:
                    case ReportElementTag.ItemElement:
                    case ReportElementTag.CustomElement:
                        ConvertObj2Table(ds, element.ElementType);
                        break;
                    case ReportElementTag.GraphElement:
                        Image2Table(ds);
                        break;
                    case ReportElementTag.NoneElement:
                        break;
                    default:
                        break;
                }
            }
        }
        private static void ConvertObj2Table(DataSet ds, Type elementType)
        {
            Convert2XmlAttribute cxa;
            DataTable dt = new DataTable();
            dt.TableName = elementType.Name;
            PropertyInfo[] props = elementType.GetProperties();
            if (props == null || props.Length == 0)
            {
                return;
            }
            foreach (PropertyInfo pro in props)
            {
                cxa = (Convert2XmlAttribute)Attribute.GetCustomAttribute(pro, typeof(Convert2XmlAttribute));
                if (cxa != null && cxa.IsConvert)
                {
                    dt.Columns.Add(pro.Name, pro.PropertyType);
                }
            }
            ds.Tables.Add(dt);
        }
        private static void Image2Table(DataSet ds)
        {
            DataTable dt = new DataTable();
            dt.TableName = XMLTools.Image_Table_Name;
            for (int i = 0; i < XMLTools.Image_Column_Count; i++)
            {
                dt.Columns.Add(XMLTools.Image_Column_Prex + i, SystemInfo.GetTypeFromString(XMLTools.Image_Column_DataType, true, true));
            }
            ds.Tables.Add(dt);
        }
        #endregion
    }
}