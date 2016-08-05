using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using XYS.Common;
using XYS.Persistent;
namespace XYS.Report
{
    public abstract class HandleSkeleton : IHandle
    {
        private readonly ReportDAL DAL;
        protected HandleSkeleton(ReportDAL dal)
        {
            this.DAL = dal;
        }

        #region 实现接口方法
        public bool HandleElement(IFillElement element, IReportKey RK)
        {
            bool result = false;
            if (element != null)
            {
                result = FillElement(element, RK);
                if (result)
                {
                    result = InnerHandle(element, RK);
                }
            }
            return result;
        }
        public bool HandleElement(List<IFillElement> elements, IReportKey RK, Type type)
        {
            bool result = false;
            if (elements != null)
            {
                elements.Clear();
                result = FillElement(elements, RK, type);
                if (result)
                {
                    result = InnerHandle(elements, RK);
                }
            }
            return result;
        }
        #endregion

        #region 受保护的方法 内部处理
        protected abstract bool InnerHandle(IFillElement element, IReportKey RK);
        protected abstract bool InnerHandle(List<IFillElement> elements, IReportKey RK);
        #endregion

        #region 填充数据
        private bool FillElement(IFillElement element, IReportKey RK)
        {
            string sql = GenderSql(element, RK);
            //LOG.Info("生成ReportInfo的SQL语句:" + sql);
            try
            {
               //LOG.Info("ReportInfo数据填充");
                this.DAL.Fill(element, sql);
                return true;
            }
            catch (Exception ex)
            {
                //LOG.Error("ReportInfo填充异常");
                return false;
            }
        }
        private bool FillElement(List<IFillElement> elements, IReportKey RK, Type type)
        {
            string sql = GenderSql(type, RK);
            //LOG.Info("生成" + type.Name + "的SQL语句:" + sql);
            try
            {
                //LOG.Info(type.Name + "集合填充");
                this.DAL.FillList(elements, type, sql);
                return true;
            }
            catch (Exception ex)
            {
                //LOG.Error(type.Name + "集合填充异常");
                return false;
            }
        }
        #endregion

        #region 生成sql语句
        protected string GenderSql(IFillElement element, IReportKey RK)
        {
            return GenderSql(element.GetType(), RK);
        }
        protected string GenderSql(Type type, IReportKey RK)
        {
            return GenderPreSQL(type) + RK.KeyWhere();
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
        #endregion

        #region
        protected bool IsExist(List<IFillElement> elementList)
        {
            if (elementList != null && elementList.Count > 0)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
