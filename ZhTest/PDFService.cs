using System;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using FastReport;
using FastReport.Export.Pdf;

using XYS.Common;
using XYS.Lis;
using XYS.Lis.Core;
using XYS.Lis.Util;
using XYS.Lis.Export.Model;
namespace ZhTest
{
    class PDFService
    {
        #region
        private static readonly DataSet PDF_DS = new DataSet();
        private static readonly Hashtable m_no2FullModelNameMap = new Hashtable(20);
        private static readonly ReportReport PDF_EXPORT = new ReportReport();
        static IReport MyPDFReport = ReportManager.GetReporter(typeof(PDFService));
        #endregion

        #region
        static PDFService()
        {
            FRDataStruct.InitDataStruct(PDF_DS);
            InitModelNameMap();
        }
        public PDFService()
        {
        }
        #endregion

        #region
        public static string GetPDF(string serialNo)
        {
            LisRequire req = new LisRequire();
            PDF_EXPORT.Clear();
            req.EqualFields.Add("serialno", serialNo);
            MyPDFReport.InitReport(PDF_EXPORT, req);
            GenderPDF();
            return null;
        }
        private static void GenderPDF()
        {
            string modelFullName = GetPrintModelName(PDF_EXPORT.PrintModelNo);
            if (modelFullName == null)
            {
                throw new ArgumentNullException("modelName");
            }
            Report report = new Report();
            report.Load(modelFullName);
            report.RegisterData(PDF_DS);
            report.Prepare();
            PDFExport export = new PDFExport();
            report.Export(export, "E:\\xys\\test\\lis\\temp.pdf");
            report.Dispose();
        }
        #endregion

        #region

        private static void FillReport(ReportReport report, DataSet ds)
        {
            FillElement(report, ds);
            FillElement(report.ReportExam, ds);
            FillElement(report.ReportPatient, ds);
            if (IsImage(report.ReportImage))
            {
                FillElement(report.ReportImage, ds);
            }
            if (report.ReportItemTable.Count > 0)
            {
                string tableName = null;
                List<IExportElement> elementList = null;
                foreach (DictionaryEntry de in report.ReportItemTable)
                {
                    tableName = de.Key as string;
                    elementList = de.Value as List<IExportElement>;
                    if (elementList != null && elementList.Count > 0)
                    {
                        FillElements(elementList, ds, tableName);
                    }
                }
            }
        }
        private static void FillElement(IExportElement element, DataSet ds)
        {
            Type elementType = element.GetType();
            PropertyInfo[] props = elementType.GetProperties();
            if (props == null || props.Length == 0)
            {
                return;
            }
            DataTable dt = ds.Tables[elementType.Name];
            DataRow dr = dt.NewRow();
            foreach (PropertyInfo prop in props)
            {
                if (IsExport(prop))
                {
                    FillDataColumn(prop, dr, element);
                }
            }
            dt.Rows.Add(dr);
        }
        private static void FillElement(IExportElement element, DataRow dr)
        {
            PropertyInfo[] props = element.GetType().GetProperties();
            if (props == null || props.Length == 0)
            {
                return;
            }
            foreach (PropertyInfo prop in props)
            {
                if (IsExport(prop))
                {
                    FillDataColumn(prop, dr, element);
                }
            }
        }
        private static void FillElements(List<IExportElement> exportElementList, DataSet ds, string tableName)
        {
            DataRow dr = null;
            DataTable dt = ds.Tables[tableName];
            foreach (IExportElement element in exportElementList)
            {
                dr = dt.NewRow();
                FillElement(element, dr);
                dt.Rows.Add(dr);
            }
        }
        private static void FillDataColumn(PropertyInfo p, DataRow dr, IExportElement element)
        {
            dr[p.Name] = p.GetValue(element, null);
        }
        #endregion

        #region
        private static string GetPrintModelName(int modelNo)
        {
            if (m_no2FullModelNameMap.Count == 0)
            {
                InitModelNameMap();
            }
            return m_no2FullModelNameMap[modelNo] as string;
        }
        #endregion
        #region
        private static bool IsImage(IExportElement element)
        {
            if (element != null)
            {
                return typeof(FRImage).Equals(element.GetType());
            }
            return false;
        }
        private static bool IsReport(Type type)
        {
            return typeof(ReportReport).Equals(type);
        }
        private static bool IsExport(Type type)
        {
            return FRDataStruct.IsExport(type);
        }
        private static bool IsExport(PropertyInfo prop)
        {
            return FRDataStruct.IsExport(prop);
        }
        private static void InitModelNameMap()
        {
            m_no2FullModelNameMap.Clear();
            lock (m_no2FullModelNameMap)
            {
                LisMap.InitModelNo2ModelPathTable(m_no2FullModelNameMap);
            }
        }
        #endregion
    }
}
