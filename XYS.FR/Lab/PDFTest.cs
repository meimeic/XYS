using System;

using XYS.FR.Util;
using XYS.FR.Model;
namespace XYS.FR.Lab
{
    public class PDFTest
    {
        private ExportPDF pdf;

        public PDFTest()
        {
            this.pdf = new ExportPDF();
        }
        public  void Test()
        {
            Data1 data = new Data1();
            data.C0 = "te";
            data.C1 = "t1";
            string model = "D:\\Project\\VS2013\\Repos\\XYS\\XYS.FR\\Print\\Model\\test.frx";
        }
    }
}