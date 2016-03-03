using System;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;
using XYS.Report.Core;
using XYS.Report.Model;
namespace XYS.Report.DAL
{
    public class ReportCommonDAL : IReportDAL
    {
        public ReportCommonDAL()
        { }
        public void Fill(ILisReportElement element, string sql)
        {
            DataTable dt = GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                FillData(element, element.GetType(), dt.Rows[0], dt.Columns);
                AfterFill(element);
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
                        FillData(element, dr,dt.Columns);
                        AfterFill(element);
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
        //获取对象属性
        protected PropertyInfo GetProperty(PropertyInfo[] props, string propName)
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
        protected void AfterFill(ILisReportElement element)
        {
            AbstractReportElement e = element as AbstractReportElement;
            if (e != null)
            {
                e.AfterFill();
            }
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

        //public void Fill(IReportElement element, Hashtable equalTable)
        //{
        //    string sql = GenderSql(element, equalTable);
        //    if (sql != null && !sql.Equals(""))
        //    {
        //        Fill(element, sql);
        //    }
        //}
        //public void FillList(List<IReportElement> elementList, Type elementType, Hashtable equalTable)
        //{
        //    string sql = GenderSql(elementType, equalTable);
        //    FillList(elementList, elementType, sql);
        //}
        //public void FillTable(Hashtable elementTable, Type elementType, Hashtable equalTable)
        //{
        //    DataTable dt = Query(elementType, equalTable);
        //    if (dt != null)
        //    {
        //        IReportElement element;
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            element = (IReportElement)elementType.Assembly.CreateInstance(elementType.FullName);
        //            FillData(element, dr);
        //            AfterFill(element);
        //            AddElement(elementTable, element);
        //        }
        //    }
        //}
        //protected void AddElement(Hashtable elementTable, IReportElement element)
        //{

        //    switch (element.ElementTag)
        //    {
        //        case ReportElementTag.InfoElement:
        //            ReportInfoElement infoElement = element as ReportInfoElement;
        //            if (infoElement != null)
        //            {
        //                elementTable[infoElement.SerialNo] = element;
        //            }
        //            break;
        //        case ReportElementTag.ItemElement:
        //            ReportItemElement itemElement = element as ReportItemElement;
        //            if (itemElement != null)
        //            {
        //                elementTable[itemElement.ItemNo] = element;
        //            }
        //            break;
        //        case ReportElementTag.GraphElement:
        //            ReportGraphElement graphElement = element as ReportGraphElement;
        //            if (graphElement != null)
        //            {
        //                elementTable[graphElement.GraphName] = element;
        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //}
        //protected DataTable Query(IReportElement element, Hashtable equalTable)
        //{
        //    string sql = GenderSql(element, equalTable);
        //    DataTable dt = GetDataTable(sql);
        //    if (dt!=null&&dt.Rows.Count > 0)
        //    {
        //        return dt;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //protected DataTable Query(Type elementType, Hashtable equalTable)
        //{
        //    string sql = GenderSql(elementType, equalTable);
        //    DataTable dt = GetDataTable(sql);
        //    if (dt.Rows.Count > 0)
        //    {
        //        return dt;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }
}
