using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using XYS.Lis.DAL;
using XYS.Lis.Core;
using XYS.Lis.Model;
namespace ZhTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hashtable equalTable = new Hashtable();
            equalTable.Add("receivedate","2015-02-14");
            equalTable.Add("sectionno", 34);
            equalTable.Add("testtypeno", 1);
            equalTable.Add("sampleno", "4");
            LisReportPatientDAL lrpDAL = new LisReportPatientDAL();
            lrpDAL.Search(equalTable);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hashtable equalTable = new Hashtable();
            equalTable.Add("receivedate", "2015-02-14");
            equalTable.Add("sectionno", 34);
            equalTable.Add("testtypeno", 1);
            equalTable.Add("sampleno", "4");
            LisReportExamDAL lreDAL = new LisReportExamDAL();
            lreDAL.Search(equalTable);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Hashtable equalTable = new Hashtable();
            equalTable.Add("receivedate", "2015-02-14");
            equalTable.Add("sectionno", 28);
            equalTable.Add("testtypeno", 1);
            equalTable.Add("sampleno", "4057");
            LisReportGraphDAL lrgDAL = new LisReportGraphDAL();
            lrgDAL.SearchList(equalTable);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Hashtable equalTable = new Hashtable();
            equalTable.Add("receivedate", "2015-02-14");
            equalTable.Add("sectionno", 28);
            equalTable.Add("testtypeno", 1);
            equalTable.Add("sampleno", "4057");
            LisReportItemDAL lriDAL = new LisReportItemDAL();
            lriDAL.SearchList(equalTable);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Hashtable equalTable = new Hashtable();
            equalTable.Add("receivedate", "2015-02-14");
            equalTable.Add("sectionno", 28);
            equalTable.Add("testtypeno", 1);
            equalTable.Add("sampleno", "4057");
            ReportReportElement rre = new ReportReportElement();
            LisReportCommonDAL.FillList(rre.CustomItemList,typeof(ReportCommonItemElement),equalTable);
        }
    }
}
