using System;
using System.Xml;

using XYS.Util;
namespace XYS.Lis.Report.Util
{
    public class XmlConfigurator
    {

        private static readonly Type declaringType = typeof(XmlConfigurator);

        private static readonly string CONFIGURATION_TAG = "lis-param";

        private static readonly string FILL_ELEMENTS_TAG = "FillElements";
        private static readonly string FILL_ELEMENT_TAG = "FillElement";

        private static readonly string REPORT_SECTIONS_TAG = "reportSections";
        private static readonly string REPORT_SECTION_TAG = "reportSection";

        private static readonly string TYPE_ATTR = "type";
        private static readonly string VALUE_ATTR = "value";
        private static readonly string NAME_ATTR = "name";

        private static readonly string FILL_ELEMENTS_ATTR = "fillElements";
       // private static readonly string FILL_TAG_ATTR = "fillTag";

        public XmlConfigurator()
        {
        }

        public static void Configure(XmlElement element)
        {
            if (element == null)
            {
                return;
            }
            string rootElementName = element.LocalName;
            if (rootElementName != CONFIGURATION_TAG)
            {
                //
                ConsoleInfo.Error(declaringType, "Xml element is - not a <" + CONFIGURATION_TAG + "> element.");
                return;
            }
            foreach (XmlNode currentNode in element.ChildNodes)
            {
                if (currentNode.NodeType == XmlNodeType.Element)
                {
                    XmlElement currentElement = (XmlElement)currentNode;
                    if (currentElement.LocalName == FILL_ELEMENTS_TAG)
                    {
                        ConfigElementMap(currentElement);
                    }
                    else if (currentElement.LocalName == REPORT_SECTIONS_TAG)
                    {
                        ConfigSectionMap(currentElement);
                    }
                    //else if (currentElement.LocalName == PAR_ITEMS_TAG)
                    //{

                    //}
                    else
                    {
                    }
                }
            }
        }

        public static void ConfigSectionMap(XmlElement element)
        {
            if (element != null)
            {
                foreach (XmlNode node in element.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        XmlElement currentElement = (XmlElement)node;
                        ConfigSection(currentElement);
                    }
                }
            }
        }
        private static void ConfigSection(XmlElement element)
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
                    ConfigManager.AddSection(section);
                }
            }
        }

        public static void ConfigElementMap(XmlElement element)
        {
            if (element != null)
            {
                foreach (XmlNode node in element.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        XmlElement currentElement = (XmlElement)node;
                        ConfigElement(currentElement);
                    }
                }
            }
        }
        private static void ConfigElement(XmlElement element)
        {
            if (element.LocalName == FILL_ELEMENT_TAG)
            {
                string name = element.GetAttribute(NAME_ATTR);
                string typeName = element.GetAttribute(TYPE_ATTR);
                if (!string.IsNullOrEmpty(typeName))
                {
                    FillElement ele = new FillElement(name, typeName);
                    ConfigManager.AddSubElement(ele);
                }
            }
        }

        //public static void ConfigParItemMap(XmlElement element)
        //{
        //    if (element != null)
        //    {
        //        foreach (XmlNode node in element.ChildNodes)
        //        {
        //            if (node.NodeType == XmlNodeType.Element)
        //            {
        //                XmlElement currentElement = (XmlElement)node;
        //                ConfigParItem(currentElement);
        //            }
        //        }
        //    }
        //}
        //private static void ConfigParItem(XmlElement element)
        //{
        //    if (element.LocalName == PAR_ITEM_TAG)
        //    {
        //        string name = element.GetAttribute(NAME_ATTR);
        //        int value = GetIntValue(element.GetAttribute(VALUE_ATTR));
        //        if (value > 0)
        //        {
        //            LisParItem parItem = new LisParItem(value, name);
        //            parItem.ImageName = element.GetAttribute(IMAGE_NAME_ATTR);
        //            ConfigManager.AddParItem(parItem);
        //        }
        //    }
        //}

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