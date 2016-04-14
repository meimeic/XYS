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
using XYS.Report.Lis.Persistence.Mongo;
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
            Require req = new Require();
            req.EqualFields.Add("SerialNo", "1602223836");
            req.EqualFields.Add("sectionno",28);
            List<ReportReportElement> reportList = Report.GetReports("where SectionNo=27 and  ReceiveDate>'2016-01-01' and ReceiveDate<'2016-01-07' ");
            foreach (ReportReportElement rre in reportList)
            {
                HandlerResult res= Report.operate(rre);
                Console.WriteLine(res.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MongoService.Query();
        }
    }
}
