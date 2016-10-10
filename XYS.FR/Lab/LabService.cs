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
        private readonly Hashtable Item2CustomMap;
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
            MinTime = new DateTime(2011, 1, 1);

            Config.WebMode = true;
            Config.ReportSettings.ShowProgress = false;
            RootPath = LabConfigManager.GetRootPath();
            HalfEnable = LabConfigManager.GetHalfEnable();
            WorkerCount = LabConfigManager.GetWorkerCount();

            ServiceInstance = new LabService();
        }
        private LabService()
        {
            this.DAL = new LabDAL();
            this.Export = new ExportData();
            this.Image2ImageMap = new Hashtable(40);
            this.RequestQueue = new BlockingCollection<LabReport>(5000);

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
            bool hasImage = (report.ImageList.Count > 0);
            //数据填充
            this.FillReport(report, ds);
            //获取模板
            string model = GetModelPath(sectionNo, report.ItemList.Count, hasImage, report.SuperList);
            //string model = "D:\\Project\\VS2013\\Repos\\XYS\\XYS.FR\\Print\\Lab\\mianyi-adaption.frx";
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
        private void FillReport(LabReport report, DataSet ds)
        {
            //上传info
            FRInfo header = new FRInfo();
            this.FillInfo(report.Info, header);
            this.Export.ExportElement(header, ds);
            //
            int sectionNo = report.Info.SectionNo;
            List<IExportElement> ls = new List<IExportElement>(30);
            switch (sectionNo)
            {
                case 39:
                    this.FillGSCustom(report.CustomList, ls);
                    break;
                case 45:
                    this.FillGeneCustom(report.CustomList,ls);
                    break;
                default:
                    this.FillItems(report.ItemList, ls);
                    this.FillImage(report.ImageList, ls);
                    this.FillCustom(report.CustomList, ls);
                    break;
            }
            this.Export.ExportElement(ls, ds);
        }
        private void FillInfo(InfoElement info, FRInfo header)
        {
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

            header.C29 = info.ReportID;
            header.C30 = info.TechnicianImage;
            header.C31 = info.CheckerImage;
        }
        private void FillItems(List<ItemElement> items, List<IExportElement> ls)
        {
            bool flag = false;
            FRItem data = null;
            FRCustom custom = new FRCustom();
            if (items.Count > 0)
            {
                foreach (ItemElement item in items)
                {
                    if (ConvertCustom(item, custom))
                    {
                        flag = true;
                        continue;
                    }
                    data = new FRItem();
                    this.FillItem(item, data);
                    ls.Add(data);
                }
                if (flag)
                {
                    ls.Add(custom);
                }
            }
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
        private void FillImage(List<ImageElement> images, List<IExportElement> ls)
        {
            string propertyName = null;
            FRImage image = new FRImage();
            if (images.Count > 0)
            {
                foreach (ImageElement img in images)
                {
                    propertyName = GetImageName(img.Name);
                    if (!string.IsNullOrEmpty(propertyName))
                    {
                        this.SetProperty(propertyName, img.Value, image);
                    }
                }
                ls.Add(image);
            }
        }
        private string GetImageName(string name)
        {
            return Image2ImageMap[name] as string;
        }
        private void FillCustom(List<CustomElement> customs, List<IExportElement> ls)
        {
            FRCustom cs = null;
            if (customs.Count > 0)
            {
                foreach (CustomElement custom in customs)
                {
                    cs = new FRCustom();
                    this.FillCustom(custom, cs);
                    ls.Add(cs);
                }
            }
        }
        private void FillGSCustom(List<CustomElement> customs, List<IExportElement> ls)
        {
            //FRData data = new FRData();
            //FRCustom custom = new FRCustom();
            //if (customs.Count > 1)
            //{
            //    this.FillCustom(customs[0], custom);
            //    ls.Add(custom);
            //    //
            //    this.FillCustom(customs[1], data);
            //    ls.Add(data);
            //}
        }

        private void FillGeneCustom(List<CustomElement> customs, List<IExportElement> ls)
        {
            string kv = null;
            FRCustom frc = null;
            foreach (CustomElement custom in customs)
            {
                frc = new FRCustom();
                frc.C0 = custom.C0;
                frc.C1 = custom.C1;
                frc.C2 = custom.C2;
                frc.C3 = custom.C3;
                frc.C4 = custom.C4;
                int index = 5;
                for (int i = 5; i < CustomElement.PropertyCount; i++)
                {
                    kv = GetPropValue("C" + i, custom);
                    if (!string.IsNullOrEmpty(kv))
                    {
                        this.FillGene(kv, index, frc);
                        index += 3;
                    }
                }
                ls.Add(frc);
            }
        }
        private void FillGene(string str, int index, FRCustom custom)
        {
            string propName = null;
            string[] kv = str.Split(new char[] { '@' });
            if (kv.Length > 1)
            {
                propName = GetPropNameByIndex(index);
                this.SetProperty(propName, kv[0], custom);

                string[] vs = kv[1].Split(new char[] { ';' });
                if (vs.Length > 1)
                {
                    index++;
                    propName = GetPropNameByIndex(index);
                    this.SetProperty(propName, vs[0], custom);

                    index++;
                    propName = GetPropNameByIndex(index);
                    this.SetProperty(propName, vs[0], custom);
                }
            }
        }

        private void FillCustom(CustomElement custom, FRCustom frc)
        {
            PropertyInfo[] props = typeof(CustomElement).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                this.SetProperty(prop.Name, prop.GetValue(custom), frc);
                //PropertyInfo prop1 = typeof(FRCustom).GetProperty(prop.Name);
                //if (prop1 != null)
                //{
                //    prop1.SetValue(frc, prop.GetValue(custom), null);
                //}
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
        private string GetPropNameByIndex(int no)
        {
            int m = no % FRCustom.PropertyCount;
            return "C" + m;
        }

        private string GetPropValue(string propName, CustomElement custom)
        {
            PropertyInfo pro = typeof(CustomElement).GetProperty(propName);
            if (pro != null)
            {
                object res = pro.GetValue(custom);
                if (res != null)
                {
                    return res.ToString();
                }
            }
            return null;
        }
        private bool ConvertCustom(ItemElement item, FRCustom custom)
        {
            string key = Item2CustomMap[item.ItemNo] as string;
            if (key != null)
            {
                this.SetProperty(key, item.Result, custom);
                return true;
            }
            return false;
        }
        #endregion

        #region 获取模板路径及报告序号
        //获取模板
        private string GetModelPath(int sectionNo, int itemCount, bool hasImage, List<int> list)
        {
            int modelNo = this.GetModelNo(sectionNo, itemCount, hasImage, list);
            return LabConfigManager.GetModelPath(modelNo);
        }
        private int GetModelNo(int sectionNo, int itemCount, bool hasImage, List<int> list)
        {
            int modelNo = LabConfigManager.GetModelNo(list);
            if (modelNo > 0)
            {
                return HalfModelNo(itemCount, hasImage, modelNo);
            }
            modelNo = LabConfigManager.GetModelNo(sectionNo);
            return HalfModelNo(itemCount, hasImage, modelNo);
        }
        private int HalfModelNo(int itemCount, bool hasImage, int modelNo)
        {
            if (!hasImage && HalfEnable && itemCount < 17)
            {
                modelNo++;
            }
            return modelNo;
        }
        //获取排序号
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
            this.Image2ImageMap.Add("图像2", "C1");
            this.Image2ImageMap.Add("normal", "C1");
            this.Image2ImageMap.Add("图谱", "C0");
            this.Image2ImageMap.Add("蛋白电泳", "C1");
        }
        private void InitItem2CustomMap()
        {
            this.Item2CustomMap.Clear();
            lock (Item2CustomMap)
            {
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
        }
        #endregion
    }
}