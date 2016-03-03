using System;
using System.Text;
using System.Reflection;

using XYS.Util;
using XYS.Common;
using XYS.Lis.Model;
namespace XYS.Lis.Core
{
    public class ElementType
    {
        #region 私有字段
        private string m_name;
        private readonly Type m_type;
        private Type m_exportType;
        #endregion

        #region 静态字段
        public static readonly ElementType DEFAULTEXAM = new ElementType(typeof(ReportInfoElement));
        public static readonly ElementType DEFAULTPATIENT = new ElementType(typeof(ReportInfoElement));
        public static readonly ElementType DEFAULTITEM = new ElementType(typeof(ReportItemElement));
        public static readonly ElementType DEFAULTGRAPH = new ElementType(typeof(ReportGraphElement));
        public static readonly ElementType DEFAULTCUSTOM = new ElementType(typeof(ReportCustomElement));
        public static readonly ElementType DEFAULTREPORT = new ElementType(typeof(ReportReportElement));
        public static readonly ElementType DEFAULTINFO = new ElementType(typeof(ReportInfoElement));
        #endregion

        #region 构造函数
        public ElementType(string typeName)
            : this(null, typeName, null)
        {
        }
        public ElementType(string name, string typeName, string exportTypeName)
        {
            this.m_name = name;
            this.m_type = SystemInfo.GetTypeFromString(typeName, true, true);
            if (!string.IsNullOrEmpty(exportTypeName))
            {
                this.m_exportType = SystemInfo.GetTypeFromString(exportTypeName, true, true);
            }
        }
        public ElementType(Type type)
            : this(null, type, null)
        {
        }
        public ElementType(string name, Type type, Type exportType)
        {
            this.m_type = type;
            this.m_name = name;
            this.m_exportType = exportType;
        }
        #endregion

        #region
        public string Name
        {
            get
            {
                if (!string.IsNullOrEmpty(this.m_name))
                {
                    return this.m_name;
                }
                else
                {
                    return this.m_type.Name;
                }
            }
        }
        public Type EType
        {
            get { return this.m_type; }
        }
        public Type ExportType
        {
            get { return this.m_exportType; }
            set { this.m_exportType = value; }
        }
        #endregion

        //#region
        //public string GenderSQL(Type type)
        //{
        //    if (IsTable(type))
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("select ");
        //        PropertyInfo[] props = type.GetProperties();
        //        foreach (PropertyInfo prop in props)
        //        {
        //            if (IsColumn(prop))
        //            {
        //                sb.Append(prop.Name);
        //                sb.Append(',');
        //            }
        //        }
        //        sb.Remove(sb.Length - 1, 1);
        //        sb.Append(" from ");
        //        sb.Append(type.Name);
        //        return sb.ToString();
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //private bool IsTable(Type type)
        //{
        //    object[] attrs = type.GetCustomAttributes(typeof(TableAttribute), true);
        //    if (attrs != null && attrs.Length > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //private bool IsColumn(PropertyInfo prop)
        //{
        //    object[] attrs = prop.GetCustomAttributes(typeof(ColumnAttribute), true);
        //    if (attrs != null && attrs.Length > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //#endregion
    }
}