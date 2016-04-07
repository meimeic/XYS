using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using XYS.Common;
using XYS.Report.Lis.Util;
using XYS.Report.Lis.Model;
using XYS.Report.Lis.Persistence;
namespace XYS.Report.Lis.Handler
{
    public class ReportFillByDBHandler : ReportHandlerSkeleton
    {
        #region 只读字段
        private static readonly string m_defaultHandlerName = "ReportFillHandler";
        #endregion

        #region 变量
        private LisReportCommonDAL m_reportDAL;
        private readonly Hashtable m_section2FillTypeMap;
        #endregion

        #region 构造函数
        public ReportFillByDBHandler()
            : this(m_defaultHandlerName)
        {
        }
        public ReportFillByDBHandler(string handlerName)
            : base(handlerName)
        {
            this.m_section2FillTypeMap = new Hashtable(20);
            this.InitFillElementTable();
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
        protected override HandlerResult OperateReport(ReportReportElement report)
        {
            if (report.PK.Configured)
            {
                LisReportPK PK = report.PK as LisReportPK;
                //填充报告
                FillReportElement(report, PK);
                //填充子项
                List<Type> availableElementList = this.GetAvailableInsideElements(PK);
                if (availableElementList != null && availableElementList.Count > 0)
                {
                    List<AbstractSubFillElement> tempList = null;
                    foreach (Type type in availableElementList)
                    {
                        tempList = report.GetReportItem(type);
                        FillSubElements(tempList, PK, type);
                    }
                }
                return new HandlerResult();
            }
            return new HandlerResult(0, "this report search key is not config!");
        }
        #endregion

        #region
        private void FillReportElement(AbstractSubFillElement report, LisReportPK PK)
        {
            string sql = GenderSql(report, PK);
            this.ReportDAL.Fill(report, sql);
        }
        private void FillSubElements(List<AbstractSubFillElement> subElementList, LisReportPK PK, Type type)
        {
            string sql = GenderSql(type, PK);
            this.ReportDAL.FillList(subElementList, type, sql);
        }
        #endregion

        #region 生成sql语句
        protected string GenderSql(AbstractSubFillElement element, LisReportPK PK)
        {
            return GenderSql(element.GetType(), PK);
        }
        protected string GenderSql(Type type, LisReportPK PK)
        {
            return GenderPreSQL(type) + GenderWhere(PK);
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
        protected string GenderWhere(LisReportPK PK)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" where");
            sb.Append(" receivedate='");
            sb.Append(PK.ReceiveDate.ToString("yyyy-MM-dd"));
            sb.Append("' and sectionno=");
            sb.Append(PK.SectionNo);
            sb.Append(" and testtypeno=");
            sb.Append(PK.TestTypeNo);
            sb.Append(" and sampleno='");
            sb.Append(PK.SampleNo);
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
        protected virtual List<Type> GetAvailableInsideElements(LisReportPK RK)
        {
            if (this.m_section2FillTypeMap.Count == 0)
            {
                InitFillElementTable();
            }
            return this.m_section2FillTypeMap[RK.SectionNo] as List<Type>;
        }
        private void InitFillElementTable()
        {
            lock (this.m_section2FillTypeMap)
            {
                ConfigManager.InitSection2FillElementTable(this.m_section2FillTypeMap);
            }
        }
        #endregion
    }
}
