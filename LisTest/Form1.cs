using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using XYS.Model;
using XYS.Report;
using XYS.Model.Lab;

using XYS.Lis.Report;
namespace LisTest
{
    public partial class Form1 : Form
    {
        private readonly LabService service;
        private readonly FRService.PDFSoapClient client;
        public Form1()
        {
            InitializeComponent();
            this.service = LabService.LService;
            this.service.HandleCompleteEvent += this.PrintPDF;
            this.client = new FRService.PDFSoapClient("PDFSoap");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string where = "where serialno='1602237378'";
            this.service.InitReport(where);
        }
        private void PrintPDF(LabReport report)
        {
            byte[] bytes = Helper.SerializeObject(report);
            this.client.LabPDF(bytes);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
