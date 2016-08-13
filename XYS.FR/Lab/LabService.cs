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
        private static readonly DateTime MinTime;
        private static readonly LabService ServiceInstance;
        #endregion
        
        #region 实例属性
        private Thread[] m_workerPool;
        private readonly LabDAL DAL;
        private readonly ExportData Export;
        private readonly Hashtable Item2CustomMap;
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
            RootPath = "E:\\report\\lab";
            MinTime = new DateTime(2011, 1, 1);
            Config.ReportSettings.ShowProgress = false;

            ServiceInstance = new LabService();
        }
        private LabService()
        {
            this.DAL = new LabDAL();
            this.Export = new ExportData();
            this.Item2CustomMap = new Hashtable(20);
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
            List<int> superList = new List<int>(16);
            //数据填充
            this.FillInfo(report.Info, ds);
            this.FillItems(report.ItemList, superList, ds);
            this.FillImage(report.ImageList, ds);
            //获取模板
            string model = GetModelPath(sectionNo, superList);
            //生成pdf,获取pdf路径
            string filePath = this.GenderPDF(model, ds);
            //获取排序号
            int orderNo = GetOrderNo(sectionNo, superList);
            //保存生成记录
            this.DAL.SaveRecord(report.Info, orderNo, filePath);
            //触发生成pdf成功事件
            this.OnSuccess(report);
        }
        private string GenderPDF(string model, DataSet ds)
        {
            //
            FastReport.Report report = new FastReport.Report();
            report.Load(model);
            report.RegisterData(ds);
            //report.Prepare(); 报告准备
            report.PreparePhase1();
            report.PreparePhase2();
            //初始化PDF输出类
            PDFExport export = new PDFExport();
            this.InitExport(export);
            //输出PDF
            string fileFullName = GetFileFullName();
            export.Export(report, fileFullName);
            //释放资源
            report.Dispose();

            return fileFullName;
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
        private void InitExport(PDFExport export)
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
        private void FillInfo(InfoElement info,DataSet ds)
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

            header.C17 = info.CollectTime>MinTime?info.CollectTime.ToString("yyyy-MM-dd HH:mm"):"";
            header.C18 = info.InceptTime > MinTime ? info.InceptTime.ToString("yyyy-MM-dd HH:mm") : "";
            header.C19 = info.CheckTime > MinTime ? info.CheckTime.ToString("yyyy-MM-dd HH:mm") : "";
            header.C20 = info.TestTime > MinTime ? info.TestTime.ToString("yyyy-MM-dd") : "";

            this.Export.ExportElement(header,ds);
        }
        private void FillItems(List<ItemElement> ls, List<int> superList, DataSet ds)
        {
            FRItem data = null;
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
                    data = new FRItem();
                    this.FillItem(item, data);
                    ls1.Add(data);
                }
            }
            this.Export.ExportElement(custom, ds);
            this.Export.ExportElement(ls1, ds);
        }
        private void FillItem(ItemElement item, FRItem data)
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
            //图片排序
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

        #region 获取模板路径及报告序号
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