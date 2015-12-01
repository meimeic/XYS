using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

using XYS.Lis;
using XYS.Lis.Util;
using XYS.Common;
namespace ZhTest
{
    public partial class Form1 : Form
    {
        static IReport report = ReportManager.GetReporter(typeof(Form1), "jsonStrategy");
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LisSearchRequire re = new LisSearchRequire(5);

            re.EqualFields.Add("SerialNo", textBox1.Text);
            string result = report.ReportExport(re);
            System.Console.Write(result);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            LisMap.InitDataSetXmlStruct();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ReportKey searchKey = new LisDBKeyImpl();
            KeyColumn kc = new KeyColumn();
            kc.Name = "patno";
            kc.PK = "396039";
            searchKey.AddKey(kc);
            KeyColumn kc2 = new KeyColumn();
            kc2.Name = "r.receivedate";
            kc2.PK = "2015-02-05";
            searchKey.AddKey(kc2);
            KeyColumn kc3 = new KeyColumn();
            kc3.Name = "r.sectionno";
            kc3.PK = 45;
            searchKey.AddKey(kc3);
            string result = report.ReportExport(searchKey);
            System.Console.Write(result);
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
