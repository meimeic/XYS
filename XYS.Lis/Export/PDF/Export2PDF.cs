using System;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text;

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
    public class Export2PDF : ReportExportSkeleton
    {
        #region
        private readonly string m_reportSeparate;
        private readonly Hashtable m_no2ModelPath;
        private static readonly DataSet PDF_DS = new DataSet();
        private static readonly ExportTag DEFAULT_EXPORT = ExportTag.PDF;
        #endregion

        #region 构造函数
        static Export2PDF()
        {
            LisMap.SetDataSet(PDF_DS);
        }
        public Export2PDF()
            : this("Export2PDF")
        {
        }
        public Export2PDF(string name)
            : base(name)
        {
            this.ExportTag = DEFAULT_EXPORT;
            this.m_reportSeparate = ";";
            this.m_no2ModelPath = new Hashtable(20);
        }
        #endregion

        #region
        public string ReportSeparate
        {
            get { return this.m_reportSeparate; }
        }
        #endregion

        #region 重写父类虚方法
        public override string export(ILisReportElement element)
        {
            if (element.ElementTag == ReportElementTag.ReportElement)
            {
                ReportReportElement rre = element as ReportReportElement;
                return InnerReportExport(rre);
            }
            else
            {
                return "";
            }
        }
        public override string export(List<ILisReportElement> reportElementList, ReportElementTag elementTag)
        {
            if (elementTag == ReportElementTag.ReportElement)
            {
                return ReportElementsExport(reportElementList, elementTag);
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region 实现父类抽象方法
        protected override string InnerElementExport(ILisReportElement reportElement)
        {
            throw new NotImplementedException();
        }
        protected override string InnerReportExport(ReportReportElement rre)
        {
            DataSet ds = PDF_DS.Clone();
            string fileFullName = GetPdfFileFullName(rre);
            string modelPath = GetModelPath(rre.PrintModelNo);
            GenderPdf(modelPath, fileFullName, rre, ds);
            return fileFullName;
        }
        protected override string GetSeparateByTag(ReportElementTag elementTag)
        {
            if (elementTag == ReportElementTag.ReportElement)
            {
                return this.ReportSeparate;
            }
            return null;
        }
        #endregion

        #region 重写父类方法
        protected override string ReportElementsExport(List<ILisReportElement> reportElementList, ReportElementTag elementTag)
        {
            string temp;
            ReportReportElement rre;
            StringBuilder sb = new StringBuilder();
            if (elementTag == ReportElementTag.ReportElement && reportElementList.Count > 0)
            {
                foreach (ILisReportElement reportElement in reportElementList)
                {
                    rre = reportElement as ReportReportElement;
                    temp = InnerReportExport(rre);
                    if (temp != null && !temp.Equals(""))
                    {
                        sb.Append(temp);
                        sb.Append(this.ReportSeparate);
                    }
                }
                if (sb.Length > this.ReportSeparate.Length)
                {
                    sb.Remove(sb.Length - this.ReportSeparate.Length, this.ReportSeparate.Length);
                }
            }
            return sb.ToString();
        }
        #endregion

        #region
        private void GenderPdf(string modelFullName, string fileFullName, ReportReportElement rre, DataSet ds)
        {
            FillElement(rre, ds);
            FillReportData(rre.ReportItemTable, ds);
            Report report = new Report();
            report.Load(modelFullName);
            report.RegisterData(ds);
            report.Prepare();
            PDFExport export = new PDFExport();
            report.Export(export, fileFullName);
            report.Dispose();
        }
        private void GenderPdf(string modelFullName, string fileFullName, DataSet ds)
        {
            Report report = new Report();
            report.Load(modelFullName);
            report.RegisterData(ds);
            report.Prepare();
            PDFExport export = new PDFExport();
            report.Export(export, fileFullName);
            report.Dispose();
        }
        private void FillReportData(Hashtable reportData, DataSet ds)
        {
            List<ILisReportElement> tempElementList;
            foreach (DictionaryEntry de in reportData)
            {
                try
                {
                    ReportElementTag elementTag = (ReportElementTag)de.Key;
                    tempElementList = de.Value as List<ILisReportElement>;
                    if (tempElementList != null && tempElementList.Count > 0)
                    {
                        switch (elementTag)
                        {
                            case ReportElementTag.InfoElement:
                                FillElement(tempElementList[0], ds);
                                break;
                            case ReportElementTag.ItemElement:
                            case ReportElementTag.CustomElement:
                                FillElements(tempElementList, ds);
                                break;
                            case ReportElementTag.GraphElement:
                                FillImageElements(GenderImageSortList(tempElementList), ds);
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }
        private void FillElements(List<ILisReportElement> reportElementList, DataSet ds)
        {
            DataTable dt;
            DataRow dr;
            Type elementType;
            PropertyInfo[] props;
            Convert2XmlAttribute cxa;
            ILisReportElement element;
            if (reportElementList.Count > 0)
            {
                foreach (ILisReportElement reportElement in reportElementList)
                {
                    element = reportElement as ILisReportElement;
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
        private SortedList GenderImageSortList(List<ILisReportElement> imageElementList)
        {
            ReportGraphElement reportImage;
            SortedList sl = new SortedList(10);
            foreach (ILisReportElement reportElement in imageElementList)
            {
                reportImage = reportElement as ReportGraphElement;
                if (reportImage != null)
                {
                    sl.Add(reportImage.GraphName, reportImage.GraphImage);
                }
            }
            return sl;
        }
        private void FillImageElements(SortedList imageElementList, DataSet ds)
        {
            int i = 0;
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

        private void FillDataColumn(PropertyInfo p, DataRow dr, ILisReportElement element)
        {
            dr[p.Name] = p.GetValue(element, null);
        }

        #endregion

        #region
        protected virtual string GetPdfFilePath()
        {
            return @"E:\lis\test";
        }
        protected virtual string GetPdfFileName()
        {
            return "Test.pdf";
        }
        protected virtual string GetPdfFileFullName(ReportReportElement rre)
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
        protected object DefaultForType(Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }
        #endregion
    }
}