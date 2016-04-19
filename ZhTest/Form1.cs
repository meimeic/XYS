using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using XYS.Report;
using XYS.Report.Lis;
using XYS.Report.Lis.Model;
using XYS.Report.Lis.IO;
namespace ZhTest
{
    public partial class Form1 : Form
    {
        static IReport Report = ReportManager.GetReport(typeof(Form1));
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            HandleResult result = null;
            Require req = new Require();
            req.EqualFields.Add("SerialNo", "1601047306");
            req.EqualFields.Add("sectionno", 11);
            req.EqualFields.Add("SampleNo", "1600005");
             List<ReportReportElement> reportList = Report.GetReports("where SectionNo in(2,27,28,62,17,23,29,34,5,19,20,21,25,30,33,35,63,14,4,24,18,11) and  ReceiveDate>'2016-01-01' and ReceiveDate<'2016-01-07' ");
            //List<ReportReportElement> reportList = Report.GetReports(req);
            //foreach (ReportReportElement rre in reportList)
            //{
            //    result = new HandlerResult();
            //    Report.Operate(rre, result);
            //    Console.WriteLine(result.Code + "    " + result.Message);
            //}
            Report.OperateAsync(reportList);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReportMongoService service = new ReportMongoService();
            service.Test();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ReportMongoService service = new ReportMongoService();
            service.TestAsync();
        }
    }
}
