using System;
using System.Data;
using System.Reflection;
using System.Collections.Generic;

using XYS.Report.Entities;
namespace XYS.Report.Persistent
{
    public abstract class ReportDB
    {
        #region 受保护构造函数
        protected ReportDB()
        {
        }
        #endregion

        #region 公共方法
        public bool Fill(IDBEntity element, string sql)
        {
            DataTable dt = GetDataTable(sql);
            if (dt == null)
            {
                return false;
            }
            if (dt.Rows.Count > 0)
            {
                if (!FillData(element, dt.Rows[0], dt.Columns))
                {
                    return false;
                }
            }
            return true;
        }
        public bool FillList(List<IDBEntity> elementList, Type type, string sql)
        {
            DataTable dt = GetDataTable(sql);
            if (dt == null)
            {
                return false;
            }
            if (dt.Rows.Count > 0)
            {
                IDBEntity element = null;
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        element = (IDBEntity)type.Assembly.CreateInstance(type.FullName);
                        if (FillData(element, type, dr, dt.Columns))
                        {
                            elementList.Add(element);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion

        #region 抽象方法
        protected abstract DataTable GetDataTable(string sql);
        #endregion

        #region 受保护的方法
        //填充对象属性
        protected bool FillData(IDBEntity element, DataRow dr, DataColumnCollection columns)
        {
            Type type = element.GetType();
            return FillData(element, type, dr, columns);
        }
        protected bool FillData(IDBEntity element, Type type, DataRow dr, DataColumnCollection columns)
        {
            PropertyInfo prop = null;
            foreach (DataColumn dc in columns)
            {
                try
                {
                    prop = type.GetProperty(dc.ColumnName);
                    if (IsColumn(prop))
                    {
                        FillProperty(element, prop, dr[dc]);
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }
        protected void FillProperty(IDBEntity element, PropertyInfo p, object v)
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
            }
            catch (Exception ex)
            {
                throw ex;
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
        #endregion
    }
}
