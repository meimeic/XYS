﻿using System;
using System.Xml;

using XYS.Lis.Core;
using XYS.Lis.Util;
using XYS.Lis.Model;
using XYS.Model;

namespace XYS.Lis.Config
{
    public class XmlParamConfigurator
    {
        private static readonly string CONFIGURATION_TAG = "lis-param";
        private static readonly string REPORT_ELEMENTS_TAG = "reportElements";
        private static readonly string REPORT_ELEMENT_TAG = "reportElement";
        private static readonly string REPORT_SECTIONS_TAG = "reportSections";
        private static readonly string REPORT_SECTION_TAG = "reportSection";
        private static readonly string PAR_ITEMS_TAG = "parItems";
        private static readonly string PAR_ITEM_TAG = "parItem";
        private static readonly string REPORT_MODELS_TAG = "reportModels";
        private static readonly string REPORT_MODEL_TAG = "reportModel";

        private static readonly string NAME_ATTR = "name";
        private static readonly string TYPE_ATTR = "type";
        private static readonly string EXPORT_TYPE_ATTR = "exportType";
        private static readonly string VALUE_ATTR = "value";
        private static readonly string REPORT_ELEMENT_REFS = "reportElementRefs";
        private static readonly string MODEL_NO_ATTR = "modelNo";
        private static readonly string ORDER_NO_ATTR = "orderNo";
        private static readonly string IMAGE_FLAG_ATTR = "imageFlag";
        private static readonly string IMAGE_PATH_ATTR = "imagePath";
        private static readonly string MODEL_PATH_ATTR = "modelPath";

        public XmlParamConfigurator()
        {
 
        }

        public void ConfigSectionMap(XmlElement element, ReporterSectionMap sectionMap)
        {
            XmlElement sectionsElement = GetTargetElement(element, REPORT_SECTIONS_TAG);
            if (sectionsElement != null)
            {
                foreach (XmlNode node in sectionsElement.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        XmlElement currentElement = (XmlElement)node;
                        ConfigSection(currentElement, sectionMap);
                    }
                }
            }
        }
        protected void ConfigSection(XmlElement element, ReporterSectionMap sectionMap)
        {
            if (element.LocalName == REPORT_SECTION_TAG)
            {
                string name = element.GetAttribute(NAME_ATTR);
                int value = GetIntValue(element.GetAttribute(VALUE_ATTR));
                if (value != -1)
                {
                    ReporterSection section = new ReporterSection(value, name);
                    string reportElementRefs = element.GetAttribute(REPORT_ELEMENT_REFS);
                    if (reportElementRefs != null && !reportElementRefs.Equals(""))
                    {
                        string[] reportElements = reportElementRefs.Split(new char[] { ',' });
                        foreach (string s in reportElements)
                        {
                            section.AddElementName(s);
                        }
                    }
                    int modelNo = GetIntValue(element.GetAttribute(MODEL_NO_ATTR));
                    int orderNo = GetIntValue(element.GetAttribute(ORDER_NO_ATTR));
                    if (modelNo >= 0)
                    {
                        section.ModelNo = modelNo;
                    }
                    if (orderNo >= 0)
                    {
                        section.OrderNo = orderNo;
                    }
                    sectionMap.Add(section);
                }
            }
        }

