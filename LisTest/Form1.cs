using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using XYS.Report;
using XYS.Lis.Report;
using XYS.Lis.Report.Model;
namespace LisTest
{
    public partial class Form1 : Form
    {
        private readonly ReportService service;
        public Form1()
        {
            InitializeComponent();
            this.service = ReportService.LabService;
            this.service.HandleCompleteEvent += this.log;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<ReportPK> pkList = new List<ReportPK>(10);
            service.InitReportPK("where serialno='1602224471'", pkList);
            foreach (ReportPK pk in pkList)
            {
                LabReport report = new LabReport();
                report.ReportPK = pk;
                service.HandleReport(report);
            }
        }
        private void log(LabReport report)
        {
            List<IFillElement> infoList = report.ItemTable["InfoElement"] as List<IFillElement>;
            if (infoList != null)
            {
                this.textBox1.Text = infoList.Count.ToString();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
