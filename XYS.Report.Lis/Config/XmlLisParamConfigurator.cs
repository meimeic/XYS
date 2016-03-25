using System;
using System.Xml;

using XYS.Util;
using XYS.Report.Lis.Core;
namespace XYS.Report.Lis.Config
{
    public class XmlLisParamConfigurator
    {
        private static readonly string CONFIGURATION_TAG = "lis-param";

        private static readonly string REPORT_ELEMENTS_TAG = "reportElements";
        private static readonly string REPORT_ELEMENT_TAG = "reportElement";

        private static readonly string REPORT_SECTIONS_TAG = "reportSections";
        private static readonly string REPORT_SECTION_TAG = "reportSection";

        private static readonly string PAR_ITEMS_TAG = "parItems";
        private static readonly string PAR_ITEM_TAG = "parItem";

        private static readonly string TYPE_ATTR = "type";
        private static readonly string VALUE_ATTR = "value";
        private static readonly string NAME_ATTR = "name";
        //private static readonly string IMAGE_FLAG_ATTR = "imageFlag";
        private static readonly string IMAGE_NAME_ATTR = "imageName";

        private static readonly string FILL_ELEMENTS_ATTR = "fillElements";
        private static readonly string FILL_TAG_ATTR = "fillTag";

        public XmlLisParamConfigurator()
        {
        }

        public static void ConfigSectionMap(LisSectionMap sectionMap)
        {
            XmlElement sectionsElement = GetTargetElement(REPORT_SECTIONS_TAG);
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
        private static void ConfigSection(XmlElement element, LisSectionMap sectionMap)
        {
            if (element.LocalName == REPORT_SECTION_TAG)
            {
                string name = element.GetAttribute(NAME_ATTR);
                int value = GetIntValue(element.GetAttribute(VALUE_ATTR));
                if (value != -1)
                {
                    LisSection section = new LisSection(value, name);
                    //配置属性
                    string fillElementsStr = element.GetAttribute(FILL_ELEMENTS_ATTR);
                    if (!string.IsNullOrEmpty(fillElementsStr))
                    {
                        string[] fillElements = fillElementsStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string ele in fillElements)
                        {
                            section.AddFillElement(ele);
                        }
                    }
                    sectionMap.Add(section);
                }
            }
        }

        public static void ConfigElementMap(LisElementMap elementMap)
        {
            XmlElement reportElements = GetTargetElement(REPORT_ELEMENTS_TAG);
            if (reportElements != null)
            {
                foreach (XmlNode node in reportElements.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        XmlElement currentElement = (XmlElement)node;
                        ConfigElement(currentElement, elementMap);
                    }
                }
            }
        }
        private static void ConfigElement(XmlElement element, LisElementMap elementMap)
        {
            if (element.LocalName == REPORT_ELEMENT_TAG)
            {
                string name = element.GetAttribute(NAME_ATTR);
                string typeName = element.GetAttribute(TYPE_ATTR);
                if (!string.IsNullOrEmpty(typeName))
                {
                    LisElement ele = new LisElement(name, typeName);
                    elementMap.Add(ele);
                }
            }
        }

        public static void ConfigParItemMap(LisParItemMap parItemMap)
        {
            XmlElement parItemElement = GetTargetElement(PAR_ITEMS_TAG);
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
        private static void ConfigParItem(XmlElement element, LisParItemMap parItemMap)
        {
            if (element.LocalName == PAR_ITEM_TAG)
            {
                string name = element.GetAttribute(NAME_ATTR);
                int value = GetIntValue(element.GetAttribute(VALUE_ATTR));
                if (value > 0)
                {
                    LisParItem parItem = new LisParItem(value, name);
                    parItem.ImageName = element.GetAttribute(IMAGE_NAME_ATTR);
                    parItemMap.Add(parItem);
                }
            }
        }

        private static XmlElement GetTargetElement(string targetTag)
        {
            XmlElement configElement = XmlConfigurator.GetParamConfigurationElement(CONFIGURATION_TAG);
            if (configElement != null)
            {
                foreach (XmlNode node in configElement.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        if (node.LocalName == targetTag)
                        {
                            return node as XmlElement;
                        }
                    }
                }
            }
            return null;
        }
        private static XmlElement GetTargetElement(XmlElement sourceElement, string targetTag)
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

        #region 辅助方法
        /// <summary>
        /// 尝试将字符串转换成数字
        /// </summary>
        /// <param name="v"></param>
        /// <returns>转换成功返回相应数值，失败返回-1</returns>
        private static int GetIntValue(string v)
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
        private static bool GetBoolean(string b)
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
        //private static FillTypeTag GetFillTag(int tag)
        //{
        //    if (tag <= 0 || tag > 3)
        //    {
        //        return FillTypeTag.None;
        //    }
        //    else
        //    {
        //        return (FillTypeTag)tag;
        //    }
        //}
        #endregion
    }
}