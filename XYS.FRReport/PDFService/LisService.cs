using System;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using FastReport;
using FastReport.Export.Pdf;

using XYS.Util;
using XYS.Model;

using XYS.Report;
using XYS.Report.Util;
using XYS.Report.Model.Lis;

using XYS.FRReport.Util;
namespace XYS.FRReport.PDFService
{
    class LisService
    {
        #region
        private static readonly DataSet PDF_DS = new DataSet();
        private static readonly Hashtable m_no2FullModelNameMap = new Hashtable(20);
        private static readonly ReportReportElement PDF_REPORT = new ReportReportElement();
        static IReport PDFReporter = ReportManager.GetReporter(typeof(LisService));
        #endregion

        #region
        static LisService()
        {
            FRDataStruct.InitDataStruct(PDF_DS);
            InitModelNameMap();
        }
        public LisService()
        {
        }
        #endregion

        #region
        public static string GetPDF(string serialNo)
        {
            Require req = new Require();
            PDF_REPORT.Clear();
            req.EqualFields.Add("serialno", serialNo);
            PDFReporter.InitReport(PDF_REPORT, req);
            FillReport(PDF_REPORT, PDF_DS);
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
        private static void FillReport(ReportReportElement report, DataSet ds)
        {
            FillElement(report, ds);
            FillElement(report.ReportExam, ds);
            FillElement(report.ReportPatient, ds);
            if (report.ReportItemTable.Count > 0)
            {
                string tableName = null;
                List<IReportElement> elementList = null;
                foreach (DictionaryEntry de in report.ReportItemTable)
                {
                    tableName = de.Key as string;
                    elementList = de.Value as List<IReportElement>;
                    if (elementList != null && elementList.Count > 0)
                    {
                        FillElements(elementList, ds, tableName);
                    }
                }
            }
        }
        private static void FillElement(IReportElement element, DataSet ds)
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
        private static void FillElement(IReportElement element, DataRow dr)
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
        private static void FillElements(List<IReportElement> exportElementList, DataSet ds, string tableName)
        {
            DataRow dr = null;
            DataTable dt = ds.Tables[tableName];
            foreach (IReportElement element in exportElementList)
            {
                dr = dt.NewRow();
                FillElement(element, dr);
                dt.Rows.Add(dr);
            }
        }
        private static void FillDataColumn(PropertyInfo p, DataRow dr, IReportElement element)
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
        private static bool IsReport(Type type)
        {
            return typeof(ReportReportElement).Equals(type);
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
