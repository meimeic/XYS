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
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
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
            string where = "where receivedate>'2016-02-22' and sectionno=4";
            this.service.InitReport(where);
        }
        private void PrintPDF(LabReport report)
        {
            byte[] re = TransHelper.SerializeObject(report);
            this.client.LabPDF(re);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string where = "where serialno='" + textBox2.Text + "'";
            this.service.InitReport(where);
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
