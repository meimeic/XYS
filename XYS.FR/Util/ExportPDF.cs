using System;
using System.Data;
using System.Reflection;
using System.Collections.Generic;

using XYS.Report;
using XYS.Common;
namespace XYS.FR.Util
{
    public class ExportPDF
    {
        #region 构造函数
        public ExportPDF()
        { }
        #endregion

        #region 公共方法
        public void ExportElement(List<IExportElement> elementList, DataSet ds)
        {
            if (elementList != null)
            {
                foreach (IExportElement element in elementList)
                {
                    ExportElement(element, ds);
                }
            }
        }
        public void ExportElement(IExportElement element, DataSet ds)
        {
            Type type = element.GetType();
            DataTable dt = ds.Tables[type.Name];
            if (dt == null)
            {
                throw new Exception("数据集中找不到对应表:" + type.Name);
            }
            DataRow dr = dt.NewRow();
            PropertyInfo[] props = type.GetProperties();
            if (props == null || props.Length == 0)
            {
                return;
            }
            foreach (PropertyInfo pro in props)
            {
                if (IsExport(pro))
                {
                    FillDataColumn(pro, dr, element);
                }
            }
            dt.Rows.Add(dr);
        }
        #endregion

        #region 私有方法
        private void FillDataColumn(PropertyInfo p, DataRow dr, IExportElement element)
        {
            dr[p.Name] = p.GetValue(element, null);
        }
        private bool IsExport(PropertyInfo prop)
        {
            if (prop != null)
            {
                object[] xAttrs = prop.GetCustomAttributes(typeof(ExportAttribute), true);
                if (xAttrs != null && xAttrs.Length > 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}