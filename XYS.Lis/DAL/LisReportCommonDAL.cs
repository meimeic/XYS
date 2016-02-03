using System;
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
        public void Fill(IReportElement element, Hashtable equalTable)
        {

            DataTable dt = Query(element, equalTable);
            if (dt != null)
            {
                FillData(element, dt.Rows[0]);
                AfterFill(element);
            }
        }
        public void FillList(List<IReportElement> elementList, Type elementType, Hashtable equalTable)
        {
            DataTable dt = Query(elementType, equalTable);
            if (dt != null)
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
        public void FillTable(Hashtable elementTable, Type elementType, Hashtable equalTable)
        {
            DataTable dt = Query(elementType, equalTable);
            if (dt != null)
            {
                IReportElement element;
                foreach (DataRow dr in dt.Rows)
                {
                    element = (IReportElement)elementType.Assembly.CreateInstance(elementType.FullName);
                    FillData(element, dr);
                    AfterFill(element);
                    AddElement(elementTable, element);
                }
            }
        }
        protected void AddElement(Hashtable elementTable, IReportElement element)
        {

            switch (element.ElementTag)
            {
                case ReportElementTag.InfoElement:
                    ReportInfoElement infoElement = element as ReportInfoElement;
                    if (infoElement != null)
                    {
                        elementTable[infoElement.SerialNo] = element;
                    }
                    break;
                case ReportElementTag.ItemElement:
                    ReportItemElement itemElement = element as ReportItemElement;
                    if (itemElement != null)
                    {
                        elementTable[itemElement.ItemNo] = element;
                    }
                    break;
                case ReportElementTag.GraphElement:
                    ReportGraphElement graphElement = element as ReportGraphElement;
                    if (graphElement != null)
                    {
                        elementTable[graphElement.GraphName] = element;
                    }
                    break;
                default:
                    break;
            }
        }
        protected DataTable Query(IReportElement element, Hashtable equalTable)
        {
            string sql = GenderSql(element, equalTable);
            DataTable dt = GetDataTable(sql);
            if (dt!=null&&dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        protected DataTable Query(Type elementType, Hashtable equalTable)
        {
            string sql = GenderSql(elementType, equalTable);
            DataTable dt = GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        protected string GetSQLWhere(Hashtable equalTable)
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
            AbstractReportElement e = element as AbstractReportElement;
            if (e != null)
            {
                e.After();
            }
        }
        protected string GenderSql(IReportElement element, Hashtable equalTable)
        {
            AbstractReportElement e = element as AbstractReportElement;
            if (e == null)
            {
                return null;
            }
            return e.SearchSQL + GetSQLWhere(equalTable);
        }
        protected string GenderSql(Type elementType, Hashtable equalTable)
        {
            AbstractReportElement temp = (AbstractReportElement)elementType.Assembly.CreateInstance(elementType.FullName);
            return temp.SearchSQL + GetSQLWhere(equalTable);
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
