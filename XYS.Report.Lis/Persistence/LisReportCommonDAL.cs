using System;
using System.Data;
using System.Reflection;
using System.Collections.Generic;

using XYS.Common;
using XYS.Report.Lis.Core;
namespace XYS.Report.Lis.Persistence
{
    public class LisReportCommonDAL
    {
        public LisReportCommonDAL()
        { }
        public void Fill(ILisReportElement element, string sql)
        {
            DataTable dt = GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                FillData(element, element.GetType(), dt.Rows[0], dt.Columns);
                //AfterFill(element);
            }
        }
        public void FillList(List<ILisReportElement> elementList, Type elementType, string sql)
        {
            DataTable dt = GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                ILisReportElement element;
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        element = (ILisReportElement)elementType.Assembly.CreateInstance(elementType.FullName);
                        FillData(element, dr, dt.Columns);
                        //AfterFill(element);
                        elementList.Add(element);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }
        }

        //填充对象属性
        protected void FillData(ILisReportElement element, DataRow dr, DataColumnCollection columns)
        {
            PropertyInfo prop = null;
            PropertyInfo[] props = element.GetType().GetProperties();
            foreach (DataColumn dc in columns)
            {
                prop = GetProperty(props, dc.ColumnName);
                if (IsColumn(prop))
                {
                    FillProperty(element, prop, dr[dc]);
                }
            }
        }
        protected void FillData(ILisReportElement element, Type type, DataRow dr, DataColumnCollection columns)
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

        protected bool FillProperty(ILisReportElement element, PropertyInfo p, DataRow dr)
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
        protected bool FillProperty(ILisReportElement element, PropertyInfo p, object v)
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
        
        protected object DefaultForType(Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }
        
        protected DataTable GetDataTable(string sql)
        {
            DataTable dt = null;
            if (!string.IsNullOrEmpty(sql))
            {
                dt = DbHelperSQL.Query(sql).Tables["dt"];
            }
            return dt;
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
    }
}
