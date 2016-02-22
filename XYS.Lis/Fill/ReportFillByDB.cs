using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using XYS.Common;
using XYS.Lis.Model;
using XYS.Lis.Core;
using XYS.Lis.DAL;
namespace XYS.Lis.Fill
{
    public class ReportFillByDB : ReportFillSkeleton
    {
        #region 变量
        private ReportCommonDAL m_reportDAL;
        private static readonly string m_FillerName = "DBFiller";
        #endregion

        #region 构造函数
        public ReportFillByDB()
            : this(m_FillerName)
        {
        }
        public ReportFillByDB(string name)
            : base(name)
        {
            this.m_reportDAL = new ReportCommonDAL();
        }
        #endregion

        #region 实例属性
        public virtual ReportCommonDAL ReportDAL
        {
            get
            {
                if (this.m_reportDAL == null)
                {
                    this.m_reportDAL = new ReportCommonDAL();
                }
                return this.m_reportDAL;
            }
            set { this.m_reportDAL = value; }
        }
        #endregion

        #region 实现父类抽象方法
        protected override void FillElement(IReportElement reportElement, ReportKey RK)
        {
            string sql = GenderSql(reportElement, RK);
            if (sql != null)
            {
                this.D_FillElement(reportElement, sql);
            }
        }
        protected override void FillElements(List<IReportElement> reportElementList, ReportKey RK, Type type)
        {
            string sql = GenderSql(type, RK);
            this.D_FillElements(reportElementList, type, sql);
        }
        #endregion

        #region DAL层代码访问
        protected virtual void D_FillElement(IReportElement reportElement, string sql)
        {
            this.ReportDAL.Fill(reportElement, sql);
        }
        protected virtual void D_FillElements(List<IReportElement> reportElementList, Type type, string sql)
        {
            this.ReportDAL.FillList(reportElementList, type, sql);
        }
        #endregion

        #region 生成sql语句
        protected string GenderSql(IReportElement element, ReportKey RK)
        {
            string where = GenderWhere(RK);
            AbstractReportElement e = element as AbstractReportElement;
            if (e != null)
            {
                //
                if (!string.IsNullOrEmpty(e.SearchSQL))
                {
                    return GenderPreSQL(element.GetType()) + GenderWhere(RK);
                }
                return e.SearchSQL + GenderWhere(RK);
            }
            return null;
        }
        protected string GenderSql(Type type, ReportKey RK)
        {
            return GenderPreSQL(type) + GenderWhere(RK);
        }
        protected string GenderPreSQL(Type type)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select ");
            PropertyInfo[] props = type.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (IsColumn(prop))
                {
                    sb.Append(prop.Name);
                    sb.Append(',');
                }
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(" from ");
            sb.Append(type.Name);
            return sb.ToString();
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
        protected string GenderWhere(Hashtable equalTable)
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
        protected string GenderWhere(ReportKey RK)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" where ");
            foreach (KeyColumn kc in RK.KeySet)
            {
                //int
                if (kc.Value.GetType().FullName == "System.Int32")
                {
                    sb.Append(kc.Name);
                    sb.Append("=");
                    sb.Append(kc.Value);
                }
                //datetime
                else if (kc.Value.GetType().FullName == "System.DateTime")
                {
                    DateTime dt = (DateTime)kc.Value;
                    sb.Append(kc.Name);
                    sb.Append("='");
                    sb.Append(dt.Date.ToString("yyyy-MM-dd"));
                    sb.Append("'");
                }
                //其他类型
                else
                {
                    sb.Append(kc.Name);
                    sb.Append("='");
                    sb.Append(kc.Value.ToString());
                    sb.Append("'");
                }
                sb.Append(" and ");
            }
            sb.Remove(sb.Length - 5, 5);
            return sb.ToString();
        }
        #endregion

        #region 辅助方法
        //protected virtual Hashtable ReportKey2Table(ReportKey key)
        //{
        //    Hashtable keyTable = new Hashtable(5);
        //    foreach (KeyColumn k in key.KeySet)
        //    {
        //        keyTable.Add(k.Name, k.Value);
        //    }
        //    return keyTable;
        //}
        //private bool IsTable(Type type)
        //{
        //    object[] attrs = type.GetCustomAttributes(typeof(TableAttribute), true);
        //    if (attrs != null && attrs.Length > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        #endregion
    }
}
