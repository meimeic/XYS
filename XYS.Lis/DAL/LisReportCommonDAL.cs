﻿using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections.Generic;

using XYS.Common;
using XYS.Lis.Core;
using XYS.Lis.Model;
namespace XYS.Lis.DAL
{
    public class LisReportCommonDAL : ILisReportDAL
    {
        public LisReportCommonDAL()
        { }
        public void Fill(IReportElement element, string sql)
        {
            DataTable dt = GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                FillData(element, dt.Rows[0]);
                AfterFill(element);
            }
        }
        //public void Fill(IReportElement element, Hashtable equalTable)
        //{
        //    string sql = GenderSql(element, equalTable);
        //    if (sql != null && !sql.Equals(""))
        //    {
        //        Fill(element, sql);
        //    }
        //}

        public void FillList(List<IReportElement> elementList, Type elementType, string sql)
        {
            DataTable dt = GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                IReportElement element;
                foreach (DataRow dr in dt.Rows)
                {
                    element = (IReportElement)elementType.Assembly.CreateInstance(elementType.FullName);
                    FillData(element, dr);
                    AfterFill(element);
                    elementList.Add(element);
                }
            }
        }
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
        
        //填充对象属性
        protected void FillData(IReportElement element, DataRow dr)
        {
            PropertyInfo[] props = GetProperties(element);
            if (props == null || props.Length == 0)
            {
                //不存在属性 弹出异常
            }
            else
            {
                TableColumnAttribute tca;
                foreach (PropertyInfo p in props)
                {
                    tca = (TableColumnAttribute)Attribute.GetCustomAttribute(p, typeof(TableColumnAttribute));
                    if (tca != null && tca.IsColumn)
                    {
                        //如果是数据库列属性
                        FillProperty(element, p, dr);
                    }
                }
            }
        }
        protected void AfterFill(IReportElement element)
        {
            element.After();
        }

        protected DataTable GetDataTable(string sql)
        {
            DataTable dt = null;
            if (sql != null)
            {
                dt = DbHelperSQL.Query(sql).Tables["dt"];
            }
            return dt;
        }
        //获取对象属性
        protected PropertyInfo[] GetProperties(IReportElement element)
        {
            PropertyInfo[] props = null;
            try
            {
                Type type = element.GetType();
                props = type.GetProperties();
            }
            catch (Exception ex)
            {
            }
            return props;
        }
        
        protected bool FillProperty(IReportElement element, PropertyInfo p, DataRow dr)
        {
            try
            {
                if (dr[p.Name.ToLower()] == null)
                {
                    throw new Exception("LisReportCommonDAL:does not exsit the["+p.Name.ToLower()+"] column in the row");
                }
                if (dr[p.Name.ToLower()] != DBNull.Value)
                {
                    object value = Convert.ChangeType(dr[p.Name.ToLower()], p.PropertyType);
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
    }
}
