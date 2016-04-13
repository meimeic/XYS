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
using XYS.Report.Lis;
using XYS.Report.Lis.Model;
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
            req.EqualFields.Add("patno", "438369");
            req.EqualFields.Add("sectionno",27);
            req.EqualFields.Add("hospitalizedtimes",3);
            List<ReportReportElement> reportList = Report.GetReports(req);
            foreach (ReportReportElement rre in reportList)
            {
                HandlerResult res= Report.operate(rre);
                Console.WriteLine(res.Message);
            }
        }
    }
}
