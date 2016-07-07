using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

using log4net;

using XYS.Report;
using XYS.Common;
using XYS.Lis.Report.Model;
using XYS.Lis.Report.Persistent;
namespace XYS.Lis.Report.Handler
{
    public abstract class ReportHandleSkeleton
    {
        #region 静态字段
        protected static ILog LOG = LogManager.GetLogger("LisReportHandle");
        #endregion

        #region 只读字段
        private readonly ReportDAL m_reportDAL;
        #endregion

        #region 构造函数
        protected ReportHandleSkeleton()
        {
            this.m_reportDAL = new ReportDAL();
        }
        #endregion

        #region 实例属性
        protected ReportDAL ReportDAL
        {
            get { return this.m_reportDAL; }
        }
        #endregion

        #region
        public bool InitElement(IFillElement element, ReportPK RK)
        {
            bool result = false;
            if (element != null)
            {
                result = FillElement(element, RK);
                if (result)
                {
                    result = HandleElement(element);
                }
            }
            return result;
        }
        public bool InitElement(List<IFillElement> elements, ReportPK RK, Type type)
        {
            bool result = false;
            if (elements != null)
            {
                elements.Clear();
                result = FillElement(elements, RK, type);
                if (result)
                {
                    result = HandleElement(elements);
                }
            }
            return result;
        }
        #endregion

        #region
        protected abstract bool HandleElement(IFillElement element,ReportPK RK);
        protected abstract bool HandleElement(List<IFillElement> elements, ReportPK RK);
        #endregion

        #region 辅助方法
        protected bool IsExist(List<IFillElement> elementList)
        {
            if (elementList != null && elementList.Count > 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 填充数据
        private bool FillElement(IFillElement element, ReportPK RK)
        {
            string sql = GenderSql(element, RK);
            LOG.Info("生成ReportInfo的SQL语句:" + sql);
            try
            {
                LOG.Info("ReportInfo数据填充");
                this.ReportDAL.Fill(element, sql);
                return true;
            }
            catch (Exception ex)
            {
                LOG.Error("ReportInfo填充异常");
                return false;
            }
        }
        private bool FillElement(List<IFillElement> elements, ReportPK RK, Type type)
        {
            string sql = GenderSql(type, RK);
            LOG.Info("生成" + type.Name + "的SQL语句:" + sql);
            try
            {
                LOG.Info(type.Name + "集合填充");
                this.ReportDAL.FillList(elements, type, sql);
                return true;
            }
            catch (Exception ex)
            {
                LOG.Error(type.Name + "集合填充异常");
                return false;
            }
        }
        #endregion

        #region 生成sql语句
        protected string GenderSql(IFillElement element, ReportPK RK)
        {
            return GenderSql(element.GetType(), RK);
        }
        protected string GenderSql(Type type, ReportPK RK)
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
        protected string GenderWhere(ReportPK RK)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" where ");
            sb.Append("receivedate='");
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
    }
}
