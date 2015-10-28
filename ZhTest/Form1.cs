using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using XYS.Lis.Util;
using XYS.Lis.Config;
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
            ConfigureModel();
        }

        private void button2_Click(object sender, EventArgs e)
        {
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
          
        }

        private void button4_Click(object sender, EventArgs e)
        {
          
        }

        private void button5_Click(object sender, EventArgs e)
        {
        
        }
        private void ConfigureModel()
        {

            ReportModelElement modelElement;
            ParamSection paramSection = (ParamSection)ConfigurationManager.GetSection("lis-param");
            ReportModelElementCollection modelCollection = paramSection.ModelCollection;
            foreach (ConfigurationElement element in modelCollection)
            {
                modelElement = element as ReportModelElement;
                if (modelElement != null)
                {
                    ConfigureModel(modelElement);
                }
            }
        }
        private void ConfigureModel(ReportModelElement model)
        {
            if (model.Value != null && model.ModelPath != null)
            {
                string name = model.Name;
                int modelNo = (int)model.Value;
                string path = model.ModelPath;
                ReportModel reportModel = new ReportModel(modelNo, path, name);
               // MODEL_MAP.Add(reportModel);
            }
        }
    }
}
