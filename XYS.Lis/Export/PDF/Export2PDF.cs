using System;
using System.Data;
using System.IO;
using System.Collections.Generic;

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

        #region
        public Export2PDF()
            : this("Export2PDF")
        {
        }
        public Export2PDF(string name)
            : base(name)
        {
            this.ExportTag = DEFAULT_EXPORT;
        }
        #endregion

        #region
        protected override string InnerElementExport(ILisReportElement reportElement)
        {
            throw new NotImplementedException();
        }
        protected override string InnerElementListExport(List<ILisReportElement> elementList)
        {
            throw new NotImplementedException();
        }

        protected override string InnerReportExport(ReportReportElement rre)
        {
            throw new NotImplementedException();
        }
        #endregion

        protected virtual string GenderPDF(ReportReportElement rre)
        {
            return null;
        }

        #region
        private DataSet GetExportDataSet(string frdName)
        {
            string frdFullName = GetReportDataStructPath(frdName);
            DataSet ds = CreateDataSet(frdFullName);
            return ds;
        }
        private string GetReportDataStructPath(string frdName)
        {
            return Path.Combine(SystemInfo.ApplicationBaseDirectory, "dataset", frdName);
        }
        private DataSet CreateDataSet(string frdFile)
        {
            DataSet ds = XMLTools.ConvertFRDFile2DataSet(frdFile);
            return ds;
        }
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
        #endregion

    }
}
