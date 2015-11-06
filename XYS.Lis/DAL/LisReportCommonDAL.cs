using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections.Generic;

using XYS.Common;
using XYS.Utility.DB;
using XYS.Lis.Core;
using XYS.Model;
using XYS.Lis.Model;

namespace XYS.Lis.DAL
{
    public class LisReportCommonDAL : ILisReportDAL
    {
        public LisReportCommonDAL()
        { }
        public void Fill(ILisReportElement element, Hashtable equalTable)
        {
            DataTable dt = Query(element, equalTable);
            if (dt != null)
            {
                FillData(element, dt.Rows[0]);
                AfterFill(element);
            }
        }
        public void FillList(List<ILisReportElement> elementList, Type elementType, Hashtable equalTable)
        {
            DataTable dt = Query(elementType, equalTable);
            if (dt != null)
            {
                ILisReportElement element;
                foreach (DataRow dr in dt.Rows)
                {
                    element = (ILisReportElement)elementType.Assembly.CreateInstance(elementType.FullName);
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
                ILisReportElement element;
                foreach (DataRow dr in dt.Rows)
                {
                    element = (ILisReportElement)elementType.Assembly.CreateInstance(elementType.FullName);
                    FillData(element, dr);
                    AfterFill(element);
                    AddElement(elementTable, element);
                }
            }
        }
        protected void AddElement(Hashtable elementTable, ILisReportElement element)
        {

            switch (element.ElementTag)
            {
                case ReportElementTag.ExamElement:
                    ReportExamElement examElement = element as ReportExamElement;
                    if (examElement != null)
                    {
                        elementTable[examElement.SerialNo] = element;
                    }
                    break;
                case ReportElementTag.PatientElement:
                    ReportPatientElement patientElement = element as ReportPatientElement;
                    if (patientElement != null)
                    {
                        elementTable[patientElement.PID] = element;
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
        protected DataTable Query(ILisReportElement element, Hashtable equalTable)
        {
            string sql = GenderSql(element, equalTable);
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
        protected void FillData(ILisReportElement element, DataRow dr)
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
        protected void AfterFill(ILisReportElement element)
        {
            element.AfterFill();
        }
        protected string GenderSql(ILisReportElement element, Hashtable equalTable)
        {
            return element.SearchSQL + GetSQLWhere(equalTable);
        }
        protected string GenderSql(Type elementType, Hashtable equalTable)
        {
            ILisReportElement temp = (ILisReportElement)elementType.Assembly.CreateInstance(elementType.FullName);
            return temp.SearchSQL + GetSQLWhere(equalTable);
        }
        protected DataTable GetDataTable(string sql)
        {
            DataTable dt = DbHelperSQL.Query(sql).Tables["dt"];
            return dt;
        }
        protected PropertyInfo[] GetProperties(ILisReportElement element)
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
        protected bool FillProperty(ILisReportElement element, PropertyInfo p, DataRow dr)
        {
            try
            {
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
