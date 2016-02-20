using System;
using System.Text;
using System.Reflection;

using XYS.Common;
using XYS.Lis.Model;
using XYS.Lis.Util;
namespace XYS.Lis.Core
{
    public class ElementType
    {
        private static readonly string m_defaultElementName = "defaultElementName";
        #region 私有字段
        private string m_name;
        private Type m_type;
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
            : this(null, typeName)
        {
        }
        public ElementType(string name, string typeName)
        {
            this.m_name = name;
            this.m_type = SystemInfo.GetTypeFromString(typeName, true, true);
        }
        public ElementType(Type type)
            : this(null, type)
        {
        }
        public ElementType(string name, Type type)
        {
            this.m_type = type;
            this.m_name = name;
        }
        #endregion

        #region
        public string Name
        {
            get
            {
                if (!string.IsNullOrEmpty(this.m_name))
                {
                    return this.m_name.ToLower();
                }
                else
                {
                    return this.m_type.Name.ToLower();
                }
            }
        }
        public Type EType
        {
            get { return this.m_type; }
        }
        public string TypeSql
        {
            get { return GenderSQL(this.m_type); }
        }
        #endregion

        #region
        public string GenderSQL(Type type)
        {
            if (IsTable(type))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select ");
                PropertyInfo[] props = type.GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    if (IsColumn(prop))
                    {
                        sb.Append(prop.Name);
                        sb.Append(',');
                    }
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(" from ");
                sb.Append(type.Name);
                return sb.ToString();
            }
            else
            {
                return null;
            }
        }
        private bool IsTable(Type type)
        {
             object[] attrs = type.GetCustomAttributes(typeof(TableAttribute), true);
             if (attrs != null && attrs.Length > 0)
             {
                 return true;
             }
             else
             {
                 return false;
             }
        }
        private bool IsColumn(PropertyInfo prop)
        {
            object[] attrs = prop.GetCustomAttributes(typeof(ColumnAttribute), true);
            if (attrs != null && attrs.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}