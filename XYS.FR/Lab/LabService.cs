using System;
using System.IO;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;

using XYS.Util;
using XYS.Report;
using XYS.Model;
using XYS.Model.Lab;

using XYS.FR.Util;
using XYS.FR.Conf;
using XYS.FR.Model;

using FastReport.Utils;
using FastReport.Export.Pdf;
namespace XYS.FR.Lab
{
    public delegate void GenderErrorHandler(LabReport report);
    public delegate void GenderSuccessHandler(LabReport report);
    public class LabService
    {
        #region 静态属性
        private static string RootPath;
        private static int WorkerCount;
        private static readonly bool HalfEnable;
        private static readonly DateTime MinTime;
        private static readonly LabService ServiceInstance;
        #endregion

        #region 实例属性
        private readonly LabDAL DAL;
        private Thread[] m_workerPool;
        private readonly ExportData Export;
        private readonly Hashtable Image2ImageMap;
        private readonly BlockingCollection<LabReport> RequestQueue;
        #endregion

        #region 私有事件
        private event GenderErrorHandler m_genderErrorEvent;
        private event GenderSuccessHandler m_genderSuccessEvent;
        #endregion

        #region 构造函数
        static LabService()
        {
            WorkerCount = 2;
            Config.WebMode = true;
            RootPath = "E:\\pdf\\report\\lab";
            MinTime = new DateTime(2011, 1, 1);
            Config.ReportSettings.ShowProgress = false;

            ServiceInstance = new LabService();
        }
        private LabService()
        {
            this.DAL = new LabDAL();
            this.Export = new ExportData();
            this.Image2ImageMap = new Hashtable(40);
            this.RequestQueue = new BlockingCollection<LabReport>(10000);

            this.Init();
        }
        #endregion

        #region 静态属性
        public static LabService PService
        {
            get { return ServiceInstance; }
        }
        #endregion

        #region 事件属性
        public event GenderErrorHandler GenderErrorEvent
        {
            add { this.m_genderErrorEvent += value; }
            remove { this.m_genderErrorEvent -= value; }
        }
        public event GenderSuccessHandler GenderSuccessEvent
        {
            add { this.m_genderSuccessEvent += value; }
            remove { this.m_genderSuccessEvent -= value; }
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
            //数据填充
            this.FillInfo(report.Info, ds);
            this.FillItems(report.ItemList, ds);
            this.FillImage(report.ImageList, ds);
            this.FillCustoms(report.CustomList,ds);
            //获取模板
            //string model = GetModelPath(sectionNo, 0, report.SuperList);
            if (report.ImageList.Count > 0)
            {
            }
            else
            {
            }
            string model = "D:\\Project\\VS2013\\Repos\\XYS\\XYS.FR\\Print\\Lab\\peixing-difen.frx";
            //生成pdf,获取pdf路径
            string filePath = this.GenderPDF(model, ds);
            //获取排序号
            int orderNo = GetOrderNo(sectionNo, report.SuperList);
            //保存生成记录
            this.DAL.SaveRecord(report.Info, orderNo, filePath);
            //触发生成pdf成功事件
            this.OnSuccess(report);
        }
        private string GenderPDF(string model, DataSet ds)
        {
            //
            using (FastReport.Report report = new FastReport.Report())
            {
                report.Load(model);
                report.RegisterData(ds);
                report.Prepare(); //报告准备
                //report.PreparePhase1();
                //report.PreparePhase2();
                //初始化PDF输出类
                using (PDFExport export = new PDFExport())
                {
                    this.InitPDFExport(export);
                    //输出PDF
                    string fileFullName = GetFileFullName();
                    export.Export(report, fileFullName);
                    return fileFullName;
                }
            }
        }
        private string GetFileFullName()
        {
            string fileName = SystemInfo.NewGuid().ToString() + ".pdf";
            string filePath = Path.Combine(RootPath, DateTime.Now.ToString("yyyyMMdd"));
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            return Path.Combine(filePath, fileName);
        }
        private void InitPDFExport(PDFExport export)
        {
        }
        #endregion

