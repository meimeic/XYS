using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

using XYS.Common;
using XYS.Report.Lis.Core;
using XYS.Report.Lis.Model;
using XYS.Report.Lis.Persistence;
namespace XYS.Report.Lis.Filler
{
    public class ReportFillByDB : ReportFillSkeleton
    {
        #region 变量
        private LisReportCommonDAL m_reportDAL;
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
            this.m_reportDAL = new LisReportCommonDAL();
        }
        #endregion

        #region 实例属性
        public virtual LisReportCommonDAL ReportDAL
        {
            get
            {
                if (this.m_reportDAL == null)
                {
                    this.m_reportDAL = new LisReportCommonDAL();
                }
                return this.m_reportDAL;
            }
            set { this.m_reportDAL = value; }
        }
        #endregion

        #region 实现父类抽象方法
        protected override void FillElement(ILisReportElement reportElement, LisReportPK RK)
        {
            string sql = GenderSql(reportElement, RK);
            if (sql != null)
            {
                this.D_FillElement(reportElement, sql);
            }
        }
        protected override void FillElements(List<ILisReportElement> reportElementList, LisReportPK RK, Type type)
        {
            string sql = GenderSql(type, RK);
            this.D_FillElements(reportElementList, type, sql);
        }
        #endregion

        #region DAL层代码访问
        protected virtual void D_FillElement(ILisReportElement reportElement, string sql)
        {
            this.ReportDAL.Fill(reportElement, sql);
        }
        protected virtual void D_FillElements(List<ILisReportElement> reportElementList, Type type, string sql)
        {
            this.ReportDAL.FillList(reportElementList, type, sql);
        }
        #endregion

        #region 生成sql语句
        protected string GenderSql(ILisReportElement element, LisReportPK RK)
        {
            return GenderSql(element.GetType(), RK);
        }
        protected string GenderSql(Type type, LisReportPK RK)
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
        protected string GenderWhere(LisReportPK RK)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" where");
            sb.Append(" receivedate='");
            sb.Append(RK.ReceiveDate.ToString("yyyy-MM-dd"));
            sb.Append("' and sectionno=");
            sb.Append(RK.SectionNo);
            sb.Append(" and testtypeno=");
            sb.Append(RK.TestTypeNo);
            sb.Append(" and sampleno='");
            sb.Append(RK.SampleNo);
            sb.Append("'");
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
        #endregion

        #region 辅助方法
        #endregion
    }
}
