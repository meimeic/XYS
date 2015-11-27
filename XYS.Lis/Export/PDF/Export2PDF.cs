using System;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using XYS.Model;
using XYS.Lis.Util;
using XYS.Lis.Core;
using XYS.Common;
using XYS.Lis.Export;
using XYS.Lis.Model.Export;

using FastReport;
using FastReport.Export.Pdf;

namespace XYS.Lis.Export.PDF
{
    public class Export2PDF : ReportExportSkeleton
    {
        #region 只读字段
        private readonly string m_reportSeparate;
        private readonly Hashtable m_no2ModelPath;
        private readonly Hashtable m_itemName2CustomPropertyName;
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
            this.m_itemName2CustomPropertyName = new Hashtable(15);
        }
        #endregion

        #region
        public string ReportSeparate
        {
            get { return this.m_reportSeparate; }
        }
        #endregion

        //#region 重写父类虚方法
        //public override string export(ILisExportElement element)
        //{
        //    if (element.ElementTag == ReportElementTag.ReportElement)
        //    {
        //        ReporterReport rr = element as ReporterReport;
        //        return InnerReportExport(rr);
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}
        //public override string export(List<ILisExportElement> reportElementList, ReportElementTag elementTag)
        //{
        //    if (elementTag == ReportElementTag.ReportElement)
        //    {
        //        return InnerExport(reportElementList, elementTag);
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}
        //#endregion

        #region 实现父类抽象方法
        protected override string InnerExport(ILisExportElement exportElement)
        {
            ReporterReport rr;
            if (exportElement.ElementTag == ReportElementTag.ReportElement)
            {
                rr = exportElement as ReporterReport;
                if (rr != null)
                {
                    return InnerReportExport(rr);
                }
                return "";
            }
            else
            {
                return "";
            }
        }
        protected override string InnerExport(List<ILisExportElement> exportElementList, ReportElementTag elementTag)
        {
            string temp;
            ReporterReport rr;
            StringBuilder sb = new StringBuilder();
            if (elementTag == ReportElementTag.ReportElement)
            {
                if (exportElementList.Count > 0)
                {
                    foreach (ILisExportElement exportElement in exportElementList)
                    {
                        if (elementTag == ReportElementTag.ReportElement)
                        {
                            rr = exportElement as ReporterReport;
                            temp = InnerReportExport(rr);
                            if (temp != null && !temp.Equals(""))
                            {
                                sb.Append(temp);
                                sb.Append(this.ReportSeparate);
                            }
                        }
                    }
                    if (sb.Length > this.ReportSeparate.Length)
                    {
                        sb.Remove(sb.Length - this.ReportSeparate.Length, this.ReportSeparate.Length);
                    }
                }
                return sb.ToString();
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region
        protected virtual string InnerReportExport(ReporterReport rr)
        {
            DataSet ds = PDF_DS.Clone();
            string fileFullName = GetPdfFileFullName(rr);
            string modelPath = GetModelPath(rr.PrintModelNo);
            GenderPdf(modelPath, fileFullName, rr, ds);
            return fileFullName;
        }
        private void GenderPdf(string modelFullName, string fileFullName, ReporterReport rr, DataSet ds)
        {
            FillReport(rr, ds);
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

        private void FillReport(ReporterReport rr, DataSet ds)
        {
            FillElement(rr, ds);
            if (rr.ReportInfo != null)
            {
                FillElement(rr.ReportInfo, ds);
            }
            if(rr.ItemList!=null)
            {
                FillItemElements(rr.ItemList, ds);
            }
            if (rr.GraphList != null)
            {
                FillGraphElements(rr.GraphList, ds);
            }
            if (rr.CustomList != null)
            {
                FillCustomElements(rr.CustomList, ds);
            }
            if (rr.KVList != null)
            {
                FillKVElements(rr.KVList,ds);
            }
        }
        private void FillItemElements(Dictionary<int, ReporterItem> exportElementDic, DataSet ds)
        {
            if (exportElementDic.Count > 0)
            {
                foreach (ReporterItem ri in exportElementDic.Values)
                {
                    FillElement(ri, ds);
                }
            }
        }
        private void FillItemElements(List<ReporterItem> itemList, DataSet ds)
        {
            if (itemList.Count > 0)
            {
                foreach (ReporterItem ri in itemList)
                {
                    FillElement(ri, ds);
                }
            }
        }
        private void FillGraphElements(Dictionary<string, ReporterGraph> exportElementDic, DataSet ds)
        {
            if (exportElementDic.Count > 0)
            {
                SortedList sl = new SortedList(10);
                foreach (ReporterGraph rg in exportElementDic.Values)
                {
                    sl.Add(rg.GraphName, rg.GraphImage);
                }
                FillImageElements(sl, ds);
            }
        }
        private void FillGraphElements(List<ReporterGraph> graphList, DataSet ds)
        {
            if (graphList.Count > 0)
            {
                SortedList sl = new SortedList(10);
                foreach (ReporterGraph rg in graphList)
                {
                    sl.Add(rg.GraphName, rg.GraphImage);
                }
                FillImageElements(sl, ds);
            }
        }
        private void FillKVElements(Dictionary<string, ReporterKV> exportElementDic, DataSet ds)
        {
            ReporterCustom custom;
            if (exportElementDic.Count > 0)
            {
                foreach(ReporterKV kv in exportElementDic.Values)
                {
                    custom = new ReporterCustom();
                    custom.Name = kv.Name;
                    ConvertKV2Custom(kv, custom);
                    FillElement(custom, ds);
                }
            }
        }
        private void FillKVElements(List<ReporterKV> kvList, DataSet ds)
        {
            ReporterCustom custom;
            if (kvList.Count > 0)
            {
                foreach (ReporterKV kv in kvList)
                {
                    custom = new ReporterCustom();
                    custom.Name = kv.Name;
                    ConvertKV2Custom(kv, custom);
                    FillElement(custom, ds);
                }
            }
        }
        private void FillCustomElements(Dictionary<string, ReporterCustom> exportElementDic, DataSet ds)
        {
            if (exportElementDic.Count > 0)
            {
                foreach(ReporterCustom rc in exportElementDic.Values)
                {
                    FillElement(rc,ds);
                }
            }
        }
        private void FillCustomElements(List<ReporterCustom> customList, DataSet ds)
        {
            if (customList.Count > 0)
            {
                foreach (ReporterCustom rc in customList)
                {
                    FillElement(rc, ds);
                }
            }
        }
        private void FillElements(List<ILisExportElement> exportElementList, DataSet ds)
        {
            DataTable dt;
            DataRow dr;
            Type elementType;
            PropertyInfo[] props;
            //ExportAttribute cxa;
            ILisExportElement element;
            if (exportElementList.Count > 0)
            {
                foreach (ILisExportElement reportElement in exportElementList)
                {
                    element = reportElement as ILisExportElement;
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
                        foreach (PropertyInfo pro in props)
                        {
                            //cxa = (ExportAttribute)Attribute.GetCustomAttribute(p, typeof(ExportAttribute));
                            // if (cxa != null && cxa.IsConvert)
                            if (pro.PropertyType == typeof(string) || pro.PropertyType == typeof(int) || pro.PropertyType == typeof(DateTime) || pro.PropertyType == typeof(byte[]))
                            {
                                FillDataColumn(pro, dr, element);
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
        }
        private void FillElement(ILisExportElement element, DataSet ds)
        {
            //ExportAttribute cxa;
            Type elementType = element.GetType();
            DataTable dt = ds.Tables[elementType.Name];
            DataRow dr = dt.NewRow();
            PropertyInfo[] props = elementType.GetProperties();
            if (props == null || props.Length == 0)
            {
                return;
            }
            foreach (PropertyInfo pro in props)
            {
                //cxa = (ExportAttribute)Attribute.GetCustomAttribute(p, typeof(ExportAttribute));
                //if (cxa != null && cxa.IsConvert)
                if (pro.PropertyType == typeof(string) || pro.PropertyType == typeof(int) || pro.PropertyType == typeof(DateTime) || pro.PropertyType == typeof(byte[]))
                {
                    FillDataColumn(pro, dr, element);
                }
            }
            dt.Rows.Add(dr);
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
        private void FillDataColumn(PropertyInfo p, DataRow dr, ILisExportElement element)
        {
            dr[p.Name] = p.GetValue(element, null);
        }

        private void ConvertKV2Custom(ReporterKV kv, ReporterCustom custom)
        {
            string propertyName;
            if (kv.KVTable.Count > 0)
            {
                foreach (DictionaryEntry de in kv.KVTable)
                {
                    propertyName = GetCustomPropertyName(de.Key.ToString());
                    if (propertyName != null)
                    {
                        SetCustomProperty(propertyName, de.Value.ToString(), custom);
                    }
                }
            }
        }
        private void SetCustomProperty(string propertyName, object value, ReporterCustom rc)
        {
            try
            {
                PropertyInfo pro = rc.GetType().GetProperty(propertyName);
                if (pro != null)
                {
                    pro.SetValue(rc, value, null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string GetCustomPropertyName(string itemName)
        {
            if (this.m_itemName2CustomPropertyName.Count == 0)
            {
                InitItemName2CustomPropertyNameTable();
            }
            return this.m_itemName2CustomPropertyName[itemName] as string;
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
        protected virtual string GetPdfFileFullName(ReporterReport rr)
        {
            return SystemInfo.GetFileFullName(GetPdfFilePath(), GetPdfFileName());
        }
        #endregion

        #region
        private void InitItemName2CustomPropertyNameTable()
        {
 
        }
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

        #region
        //private SortedList GenderImageSortList(List<ILisReportElement> imageElementList)
        //{
        //    ReportGraphElement reportImage;
        //    SortedList sl = new SortedList(10);
        //    foreach (ILisReportElement reportElement in imageElementList)
        //    {
        //        reportImage = reportElement as ReportGraphElement;
        //        if (reportImage != null)
        //        {
        //            sl.Add(reportImage.GraphName, reportImage.GraphImage);
        //        }
        //    }
        //    return sl;
        //}
        #endregion
    }
}