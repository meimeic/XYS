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
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ReportKey searchKey = new LisDBKeyImpl();
            KeyColumn kc = new KeyColumn();
            kc.Name = "patno";
            kc.Value = "396039";
            searchKey.AddColumn(kc);
            KeyColumn kc2 = new KeyColumn();
            kc2.Name = "r.receivedate";
            kc2.Value = "2015-02-05";
            searchKey.AddColumn(kc2);
            KeyColumn kc3 = new KeyColumn();
            kc3.Name = "r.sectionno";
            kc3.Value = 45;
            searchKey.AddColumn(kc3);
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
