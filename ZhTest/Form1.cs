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
using System.Xml;
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
             List<ReportReportElement> reportList = Report.GetReports("where SectionNo in(2,27,28,62) and  ReceiveDate>'2016-01-01' and ReceiveDate<'2016-01-07' ");
            // List<ReportReportElement> reportList = Report.GetReports("where SectionNo in(2,27,28,62,17,23,29,34,5,19,20,21,25,30,33,35,63,14,4,24,18,11) and  ReceiveDate>'2016-01-01' and ReceiveDate<'2016-01-07' ");
            //List<ReportReportElement> reportList = Report.GetReports(req);
            //foreach (ReportReportElement rre in reportList)
            //{
            //    result = new HandlerResult();
            //    Report.Operate(rre, result);
            //    Console.WriteLine(result.Code + "    " + result.Message);
            //}
            Report.Operate(reportList);
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

        private void button4_Click(object sender, EventArgs e)
        {
            ApplyItem item = null;
            LabApplyInfo info = new LabApplyInfo();
            info.ClinicType = 2;
            info.PatientID = "346097";
            info.VisitTimes = 1;
            info.Operator = "尚磊";
            List<ApplyItem> appList = new List<ApplyItem>();
            item = new ApplyItem();
            item.ApplyNo = "1603284392";
            item.ApplyStatus = 7;
            item.Operator = "尚磊";
            appList.Add(item);
            info.ApplyCollection = appList;

            try
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(@"test.txt"))
                {
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(LabApplyInfo));
                    xs.Serialize(writer, info);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string xml = @"<root>
	                                    <visit_type>2</visit_type>
	                                    <patient_id>346097</patient_id>
	                                    <visit_no>1</visit_no>
	                                    <operator>尚磊</operator>
	                                    <applys>
		                                    <apply>
			                                    <apply_no>1603284392</apply_no>
			                                    <apply_status>7</apply_status>
			                                    <operator>尚磊</operator>
		                                    </apply>
	                                    </applys>
                                      </root>";
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.LoadXml(xml);
          //  XmlNodeReader reader = new XmlNodeReader(doc.DocumentElement);
            StringReader reader = new StringReader(xml);
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(LabApplyInfo));
            LabApplyInfo info = (LabApplyInfo)xs.Deserialize(reader);
            if (info.ApplyCollection != null && info.ApplyCollection.Count > 0)
            {
                foreach (ApplyItem item in info.ApplyCollection)
                {
                    Console.WriteLine(item.ApplyNo);
                }
            }
        }
    }
}
