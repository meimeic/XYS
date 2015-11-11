using System;
using System.Collections;
using System.Data;
using System.Reflection;

using XYS.Lis.Core;
using XYS.Lis.Export;
using XYS.Lis.Model;
using XYS.Lis.Util;
using XYS.Model;
using XYS.Common;

using FastReport;
using FastReport.Export.Pdf;

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
            this.m_ds.Clear();
            string modelPath = GetModelPath(rre.PrintModelNo);
            return GenderPdf(modelPath, rre);
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
        private void FillReportData(Hashtable reportData)
        {
            ILisReportElement tempElement;
            Hashtable tempTable;
            foreach (DictionaryEntry de in reportData)
            {
                try
                {
                    ReportElementTag elementTag = (ReportElementTag)de.Key;
                    switch (elementTag)
                    {
                        case ReportElementTag.ExamElement:
                        case ReportElementTag.PatientElement:
                            tempElement = de.Value as ILisReportElement;
                            if (tempElement != null)
                            {
                                FillElement(tempElement, this.m_ds);
                            }
                            break;
                        case ReportElementTag.ItemElement:
                            tempTable = de.Value as Hashtable;
                            if (tempTable != null)
                            {
                                FillElements(tempTable, this.m_ds);
                            }
                            break;
                        case ReportElementTag.GraphElement:
                            tempTable = de.Value as Hashtable;
                            if (tempTable != null)
                            {
                                FillImageElements(GenderImageTable(tempTable), this.m_ds);
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }
        private void FillElements(Hashtable elementTable,DataSet ds)
        {
            DataTable dt;
            DataRow dr;
            Type elementType;
            PropertyInfo[] props;
            Convert2XmlAttribute cxa;
            ILisReportElement element;
            foreach (DictionaryEntry de in elementTable)
            {
                element = de.Value as ILisReportElement;
                if (element != null)
                {
                    elementType = element.GetType();
                    dt = ds.Tables[elementType.Name];
                    dr = dt.NewRow();
                    props = elementType.GetProperties();
                    if (props == null || props.Length == 0)
                    {
                        continue;
                    }
                    foreach (PropertyInfo p in props)
                    {
                        cxa = (Convert2XmlAttribute)Attribute.GetCustomAttribute(p, typeof(Convert2XmlAttribute));
                        if (cxa != null && cxa.IsConvert)
                        {
                            FillDataColumn(p, dr, element);
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
        }
        private void FillElement(ILisReportElement element, DataSet ds)
        {
            Convert2XmlAttribute cxa;
            Type elementType = element.GetType();
            DataTable dt = ds.Tables[elementType.Name];
            DataRow dr = dt.NewRow();
            PropertyInfo[] props = elementType.GetProperties();
            if (props == null || props.Length == 0)
            {
                return;
            }
            foreach (PropertyInfo p in props)
            {
                cxa = (Convert2XmlAttribute)Attribute.GetCustomAttribute(p, typeof(Convert2XmlAttribute));
                if (cxa != null && cxa.IsConvert)
                {
                    FillDataColumn(p, dr, element);
                }
            }
            dt.Rows.Add(dr);
        }
        private void FillImageElements(SortedList imageElementList, DataSet ds)
        {
            int i = 1;
            int j = 0;
            string columnName;
            string tableName = XMLTools.Image_Table_Name;
            DataTable dt = ds.Tables[tableName];
            DataRow dr = dt.NewRow();
            foreach (DictionaryEntry de in imageElementList)
            {
                j = i % XMLTools.Image_Column_Count;
                columnName = XMLTools.Image_Column_Prex + j;
                dr[columnName] = de.Value;
                i++;
            }
            dt.Rows.Add(dr);
        }
        private SortedList GenderImageTable(Hashtable imageElementTable)
        {
            ReportGraphElement reportImage;
            SortedList sl = new SortedList(10);
            foreach (DictionaryEntry de in imageElementTable)
            {
                reportImage = de.Value as ReportGraphElement;
                if (reportImage != null)
                {
                    sl.Add(reportImage.GraphName, reportImage.GraphImage);
                }
            }
            return sl;
        }

        private string GenderPdf(string modelFullName,ReportReportElement rre)
        {
            FillReportData(rre.ItemTable);
            Report report = new Report();
            report.Load(modelFullName);
            report.RegisterData(this.m_ds);
            report.Prepare();
            PDFExport export = new PDFExport();
            string fileFullName = GetPdfFileFullName();
            report.Export(export, fileFullName);
            report.Dispose();
            return fileFullName;
        }
        private string GetPdfFilePath()
        {
            return @"E:\lis\test";
        }
        private string GetPdfFileName()
        {
            return "Test.pdf";
        }
        private string GetPdfFileFullName()
        {
            return SystemInfo.GetFileFullName(GetPdfFilePath(), GetPdfFileName());
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
        protected void FillDataColumn(PropertyInfo p, DataRow dr,ILisReportElement element)
        {
            dr[p.Name] = p.GetValue(element, null);
        }
        protected object DefaultForType(Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }
        #endregion
    }
}
