using System;
using System.Collections.Generic;
using System.Xml;
using System.Data;
using System.Reflection;

using XYS.Common;
namespace XYS.Lis.Util
{
    public class XMLTools
    {
        #region
        private static readonly string TABLE_TAG = "TableDataSource";
        private static readonly string COLUMN_TAG = "Column";
        private static readonly string NAME_ATTR = "Name";
        private static readonly string REF_NAME_ATTR = "ReferenceName";
        private static readonly string DATA_TYPE_ATTR = "DataType";
        private static readonly string ENABLE_ATTR = "Enabled";
        private static readonly string PROP_NAME_ATTR = "PropName";
        private static readonly string PROP_NAME_ATTR_VALUE = "Column";

        private static readonly string DEFAULT_DATA_TYPE_ATTR_VALUE = "System.Int32";
        private static readonly string DEFAULT_ENABLE_ATTR = "true";

        public static readonly int Image_Column_Count = 10;
        public static readonly string Image_Table_Name = "ReportImage";
        public static readonly string Image_Column_Prex = "Image";
        public static readonly string Image_Column_DataType = "System.Byte[]";
        #endregion

        #region 公共静态方法

        //根据xml文件生成dataset
        public static DataSet ConvertFRDFile2DataSet(string frdFile)
        {
            DataSet ds = new DataSet();
            XmlDocument doc = new XmlDocument();
            doc.Load(frdFile);
            XmlNode root = doc.SelectSingleNode("Dictionary");
            //设置table
            XmlNodeList XmlTables = root.SelectNodes("TableDataSource");
            DataTable dt;
            for (int i = 0; i < XmlTables.Count; i++)
            {
                XmlNode node = XmlTables[i];
                dt = ConvertXml2DataTable(node);
                ds.Tables.Add(dt);
            }
            //设置 ralation
            XmlNodeList XmlRelations = root.SelectNodes("Relation");
            foreach (XmlNode node in XmlRelations)
            {
                ConvertXml2TableRelation(node, ds);
            }
            return ds;
        }

        #endregion
        #region 静态私有方法

        //设置数据表
        private static DataTable ConvertXml2DataTable(XmlNode node)
        {
            string TableName = node.Attributes["Name"].Value;
            DataTable dt = new DataTable();
            dt.TableName = TableName;
            XmlNodeList XmlColumns = node.ChildNodes;
            string ColumnName;
            string ColumnType;
            foreach (XmlNode n in XmlColumns)
            {
                ColumnName = n.Attributes["Name"].Value;
                ColumnType = n.Attributes["DataType"].Value;
                dt.Columns.Add(ColumnName, Type.GetType(ColumnType));
            }
            return dt;
        }
        //设置数据表关系
        private static void ConvertXml2TableRelation(XmlNode node, DataSet ds)
        {
            string relationName = node.Attributes["Name"].Value;
            string parentTableName = node.Attributes["ParentDataSource"].Value;
            string childTableName = node.Attributes["ChildDataSource"].Value;
            string[] parentColumnsName = node.Attributes["ParentColumns"].Value.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string[] childColumnsName = node.Attributes["ChildColumns"].Value.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            ds.Relations.Add(relationName, GetRelationColumns(ds.Tables[parentTableName], parentColumnsName), GetRelationColumns(ds.Tables[childTableName], childColumnsName));
        }
        //获取关系列
        private static DataColumn[] GetRelationColumns(DataTable dt, string[] relationColumnsName)
        {
            DataColumn[] relationColumns = new DataColumn[relationColumnsName.Length];
            int relationColumnIndex = 0;
            foreach (string columnName in relationColumnsName)
            {
                relationColumns[relationColumnIndex] = dt.Columns[columnName];
                relationColumnIndex++;
            }
            return relationColumns;
        }
        #endregion

        public static void ConvertObj2Xml(XmlDocument xmlDoc, XmlNode parentNode, Type elementType)
        {
            Dictionary<string, string> attrDic = GenderTableAttrDic(elementType);
            XmlElement tableElement = CreateElement(xmlDoc, TABLE_TAG, attrDic);
            InitColumnXmlNodeAttr(xmlDoc, tableElement, elementType);
            parentNode.AppendChild(tableElement);
        }
        public static void Image2Xml(XmlDocument xmlDoc, XmlNode parentNode)
        {
            XmlElement columnElement;
            Dictionary<string, string> attrDic = GenderTableAttrDic(Image_Table_Name);
            XmlElement tableElement = CreateElement(xmlDoc, TABLE_TAG, attrDic);
            for (int i = 1; i <= Image_Column_Count; i++)
            {
                attrDic = GenderColumnAttrDic(Image_Column_Prex + i, Image_Column_DataType);
                columnElement = CreateElement(xmlDoc, COLUMN_TAG, attrDic);
                tableElement.AppendChild(columnElement);
            }
            parentNode.AppendChild(tableElement);
        }
        private static Dictionary<string, string> GenderTableAttrDic(Type elementType)
        {
            return GenderTableAttrDic(elementType.Name);
        }
        private static Dictionary<string, string> GenderTableAttrDic(string elementName)
        {
            Dictionary<string, string> result = new Dictionary<string, string>(5);
            result.Add(NAME_ATTR, elementName);
            result.Add(REF_NAME_ATTR, "Data." + elementName);
            result.Add(DATA_TYPE_ATTR, DEFAULT_DATA_TYPE_ATTR_VALUE);
            result.Add(ENABLE_ATTR, DEFAULT_ENABLE_ATTR);
            return result;
        } 
        
        private static Dictionary<string, string> GenderColumnAttrDic(PropertyInfo p)
        {
            return GenderColumnAttrDic(p.Name, p.PropertyType.Name);
        }
        private static Dictionary<string, string> GenderColumnAttrDic(string name, string typeName)
        {
            Dictionary<string, string> result = new Dictionary<string, string>(5);
            result.Add(NAME_ATTR, name);
            result.Add(DATA_TYPE_ATTR, typeName);
            result.Add(PROP_NAME_ATTR, PROP_NAME_ATTR_VALUE);
            return result;
        }
        
        public static XmlElement CreateElement(XmlDocument xmlDoc, string name, Dictionary<string, string> attrDic)
        {
            XmlElement element = xmlDoc.CreateElement(name);
            foreach (KeyValuePair<string, string> kv in attrDic)
            {
                element.SetAttribute(kv.Key.ToString(), kv.Value.ToString());
            }
            return element;
        }
        private static void InitColumnXmlNodeAttr(XmlDocument xmlDoc, XmlElement tableElement, Type elementType)
        {
            Convert2XmlAttribute cxa;
            XmlElement columnElement;
            Dictionary<string, string> attrDic;
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
                    attrDic = GenderColumnAttrDic(pro);
                    columnElement = CreateElement(xmlDoc, COLUMN_TAG, attrDic);
                    tableElement.AppendChild(columnElement);
                }
            }
        }
    }
}
