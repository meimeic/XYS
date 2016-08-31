using System;
using System.Data;
using System.Reflection;
using System.Collections.Generic;

using log4net;

using XYS.Report;
using XYS.Common;
namespace XYS.Persistent
{
    public abstract class ReportDAL
    {
        #region
        protected static readonly ILog LOG = LogManager.GetLogger("ReportPersistent");
        #endregion

        #region 受保护构造函数
        protected ReportDAL()
        {
        }
        #endregion

        #region 公共方法
        public void Fill(IFillElement element, string sql)
        {
            DataTable dt = GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                FillData(element, dt.Rows[0], dt.Columns);
            }
        }
        public void FillList(List<IFillElement> elementList, Type type, string sql)
        {
            DataTable dt = GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                IFillElement element;
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        element = (IFillElement)type.Assembly.CreateInstance(type.FullName);
                        FillData(element, type, dr, dt.Columns);
                        elementList.Add(element);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }
        }
        #endregion

        #region 抽象方法
        protected abstract DataTable GetDataTable(string sql);
        #endregion

        #region 受保护的方法
        //填充对象属性
        protected void FillData(IFillElement element, DataRow dr, DataColumnCollection columns)
        {
            Type type = element.GetType();
            FillData(element, type, dr, columns);
        }
        protected void FillData(IFillElement element, Type type, DataRow dr, DataColumnCollection columns)
        {
            PropertyInfo prop = null;
            foreach (DataColumn dc in columns)
            {
                prop = type.GetProperty(dc.ColumnName);
                if (IsColumn(prop))
                {
                    FillProperty(element, prop, dr[dc]);
                }
            }
        }
        protected bool FillProperty(IFillElement element, PropertyInfo p, object v)
        {
            try
            {
                if (v != DBNull.Value)
                {
                    object value = Convert.ChangeType(v, p.PropertyType);
                    p.SetValue(element, value, null);
                }
                else
                {
                    p.SetValue(element, DefaultForType(p.PropertyType), null);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        protected bool FillProperty(IFillElement element, PropertyInfo p, DataRow dr)
        {
            try
            {
                if (dr[p.Name] != null)
                {
                    if (dr[p.Name] != DBNull.Value)
                    {
                        object value = Convert.ChangeType(dr[p.Name], p.PropertyType);
                        p.SetValue(element, value, null);
                    }
                    else
                    {
                        p.SetValue(element, DefaultForType(p.PropertyType), null);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        protected object DefaultForType(Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }
        #endregion

        #region 私有方法
        //查看属性是否为数据库列
        private bool IsColumn(PropertyInfo prop)
        {
            if (prop != null)
            {
                object[] attrs = prop.GetCustomAttributes(typeof(ColumnAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    return true;
                }
            }
            return false;
        }
        private bool StrEqual(string str1, string str2)
        {
            return str1.ToLower().Equals(str2.ToLower());
        }
        private PropertyInfo GetProperty(PropertyInfo[] props, string propName)
        {
            PropertyInfo p = null;
            foreach (PropertyInfo prop in props)
            {
                if (StrEqual(prop.Name, propName))
                {
                    p = prop;
                    break;
                }
            }
            return p;
        }
        #endregion
    }
}