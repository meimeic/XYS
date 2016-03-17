using System;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using FastReport;
using FastReport.Export.Pdf;

using XYS.Util;

using XYS.Report.Lis;
using XYS.Report.Lis.Util;
using XYS.Report.Lis.Core;
using XYS.Report.Lis.Model;

using XYS.FRReport.Util;
namespace XYS.FRReport.PDFService
{
    class LisService
    {
        #region
        private static readonly string XmlStructFileName = "LisReport.frd";

        private static readonly DataSet PDF_DS = new DataSet();
        private static readonly Hashtable m_printModelMap = new Hashtable(20);

        private static readonly Hashtable m_section2ModelNoMap = new Hashtable(20);
        private static readonly Hashtable m_item2ModelNoMap = new Hashtable(10);

        private static readonly Hashtable m_section2OrderMap = new Hashtable(20);
        private static readonly Hashtable m_item2OrderMap = new Hashtable(10);

        private static readonly ReportReportElement PDF_REPORT = new ReportReportElement();
        static IReport PDFReporter = ReportManager.GetReporter(typeof(LisService));
        #endregion

        #region
        static LisService()
        {
            List<Type> elementList = new List<Type>();
            ConfigManager.InitAllElementList(elementList);
            FRDataStruct.InitXmlDataStruct(elementList, XmlStructFileName);
            FRDataStruct.InitRawDataStruct(elementList, PDF_DS);
        }
        public LisService()
        {
        }
        #endregion

        #region 公共函数
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
            string modelFullName =SystemInfo.GetFileFullName(SystemInfo.ApplicationBaseDirectory,"PrintModel\\Lis\\lj-xuechanggui.frx");
            if (modelFullName == null)
            {
                throw new ArgumentNullException("modelName");
            }
            FastReport.Report report = new FastReport.Report();
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
            if (report.ReportImageMap != null)
            {
                FillElement(report.ReportImageMap, ds);
            }
            if (report.ReportItemTable.Count > 0)
            {
                Type type = null;
                List<ILisReportElement> elementList = null;
                foreach (DictionaryEntry de in report.ReportItemTable)
                {
                    type = de.Key as Type;
                    elementList = de.Value as List<ILisReportElement>;
                    if (IsExport(type)&&IsExist(elementList))
                    {
                        FillElements(elementList, ds, type);
                    }
                }
            }
        }
        private static void FillElement(ILisReportElement element, DataSet ds)
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
        private static void FillElement(ILisReportElement element, DataRow dr)
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
        private static void FillElement(ILisReportElement element, Type type, DataRow dr)
        {
            PropertyInfo[] props = type.GetProperties();
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
        private static void FillElements(List<ILisReportElement> exportElementList, DataSet ds, Type type)
        {
            DataRow dr = null;
            DataTable dt = ds.Tables[type.Name];
            foreach (ILisReportElement element in exportElementList)
            {
                dr = dt.NewRow();
                FillElement(element, dr);
                dt.Rows.Add(dr);
            }
        }
        private static void FillDataColumn(PropertyInfo p, DataRow dr, ILisReportElement element)
        {
            dr[p.Name] = p.GetValue(element, null);
        }
        #endregion

        #region
        private static string GetPrintModelName(int modelNo)
        {
            if (m_section2ModelNoMap.Count == 0)
            {
                InitModelNameMap();
            }
            return m_section2ModelNoMap[modelNo] as string;
        }
        #endregion

        #region
        private static bool IsExist(List<ILisReportElement> elementList)
        {
            if (elementList != null && elementList.Count > 0)
            {
                return true;
            }
            return false;
        }
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
        }
        #endregion

        #region 获取模板号

        #endregion
    }
}
