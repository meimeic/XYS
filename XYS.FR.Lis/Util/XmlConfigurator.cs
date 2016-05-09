using System;
using System.IO;
using System.Xml;

using XYS.Util;
namespace XYS.FR.Lis.Util
{
    public class XmlConfigurator
    {
        private static readonly Type declaringType = typeof(XmlConfigurator);

        private static readonly string CONFIGURATION_TAG = "lisFR";

        private static readonly string LIS_GROUPS_TAG = "lisGroups";
        private static readonly string LIS_GROUP_TAG = "lisGroup";

        private static readonly string LIS_ITEMS_TAG = "lisItems";
        private static readonly string LIS_ITEM_TAG = "lisItem";

        private static readonly string PRINT_MODELS_TAG = "printModels";
        private static readonly string PRINT_MODEL_TAG = "printModel";

        private static readonly string NAME_ATTR = "name";
        private static readonly string VALUE_ATTR = "value";
        private static readonly string PATH_ATTR = "path";
        private static readonly string RELATIVE_PATH_ATTR = "relativePath";
        private static readonly string MODEL_NO_ATTR = "modelNo";
        private static readonly string ORDER_NO_ATTR = "orderNo";


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
                    if (currentElement.LocalName == LIS_GROUPS_TAG)
                    {
                        ConfigLisGroupMap(currentElement);
                    }
                    else if (currentElement.LocalName == LIS_ITEMS_TAG)
                    {
                        ConfigLisItemMap(currentElement);
                    }
                    else if (currentElement.LocalName == PRINT_MODELS_TAG)
                    {
                        ConfigPrintModelMap(currentElement);
                    }
                    else
                    {
                    }
                }
            }
        }

        public static void ConfigLisGroupMap(XmlElement element)
        {
            if (element != null)
            {
                foreach (XmlNode node in element.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        XmlElement currentElement = (XmlElement)node;
                        ConfigLisGroup(currentElement);
                    }
                }
            }
        }
        private static void ConfigLisGroup(XmlElement element)
        {
            if (element.LocalName == LIS_GROUP_TAG)
            {
                string name = element.GetAttribute(NAME_ATTR);
                int value = GetIntValue(element.GetAttribute(VALUE_ATTR));
                if (value != -1)
                {
                    LisGroup group = new LisGroup(value, name);
                    //配置属性
                   int modelNo=GetIntValue(element.GetAttribute(MODEL_NO_ATTR));
                    if(modelNo>0)
                    {
                        group.PrintModelNo=modelNo;
                    }
                    int orderNo=GetIntValue(element.GetAttribute(ORDER_NO_ATTR));
                    if(orderNo>0)
                    {
                        group.OrderNo=orderNo;
                    }
                    ConfigManager.AddGroup(group);
                }
            }
        }

        public static void ConfigLisItemMap(XmlElement element)
        {
            if (element != null)
            {
                foreach (XmlNode node in element.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        XmlElement currentElement = (XmlElement)node;
                        ConfigLisItem(currentElement);
                    }
                }
            }
        }
        private static void ConfigLisItem(XmlElement element)
        {
            if (element.LocalName == LIS_ITEM_TAG)
            {
                string name = element.GetAttribute(NAME_ATTR);
                int value = GetIntValue(element.GetAttribute(VALUE_ATTR));
                if (value != -1)
                {
                    LisItem item = new LisItem(value, name);
                    //配置属性
                    int modelNo = GetIntValue(element.GetAttribute(MODEL_NO_ATTR));
                    if (modelNo > 0)
                    {
                        item.PrintModelNo = modelNo;
                    }
                    int orderNo = GetIntValue(element.GetAttribute(ORDER_NO_ATTR));
                    if (orderNo > 0)
                    {
                        item.OrderNo = orderNo;
                    }
                    ConfigManager.AddLisItem(item);
                }
            }
        }

        public static void ConfigPrintModelMap(XmlElement element)
        {
            if (element != null)
            {
                string relativePath = element.GetAttribute(RELATIVE_PATH_ATTR);
                foreach (XmlNode node in element.ChildNodes)
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        XmlElement currentElement = (XmlElement)node;
                        ConfigPrintModel(currentElement, relativePath);
                    }
                }
            }
        }
        private static void ConfigPrintModel(XmlElement element, string relativePath)
        {
            if (element.LocalName == PRINT_MODEL_TAG)
            {
                string name = element.GetAttribute(NAME_ATTR);
                int value = GetIntValue(element.GetAttribute(VALUE_ATTR));
                if (value != -1)
                {
                    LisPrintModel model = new LisPrintModel(value, name);
                    string path = element.GetAttribute(PATH_ATTR);
                    model.Path = Path.Combine(SystemInfo.ApplicationBaseDirectory, relativePath, path);
                    ConfigManager.AddPrintModel(model);
                }
            }
        }

        #region 辅助方法
        /// <summary>
        /// 尝试将字符串转换成数字
        /// </summary>
        /// <param name="v"></param>
        /// <returns>转换成功返回相应数值，失败返回-1</returns>
        private static int GetIntValue(string v)
        {
            if (string.IsNullOrEmpty(v))
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
        #endregion
    }
}