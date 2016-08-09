using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Reflection;
using System.Collections.Generic;

using XYS.Util;
using XYS.Report;
using XYS.Common;

using XYS.FR.Service.Model;
namespace XYS.FR.Service.Util
{
    public class DataStruct
    {
        #region 字段常量
        private static readonly Type declaringType = typeof(DataStruct);
        private static readonly string FileName = "reporttables.frd";

        private static readonly string ROOT_TAG = "Dictionary";
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
        #endregion

        #region 静态常量
        private static readonly DataSet FRSet;
        private static readonly List<Type> TypeList;
        #endregion

        #region 静态构造函数
        static DataStruct()
        {
            FRSet = new DataSet();
            TypeList = new List<Type>(10);

            Init();
        }
        #endregion

        #region 公共静态属性
        public static List<Type> TList
        {
            get { return TypeList; }
        }
        #endregion

        #region 公共静态方法
        public static void InitXMLStruct()
        {
            string fileFullName = Path.Combine(SystemInfo.ApplicationBaseDirectory, "Print", "DataSet", FileName);
            InitXmlStruct(fileFullName);
        }
        public static DataSet GetSet()
        {
            return FRSet.Clone();
        }
        #endregion

        #region 初始化模板结构
        protected static void InitXmlStruct(string fileFullName)
        {
            ConsoleInfo.Debug(declaringType, "Start--make the data struct xml file " + fileFullName + " by exportelemnts");
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(dec);
            XmlNode root = doc.CreateNode(XmlNodeType.Element, ROOT_TAG, null);
            foreach (Type type in TypeList)
            {
                if (IsExport(type))
                {
                    ConvertObj2Xml(doc, root, type);
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
                ConsoleInfo.Debug(declaringType, "End--make the data struct xml file " + fileFullName + " by exportelemnts");
            }
            catch (Exception ex)
            {
                ConsoleInfo.Error(declaringType, "DataStruct:" + ex.Message);
                throw ex;
            }
        }
        protected static void InitRamStruct()
        {
            DataTable dt = null;
            ConsoleInfo.Debug(declaringType, "Start--make the RAM dataset " + " by exportelemnts");
            foreach (Type type in TypeList)
            {
                if (IsExport(type))
                {
                    dt = new DataTable();
                    ConvertObj2Table(dt, type);
                    FRSet.Tables.Add(dt);
                }
            }
            ConsoleInfo.Debug(declaringType, "end--make the RAM dataset " + " by exportelemnts");
        }
        #endregion

        #region xml相关操作
        public static void ConvertObj2Xml(XmlDocument xmlDoc, XmlNode parentNode, Type elementType)
        {
            ConsoleInfo.Debug(declaringType, "making xml table struct by " + elementType.Name);
            Dictionary<string, string> attrDic = GenderTableAttrDic(elementType);
            XmlElement tableElement = CreateElement(xmlDoc, TABLE_TAG, attrDic);
            InitColumnXmlNodeAttr(xmlDoc, tableElement, elementType);
            ConsoleInfo.Debug(declaringType, "maked xml table struct by " + elementType.Name);
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
            return GenderColumnAttrDic(p.Name, p.PropertyType.FullName);
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
            XmlElement columnElement = null;
            Dictionary<string, string> attrDic = null;
            PropertyInfo[] props = elementType.GetProperties();
            if (props == null || props.Length == 0)
            {
                return;
            }
            foreach (PropertyInfo pro in props)
            {
                if (IsExport(pro))
                {
                    attrDic = GenderColumnAttrDic(pro);
                    columnElement = CreateElement(xmlDoc, COLUMN_TAG, attrDic);
                    tableElement.AppendChild(columnElement);
                }
            }
        }
        public static string GetDataStructFilePath()
        {
            try
            {
                string applicationBaseDirectory = SystemInfo.ApplicationBaseDirectory;
                string filePath = Path.Combine(applicationBaseDirectory, "DataStruct");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                return filePath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region dataset相关操作
        private static void ConvertObj2Table(DataTable dt, Type elementType)
        {
            ConsoleInfo.Debug(declaringType, "making RAM table by " + elementType.Name);
            dt.TableName = elementType.Name;
            PropertyInfo[] props = elementType.GetProperties();
            if (props == null || props.Length == 0)
            {
                return;
            }
            foreach (PropertyInfo pro in props)
            {
                if (IsExport(pro))
                {
                    dt.Columns.Add(pro.Name, pro.PropertyType);
                }
            }
            ConsoleInfo.Debug(declaringType, "maked RAM table by " + elementType.Name);
        }
        #endregion

        #region 辅助方法
        public static bool IsExport(Type type)
        {
            if (type != null)
            {
                return typeof(IExportElement).IsAssignableFrom(type);
            }
            return false;
        }
        public static bool IsExport(PropertyInfo prop)
        {
            if (prop != null)
            {
                object[] xAttrs = prop.GetCustomAttributes(typeof(ExportAttribute), true);
                if (xAttrs != null && xAttrs.Length > 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 私有静态方法
        private static void Init()
        {
            TypeList.Add(typeof(Custom));
            TypeList.Add(typeof(Data1));
            TypeList.Add(typeof(Data2));
            TypeList.Add(typeof(Data3));
            TypeList.Add(typeof(Image));
            TypeList.Add(typeof(KVData));

            InitRamStruct();
        }

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
    }
}
