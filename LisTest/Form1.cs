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
using System.Threading;
namespace LisTest
{
    public partial class Form1 : Form
    {
        private readonly LabService service;
        private readonly string m_connectionString;
        //private readonly FRService.LabPDFSoapClient PDFClient;
        //private readonly MongoService.LabMongoSoapClient MongoClient;

        private readonly ReportService.ReportStatusSoapClient ReportClient;

        static Form1()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        public Form1()
        {
            InitializeComponent();
            this.service = LabService.LService;
            this.service.HandleCompleteEvent += this.PrintPDF;
            //this.PDFClient = new FRService.LabPDFSoapClient("LabPDFSoap");
            //this.MongoClient = new MongoService.LabMongoSoapClient("LabMongoSoap");
            this.ReportClient = new ReportService.ReportStatusSoapClient("ReportStatusSoap");
            this.m_connectionString = ConfigurationManager.ConnectionStrings["LabMSSQL"].ConnectionString;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string where = " where sectionno=27 and ReceiveDate>'2016-02-20'";
            this.service.InitReport(where);
        }
        private void PrintPDF(LabReport report)
        {
            byte[] re = TransHelper.SerializeObject(report);
            //this.PDFClient.PrintPDF(re);
        }

        private void SaveMongo(LabReport report)
        {
            byte[] re = TransHelper.SerializeObject(report);
            //this.MongoClient.SaveToMongo(re);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sql = "select top 100000 serialno from reportform where SectionNo in (2,27) and serialno is not null and serialno<>'' and ReceiveDate>'2014-12-31' and ReceiveDate<'2016-01-01'";
            DataTable dt = this.Query(sql).Tables["dt"];
            foreach (DataRow dr in dt.Rows)
            {
                string req = this.Request(dr["serialno"].ToString());
                this.ReportClient.UpdateLabApplyInfo(req);
                Thread.Sleep(3);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string req = this.Request(textBox2.Text);
            this.ReportClient.UpdateLabApplyInfo(req);
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }


        private string Request(string serialno)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.Append("<root><visit_type>2</visit_type>");
            sb.Append("<patient_id>346097</patient_id><visit_no>1</visit_no>");
            sb.Append("<operator>尚磊</operator><applys>");
            sb.Append("<apply><apply_no>");
            sb.Append(serialno.Trim());
            sb.Append("</apply_no><apply_status>7</apply_status>");
            sb.Append("<operator>尚磊</operator></apply></applys>");
            sb.Append("</root>");
            return sb.ToString();
        }
        private DataSet Query(string SQLString)
        {
            using (SqlConnection con = new SqlConnection(this.m_connectionString))
            {
                try
                {
                    DataSet ds = new DataSet();
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(SQLString, con);
                    da.Fill(ds, "dt");
                    return ds;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }
    }
}
