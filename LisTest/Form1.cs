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
        private readonly LabService service;
        public Form1()
        {
            InitializeComponent();
            this.service = LabService.LService;
            this.service.HandleCompleteEvent += this.log;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<LabPK> pkList = new List<LabPK>(10);
            service.InitReportPK("where serialno='1602224471'", pkList);
            foreach (LabPK pk in pkList)
            {
                ReportElement report = new ReportElement();
                report.ReportPK = pk;
                service.HandleReport(report);
            }
        }
        private void log(ReportElement report)
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
