using System;
using System.Collections;
using System.Data;
using System.Reflection;

using FastReport;

using XYS.Lis.Core;
using XYS.Lis.Export;
using XYS.Lis.Model;
using XYS.Lis.Util;
using XYS.Model;

namespace XYS.Lis.Export.PDF
{
    public class Export2PDF:ReportExportSkeleton
    {
        private static readonly ExportTag DEFAULT_EXPORT = ExportTag.PDF;
        private readonly DataSet m_ds;
        private readonly Hashtable m_no2ModelPath;
        #region
        static Export2PDF()
        {
            LisMap.InitDataSetStruct();
        }
        public Export2PDF()
            : this("Export2PDF")
        {

        }
        public Export2PDF(string name)
            : base(name)
        {
            this.ExportTag = DEFAULT_EXPORT;
            this.m_ds = new DataSet();
            LisMap.SetDataSet(this.m_ds);
            this.m_no2ModelPath = new Hashtable(20);
        }
        #endregion

        #region
        protected override string InnerElementExport(ILisReportElement reportElement)
        {
            throw new NotImplementedException();
        }
        protected override string InnerElementsExport(Hashtable table,ReportElementTag elementTag)
        {
            throw new NotImplementedException();
        }

        protected override string InnerReportExport(ReportReportElement rre)
        {

        }
        #endregion

        #region

        private void FillExam()
        {

        }
        private void FillPatient()
        { 
        }
        private void FillItem()
        { 
        }
        private void FillGraph()
        {
        }
        private void FillCustom()
        {

        }
        private void FillElement(ILisReportElement element, DataSet ds)
        {
            Type elementType = element.GetType();
            DataTable dt = ds.Tables[elementType.Name];
            DataRow dr = dt.NewRow();

        }
        #endregion

        #region
        private string GetModelPath(int modelNo)
        {
            if (this.m_no2ModelPath.Count == 0)
            {
                lock (this.m_no2ModelPath)
                {
                    LisMap.InitModelNo2ModelPathTable(this.m_no2ModelPath);
                }
            }
            string path = this.m_no2ModelPath[modelNo] as string;
            return path;
        }
        #endregion

        #region
        protected void FillDataRow(PropertyInfo p, DataRow dr)
        {
            dr[p.Name] = p.GetValue(null, null);
        }
        protected object DefaultForType(Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }
        #endregion
    }
}
