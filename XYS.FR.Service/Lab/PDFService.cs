using System;
using System.IO;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;

using FastReport;
using FastReport.Export.Pdf;

using XYS.Util;
using XYS.Report;
using XYS.Model.Lab;

using XYS.FR.Service.Util;
using XYS.FR.Service.Model;
using XYS.FR.Service.Config;
namespace XYS.FR.Service.Lab
{
    public class PDFService
    {
        private static int WorkerCount;
        private static readonly DateTime MinTime;
        private static readonly PDFService ServiceInstance;

        private Thread[] m_workerPool;
        private readonly PDFDAL DAL;
        private readonly ExportPDF Export;
        private readonly Hashtable Item2CustomMap;
        private readonly BlockingCollection<LabReport> RequestQueue;

        #region 构造函数
        static PDFService()
        {
            WorkerCount = 2;
            MinTime = new DateTime(2011, 1, 1);
            ServiceInstance = new PDFService();
        }
        private PDFService()
        {
            this.DAL = new PDFDAL();
            this.Export = new ExportPDF();
            this.Item2CustomMap = new Hashtable(20);
            this.RequestQueue = new BlockingCollection<LabReport>(10000);

            this.Init();
        }
        #endregion

        #region 静态属性
        public static PDFService PService
        {
            get { return ServiceInstance; }
        }
        #endregion

        #region 生产者方法
        public void HandleReport(LabReport report)
        {
            this.RequestQueue.Add(report);
        }
        #endregion

        #region 消费者方法
        protected void Consumer()
        {
            foreach (LabReport report in this.RequestQueue.GetConsumingEnumerable())
            {
                this.GenderPDF(report);
            }
        }
        protected void GenderPDF(LabReport report)
        {
            DataSet ds = DataStruct.GetSet();
            int sectionNo = report.Info.SectionNo;
            List<int> superList = new List<int>(16);

            this.FillInfo(report.Info, ds);
            this.FillItems(report.ItemList, superList, ds);
            this.FillImage(report.ImageList, ds);

            string model = GetModelPath(sectionNo, superList);
            string filePath = this.GenderPDF(model, ds);

        }
        private string GenderPDF(string model,DataSet ds)
        {
            FastReport.Report report = new FastReport.Report();
            report.Load(model);
            report.RegisterData(ds);
            report.Prepare();
            PDFExport export = new PDFExport();
            report.Export(export, "E:\\report\\lab\\temp.pdf");
            report.Dispose();
            return "";
        }
        #endregion

        #region 将数据填充到dataset
        private void FillInfo(InfoElement info,DataSet ds)
        {
            Data1 header = new Data1();
            header.C0 = info.SerialNo;
            header.C1 = info.DeptName;
            header.C2 = info.BedNo;
            header.C3 = info.SampleNo;
            header.C4 = info.PatientName;
            header.C5 = info.GenderName;
            header.C6 = info.AgeStr;
            header.C7 = info.PatientID;
            header.C8 = info.SampleTypeName;
            header.C9 = info.ClinicName;
            header.C10 = info.Doctor;
            header.C11 = info.ClinicalDiagnosis;
            header.C12 = info.Explanation;

            header.C13 = info.Memo;
            header.C14 = info.Comment;
            header.C15 = info.Description;
            header.C16 = info.ReportContent;

            header.C17 = info.CollectTime>MinTime?info.CollectTime.ToString("yyyy-MM-dd HH:mm"):"";
            header.C18 = info.InceptTime > MinTime ? info.InceptTime.ToString("yyyy-MM-dd HH:mm") : "";
            header.C19 = info.CheckTime > MinTime ? info.CheckTime.ToString("yyyy-MM-dd HH:mm") : "";
            header.C20 = info.TestTime > MinTime ? info.TestTime.ToString("yyyy-MM-dd") : "";

            this.Export.ExportElement(header,ds);
        }