        public void ConfigReportElementMap(XmlElement element, ReportElementTypeMap reportTypeMap)
        {
            XmlElement reportElementElement = GetTargetElement(element, REPORT_ELEMENTS_TAG);
            if (reportElementElement != null)
            {
                foreach (XmlNode node in reportElementElement.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        XmlElement currentElement = (XmlElement)node;
                        ConfigReportElement(currentElement, reportTypeMap);
                    }
                }
            }
        }
        protected void ConfigReportElement(XmlElement element, ReportElementTypeMap reportTypeMap)
        {
            if (element.LocalName == REPORT_ELEMENT_TAG)
            {
                string typeName = element.GetAttribute(TYPE_ATTR);
                string exportName = element.GetAttribute(EXPORT_TYPE_ATTR);
                string name = element.GetAttribute(NAME_ATTR);
                int value = GetIntValue(element.GetAttribute(VALUE_ATTR));
                Type type = SystemInfo.GetTypeFromString(typeName, true, true);
                Type exportType = SystemInfo.GetTypeFromString(exportName, true, true);
                if (type != null && name != null)
                {
                    ReportElementType elementType = new ReportElementType(name, type, exportType);
                    elementType.ElementTag = GetElementTag(value);
                    reportTypeMap.Add(elementType);
                }
            }
        }

        public void ConfigParItemMap(XmlElement element, ParItemMap parItemMap)
        {
            XmlElement parItemElement = GetTargetElement(element, PAR_ITEMS_TAG);
            if (parItemElement != null)
            {
                foreach (XmlNode node in parItemElement.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        XmlElement currentElement = (XmlElement)node;
                        ConfigParItem(currentElement, parItemMap);
                    }
                }
            }
        }
        protected void ConfigParItem(XmlElement element, ParItemMap parItemMap)
        {
            if (element.LocalName == PAR_ITEM_TAG)
            {
                string name = element.GetAttribute(NAME_ATTR);
                int value = GetIntValue(element.GetAttribute(VALUE_ATTR));
                if (value > 0)
                {
                    ParItem parItem = new ParItem(value, name);
                    int modelNo = GetIntValue(element.GetAttribute(MODEL_NO_ATTR));
                    int orderNo = GetIntValue(element.GetAttribute(ORDER_NO_ATTR));
                    if (modelNo >= 0)
                    {
                        parItem.ModelNo = modelNo;
                    }
                    if (orderNo >= 0)
                    {
                        parItem.OrderNo = orderNo;
                    }
                    parItem.ImageFlag = GetBoolean(element.GetAttribute(IMAGE_FLAG_ATTR));
                    parItem.ImagePath = element.GetAttribute(IMAGE_PATH_ATTR);
                    parItemMap.Add(parItem);
                }
            }
        }

        public void ConfigReportModelMap(XmlElement element,ReportModelMap modelMap)
        {
            XmlElement mapElement = GetTargetElement(element, REPORT_MODELS_TAG);
            if (mapElement != null)
            {
                foreach (XmlNode node in mapElement.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        XmlElement currentElement = (XmlElement)node;
                        ConfigReportModel(currentElement, modelMap);
                    }
                }
            }
        }
        protected void ConfigReportModel(XmlElement element, ReportModelMap modelMap)
        {
            if (element.LocalName == REPORT_MODEL_TAG)
            {
                int value = GetIntValue(element.GetAttribute(VALUE_ATTR));
                string path=element.GetAttribute(MODEL_PATH_ATTR);
                if (value > 0)
                {
                    ReportModel model = new ReportModel(value, path);
                    model.ModelName = element.GetAttribute(NAME_ATTR);
                    modelMap.Add(model);
                }
            }
        }

        protected XmlElement GetTargetElement(XmlElement sourceElement, string targetTag)
        {
            if (sourceElement.LocalName != CONFIGURATION_TAG)
            {
                //错误
                return null;
            }
            foreach (XmlNode node in sourceElement.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    XmlElement element = (XmlElement)node;
                    if (element.LocalName == targetTag)
                    {
                        return element;
                    }
                }
            }
            return null;
        }
        protected int GetIntValue(string v)
        {
            if (v == null || v.Equals(""))
            {
                return -1;
            }
            int result;
            bool f = SystemInfo.TryParse(v, out result);
            if (f)
            {
                return result;
            }
            else
            {
                return -1;
            }
        }
        protected bool GetBoolean(string b)
        {
            if (b == null || b.Equals(""))
            {
                return false;
            }
            bool result;
            bool f = Boolean.TryParse(b, out result);
            if (f)
            {
                return result;
            }
            return false;
        }
        private static ReportElementTag GetElementTag(int tag)
        {
            if (tag <= 0 || tag > 7)
            {
                return ReportElementTag.NoneElement;
            }
            else
            {
                return (ReportElementTag)tag;
            }
        }
    }
}