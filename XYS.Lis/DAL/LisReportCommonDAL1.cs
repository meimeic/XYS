using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using XYS.Common;
using XYS.Lis.Core;
using XYS.Lis.Core;

namespace XYS.Lis.DAL
{
    public class LisReportCommonDAL1<T>
        where T : ILisReportElement, new()
    {
        public static void Search(T t, Hashtable equalTable)
        {
            Query(t, equalTable);
        }
        public static void SearchList(List<T> lt, Hashtable equalTable)
        {
            QueryList(lt, equalTable);
        }
        protected static void Query(T t, Hashtable equalTable)
        {
            string sql = GenderSql(t,equalTable);
            DataTable dt = GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                FillData(t, dt.Rows[0]);
                AfterFill(t);
            }
        }
        protected static void QueryList(List<T> lt, Hashtable equalTable)
        {
            T temp = new T();
            string sql = GenderSql(temp,equalTable);
            temp = default(T);
            DataTable dt = GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                T t;
                foreach (DataRow dr in dt.Rows)
                {
                    t = new T();
                    FillData(t, dr);
                    AfterFill(t);
                    lt.Add(t);
                }
            }
        }
        protected static string GetSQLWhere(Hashtable equalTable)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" where ");
            foreach (DictionaryEntry de in equalTable)
            {
                //int
                if (de.Value.GetType().FullName == "System.Int32")
                {
                    sb.Append(de.Key);
                    sb.Append("=");
                    sb.Append(de.Value);
                }
                //datetime
                else if (de.Value.GetType().FullName == "System.DateTime")
                {
                    DateTime dt = (DateTime)de.Value;
                    sb.Append(de.Key);
                    sb.Append("='");
                    sb.Append(dt.Date.ToString("yyyy-MM-dd"));
                    sb.Append("'");
                }
                //其他类型
                else
                {
                    sb.Append(de.Key);
                    sb.Append("='");
                    sb.Append(de.Value.ToString());
                    sb.Append("'");
                }
                sb.Append(" and ");
            }
            sb.Remove(sb.Length - 5, 5);
            return sb.ToString();
        }
        protected static void FillData(T t, DataRow dr)
        {
            PropertyInfo[] props = GetProperties(t);
            if (props == null || props.Length == 0)
            {
                //不存在属性 弹出异常
            }
            else
            {
                ColumnAttribute tca;
                foreach (PropertyInfo p in props)
                {
                    tca = (ColumnAttribute)Attribute.GetCustomAttribute(p, typeof(ColumnAttribute));
                    if (tca != null && tca.IsColumn)
                    {
                        //如果是数据库列属性
                        FillProperty(t, p, dr);
                    }
                }
            }
        }
        protected static void AfterFill(T t)
        {
            t.AfterFill();
        }
        protected static string GenderSql(T t,Hashtable equalTable)
        {
            return t.SearchSQL+GetSQLWhere(equalTable);
        }
        protected static DataTable GetDataTable(string sql)
        {
            DataTable dt = DbHelperSQL.Query(sql).Tables["dt"];
            return dt;
        }
        protected static PropertyInfo[] GetProperties(T t)
        {
            PropertyInfo[] props = null;
            try
            {
                Type type = typeof(T);
                props = type.GetProperties();
            }
            catch (Exception ex)
            {
            }
            return props;
        }
        protected static bool FillProperty(T t, PropertyInfo p, DataRow dr)
        {
            try
            {
                if (dr[p.Name.ToLower()] != DBNull.Value)
                {
                    object value = Convert.ChangeType(dr[p.Name.ToLower()], p.PropertyType);
                    p.SetValue(t, value, null);
                }
                else
                {
                    p.SetValue(t, DefaultForType(p.PropertyType), null);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        protected static object DefaultForType(Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }
    }
}