        private void FillItems(List<ItemElement> ls, List<int> superList, DataSet ds)
        {
            Data3 data = null;
            Custom custom = new Custom();
            List<IExportElement> ls1 = new List<IExportElement>(16);
            ls.Sort();
            foreach (ItemElement item in ls)
            {
                //
                if (!superList.Contains(item.SuperNo))
                {
                    superList.Add(item.SuperNo);
                }
                if (!this.ConvertCustom(item, custom))
                {
                    data = new Data3();
                    this.FillItem(item, data);
                    ls1.Add(data);
                }
            }
            this.Export.ExportElement(custom, ds);
            this.Export.ExportElement(ls1, ds);
        }
        private void FillItem(ItemElement item, Data3 data)
        {
            data.C0 = item.CName;
            data.C1 = item.EName;
            data.C2 = item.Result;
            data.C3 = item.Status;
            data.C4 = item.Unit;
            data.C5 = item.RefRange;
        }
        private bool ConvertCustom(ItemElement item, Custom custom)
        {
            string key = Item2CustomMap[item.ItemNo] as string;
            if (key != null)
            {
                this.SetProperty(key, item.Result, custom);
                return true;
            }
            return false;
        }

        private void FillImage(List<ImageElement> images, DataSet ds)
        {
            int order = 0;
            string propertyName = null;
            Image image = new Image();

            images.Sort();

            foreach (ImageElement img in images)
            {
                propertyName = GetImagePropName(order);
                this.SetProperty(propertyName, img.Url, image);
                order++;
            }
            this.Export.ExportElement(image, ds);
        }
        private string GetImagePropName(int no)
        {
            int m = no % Image.ColumnCount;
            return "C" + m;
        }
        private void SetProperty(string propName, object value, IExportElement element)
        {
            try
            {
                PropertyInfo pro = element.GetType().GetProperty(propName);
                if (pro != null)
                {
                    pro.SetValue(element, value, null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 私有方法获取模板路径及报告序号
        private string GetModelPath(int sectionNo, List<int> list)
        {
            int modelNo = LabConfigManager.GetModelNo(list);
            if (modelNo <= 0)
            {
                modelNo = LabConfigManager.GetModelNo(sectionNo);
            }
            return LabConfigManager.GetModelPath(modelNo);
        }
        private int GetOrderNo(int sectionNo, List<int> itemList)
        {
            int result = LabConfigManager.GetOrderNo(itemList);
            if (result <= 0)
            {
                result = LabConfigManager.GetOrderNo(sectionNo);
            }
            return result;
        }
        #endregion

        #region 私有方法
        private void Init()
        {
            this.InitWorkerPool();
            this.InitItem2CustomMap();
        }
        private void InitWorkerPool()
        {
            Thread th = null;
            this.m_workerPool = new Thread[WorkerCount];
            for (int i = 0; i < WorkerCount; i++)
            {
                th = new Thread(Consumer);
                this.m_workerPool[i] = th;
                th.Start();
            }
        }
        private void InitItem2CustomMap()
        {
            this.Item2CustomMap.Clear();

            this.Item2CustomMap.Add(90009288, "C0");
            this.Item2CustomMap.Add(90009289, "C1");
            this.Item2CustomMap.Add(90009290, "C2");
            this.Item2CustomMap.Add(90009291, "C3");
            this.Item2CustomMap.Add(90009292, "C4");
            this.Item2CustomMap.Add(90009293, "C5");
            this.Item2CustomMap.Add(90009294, "C6");
            this.Item2CustomMap.Add(90009295, "C7");
            this.Item2CustomMap.Add(90009296, "C8");
            this.Item2CustomMap.Add(90009297, "C9");
            this.Item2CustomMap.Add(90009300, "C10");
            this.Item2CustomMap.Add(90009301, "C11");

            this.Item2CustomMap.Add(90008528, "C0");
            this.Item2CustomMap.Add(90008797, "C1");
            this.Item2CustomMap.Add(90008798, "C2");
            this.Item2CustomMap.Add(90008799, "C3");
        }
        #endregion
    }
}