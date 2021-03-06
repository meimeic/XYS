﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using log4net;

using XYS.Common;
using XYS.Persistent;
namespace XYS.Report
{
    public abstract class HandleSkeleton : IHandle
    {
        #region 私有字段
        private readonly ReportDAL DAL;
        protected static readonly ILog LOG = LogManager.GetLogger("ReportHandle");
        #endregion

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
                LOG.Info("ID为" + RK.ID + "的报告进行" + element.GetType().Name + "类型的数据填充");
                result = FillElement(element, RK);
                if (result)
                {
                    LOG.Info(element.GetType().Name + "数据填充成功，进行内部处理");
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
                LOG.Info("ID为" + RK.ID + "的报告进行" + type.Name + "类型的数据集合填充");
                result = FillElement(elements, RK, type);
                if (result)
                {
                    LOG.Info(type.Name + "数据集合填充成功，进行内部处理");
                    result = InnerHandle(elements, RK);
                }
            }
            return result;
        }
        #endregion

        #region 受保护的方法内部处理
        protected abstract bool InnerHandle(IFillElement element, IReportKey RK);
        protected abstract bool InnerHandle(List<IFillElement> elements, IReportKey RK);
        #endregion

        #region 填充数据
        private bool FillElement(IFillElement element, IReportKey RK)
        {
            string sql = GenderSql(element, RK);
            LOG.Info("填充SQL语句:" + sql);
            try
            {
                LOG.Info("数据填充中");
                return this.DAL.Fill(element, sql);
            }
            catch (Exception ex)
            {
                LOG.Error("数据填充异常:" + ex.Message);
                return false;
            }
        }
        private bool FillElement(List<IFillElement> elements, IReportKey RK, Type type)
        {
            string sql = GenderSql(type, RK);
            LOG.Info("填充" + type.Name + "集合SQL语句:" + sql);
            try
            {
                LOG.Info(type.Name + "类型集合填充");
                return this.DAL.FillList(elements, type, sql);
            }
            catch (Exception ex)
            {
                LOG.Error(type.Name + "类型集合填充异常:" + ex.Message);
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
            sb.Append(" ");
            return sb.ToString();
        }
        private bool IsColumn(PropertyInfo prop)
        {
            if (prop != null)
            {
                try
                {
                    object[] attrs = prop.GetCustomAttributes(typeof(ColumnAttribute), true);
                    if (attrs != null && attrs.Length > 0)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    LOG.Warn("获取" + prop.Name + "的特性异常", ex);
                    return false;
                }
            }
            return false;
        }
        #endregion

        #region 受保护方法
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
