using System;
using System.IO;
using System.Data;

using XYS.FR.Util;
using XYS.FR.Model;

using FastReport.Utils;
using FastReport.Export.Pdf;
namespace XYS.FR.Lab
{
    public class PDFTest
    {
        private ExportData pdf;

        static PDFTest()
        {
            Config.WebMode = true;
            Config.ReportSettings.ShowProgress = false;
        }
        public PDFTest()
        {
            this.pdf = new ExportData();
        }
        public void Test()
        {
            FRInfo data = new FRInfo();
            data.C0 = "te";
            data.C1 = "t1";
            string model = "D:\\Project\\VS2013\\Repos\\XYS\\XYS.FR\\Print\\Model\\test.frx";
            DataSet ds = DataStruct.GetSet();
            this.pdf.ExportElement(data, ds);
            GenderPDF(model, ds);
        }

        private string GenderPDF(string model, DataSet ds)
        {
            FastReport.Report report = new FastReport.Report();
            report.Load(model);
            report.RegisterData(ds);

            //report.Prepare();
            report.PreparePhase1();
            report.PreparePhase2();

            //初始化输出类
            PDFExport export = new PDFExport();
            //输出
            string path = "E:\\lis\\temp.pdf";
            export.Export(report, path);

            return path;
        }
    }
}