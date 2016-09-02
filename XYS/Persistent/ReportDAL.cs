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
        #region 静态字段
        protected static readonly ILog LOG = LogManager.GetLogger("ReportPersistent");
        #endregion

        #region 受保护构造函数
        protected ReportDAL()
        {
        }
        #endregion

        #region 公共方法
        public bool Fill(IFillElement element, string sql)
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
                    LOG.Error("填充" + element.GetType().Name + "元素失败");
                    return false;
                }
            }
            return true;
        }
        public bool FillList(List<IFillElement> elementList, Type type, string sql)
        {
            DataTable dt = GetDataTable(sql);
            if (dt == null)
            {
                return false;
            }
            if (dt.Rows.Count > 0)
            {
                IFillElement element = null;
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        element = (IFillElement)type.Assembly.CreateInstance(type.FullName);
                        if (FillData(element, type, dr, dt.Columns))
                        {
                            elementList.Add(element);
                        }
                        else
                        {
                            LOG.Error("填充" + type.Name + "元素列表失败");
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        LOG.Error("创建" + type.Name + "元素异常", ex);
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
        protected bool FillData(IFillElement element, DataRow dr, DataColumnCollection columns)
        {
            Type type = element.GetType();
            return FillData(element, type, dr, columns);
        }
        protected bool FillData(IFillElement element, Type type, DataRow dr, DataColumnCollection columns)
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
                    LOG.Error("填充" + type.Name + "的" + dc.ColumnName + "属性异常", ex);
                    return false;
                }
            }
            return true;
        }
        protected void FillProperty(IFillElement element, PropertyInfo p, object v)
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