        #region 触发事件
        protected void OnError(LabReport report)
        {
            GenderErrorHandler handler = this.m_genderErrorEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        protected void OnSuccess(LabReport report)
        {
            GenderSuccessHandler handler = this.m_genderSuccessEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        #endregion

        #region 将数据填充到DataSet
        private void FillReport(LabReport report,DataSet ds)
        {

        }
        private void FillInfo(InfoElement info, DataSet ds)
        {
            FRInfo header = new FRInfo();
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

            header.C17 = info.CollectTime > MinTime ? info.CollectTime.ToString("yyyy-MM-dd HH:mm") : "";
            header.C18 = info.InceptTime > MinTime ? info.InceptTime.ToString("yyyy-MM-dd HH:mm") : "";
            header.C19 = info.CheckTime > MinTime ? info.CheckTime.ToString("yyyy-MM-dd HH:mm") : "";
            header.C20 = info.TestTime > MinTime ? info.TestTime.ToString("yyyy-MM-dd") : "";

            header.C21 = info.TechnicianUrl;
            header.C22 = info.CheckerUrl;

            header.C30 = info.TechnicianImage;
            header.C31 = info.CheckerImage;

            this.Export.ExportElement(header, ds);
        }
        private void FillItems(List<ItemElement> items, DataSet ds)
        {
            FRItem data = null;
            Custom custom = new Custom();
            List<IExportElement> ls = new List<IExportElement>(16);
            items.Sort();
            foreach (ItemElement item in items)
            {
                data = new FRItem();
                this.FillItem(item, data);
                ls.Add(data);
            }
            this.Export.ExportElement(custom, ds);
            this.Export.ExportElement(ls, ds);
        }
        private void FillItem(ItemElement item, FRItem data)
        {
            data.C0 = item.CName;
            data.C1 = item.EName;
            data.C2 = item.Result;
            data.C3 = item.Status;
            data.C4 = item.Unit;
            data.C5 = item.RefRange;
            data.C6 = item.ItemNo.ToString();
        }
        private void FillImage(List<ImageElement> images, DataSet ds)
        {
            string propertyName = null;
            Image image = new Image();

            foreach (ImageElement img in images)
            {
                propertyName = GetImagePropName(img.Name);
                if (!string.IsNullOrEmpty(propertyName))
                {
                    this.SetProperty(propertyName, img.Value, image);
                }
            }
            this.Export.ExportElement(image, ds);
        }
        private string GetImagePropName(int no)
        {
            int m = no % Image.ColumnCount;
            return "C" + m;
        }
        private string GetImagePropName(string name)
        {
            return Image2ImageMap[name] as string;
        }
        private void FillCustoms(List<CustomElement> customs, DataSet ds)
        {
            FRData data = null;
            List<IExportElement> ls = new List<IExportElement>(3);
            foreach (CustomElement custom in customs)
            {
                data = new FRData();
                this.FillCustom(custom, data);
                ls.Add(data);
            }
            this.Export.ExportElement(ls, ds);
        }
        private void FillCustom(CustomElement custom, FRData data)
        {
            PropertyInfo[] props = data.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                PropertyInfo prop1 = custom.GetType().GetProperty(prop.Name);
                if (prop1 != null)
                {
                    prop.SetValue(data, prop1.GetValue(custom), null);
                }
            }
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

        #region 获取模板路径及报告序号
        private string GetModelPath(int sectionNo, int itemCount, List<int> list)
        {
            int modelNo = -1;
            //按小组划分模板
            //switch (sectionNo)
            //{
            //    //根据小组号获得固定死模板
            //    case 2:
            //    case 27:
            //        modelNo = LabConfigManager.GetModelNo(sectionNo);
            //        break;
            //    //有图模板
            //    case 11:
            //        modelNo = GetModelNo(sectionNo, list);
            //        break;
            //}
            modelNo = this.GetModelNo(sectionNo, list);
            return LabConfigManager.GetModelPath(modelNo);
        }
        //获取通用模板
        private int GetModelNo(int sectionNo, int itemCount, List<int> list)
        {
            int modelNo = LabConfigManager.GetModelNo(list);
            if (modelNo <= 0)
            {
                modelNo = GetModelNo(sectionNo, itemCount);
            }
            return modelNo;
        }
        //获取有图模板
        private int GetModelNo(int sectionNo, List<int> list)
        {
            int modelNo = LabConfigManager.GetModelNo(list);
            if (modelNo <= 0)
            {
                modelNo = LabConfigManager.GetModelNo(sectionNo);
            }
            return modelNo;
        }
        private int GetModelNo(int sectionNo, int itemCount)
        {
            int modelNo = LabConfigManager.GetModelNo(sectionNo);
            if (itemCount < 17 && HalfEnable)
            {
                modelNo++;
            }
            return modelNo;
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
            this.InitImage2ImageMap();
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
        private void InitImage2ImageMap()
        {
            this.Image2ImageMap.Clear();
            this.Image2ImageMap.Add("S_FLHxS", "C0");
            this.Image2ImageMap.Add("S_FLLxS", "C1");
            this.Image2ImageMap.Add("S_SSCxS", "C2");
            this.Image2ImageMap.Add("B_FLHxB1", "C3");
            this.Image2ImageMap.Add("S_FSCWxS", "C4");
            this.Image2ImageMap.Add("S_FLLWxS", "C5");
            this.Image2ImageMap.Add("图像1", "C0");
            this.Image2ImageMap.Add("normal", "C1");
            this.Image2ImageMap.Add("图谱", "C0");
            this.Image2ImageMap.Add("蛋白电泳", "C1");
        }
        #endregion
    }
}