using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Concurrent;

using XYS.Util;
using XYS.Lis.Report;
using XYS.Lis.Report.Model;
using XYS.FR.Service.Util;
using XYS.FR.Service.Config;
namespace XYS.FR.Service.Lab
{
    public class PDFService
    {
        private readonly ExportPDF Export;
        private readonly BlockingCollection<LabReport> InitRequestQueue;
        private PDFService()
        {
            this.Export = new ExportPDF();
            this.InitRequestQueue = new BlockingCollection<LabReport>(10000);
        }


        public void GenderPDF(LabReport report)
        {
            this.InitRequestQueue.Add(report);
        }

        private string GetModelPath(int sectionNo, List<int> list)
        {
            int modelNo = LabConfigManager.GetModelNo(list);
            if (modelNo <= 0)
            {
                modelNo = LabConfigManager.GetModelNo(sectionNo);
            }
            return LabConfigManager.GetModelPath(modelNo);
        }
        private int GetOrderNo(int sectionNo, List<int> list)
        {
            return 0;
        }
    }
}