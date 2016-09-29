using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

using XYS.Report.Attributes;
using XYS.Report.Entities;
namespace XYS.Report.Persistent
{
    public interface IReportDAO
    {
        bool Fill(IDBReportItem element,string sql);
        bool Fill(List<IDBReportItem> elementList, Type type,string sql);
    }

    public abstract class ReportDAOBase:IReportDAO
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        protected ReportDAOBase()
        {
        }
        

        /// <summary>
        /// 数据项填充
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool Fill(IDBReportItem element,string sql)
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
        public bool Fill(List<IDBReportItem> elementList, Type type,string sql)
        {
            DataTable dt = GetDataTable(sql);
            if (dt == null)
            {
                return false;
            }
            if (dt.Rows.Count > 0)
            {
                IDBReportItem element = null;
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        element = (IDBReportItem)type.Assembly.CreateInstance(type.FullName);
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

        #region 抽象方法
        protected abstract DataTable GetDataTable(string sql);
        #endregion

        #region 受保护的方法
        //填充对象属性
        protected bool FillData(IDBReportItem element, DataRow dr, DataColumnCollection columns)
        {
            Type type = element.GetType();
            return FillData(element, type, dr, columns);
        }
        protected bool FillData(IDBReportItem element, Type type, DataRow dr, DataColumnCollection columns)
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
        protected void FillProperty(IDBReportItem element, PropertyInfo p, object v)
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

        private bool IsColumn(PropertyInfo prop)
        {
            if (prop != null)
            {
                try
                {
                    object[] attrs = prop.GetCustomAttributes(typeof(DBColumnAttribute), true);
                    if (attrs != null && attrs.Length > 0)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }
    }
}
