using System;
using System.IO;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;

using XYS.Model;
using XYS.Model.Lab;
using XYS.Mongo.Model;
namespace XYS.Mongo.Lab
{
    public class LabService
    {
        private static int WorkerCount;
        private static readonly LabService ServiceInstance;

        private Thread[] m_workerPool;
        private readonly LabMongo MongoService;
        private readonly BlockingCollection<LabReport> RequestQueue;

        #region 构造函数
        static LabService()
        {
            WorkerCount = 2;
            ServiceInstance = new LabService();
        }
        private LabService()
        {
            this.MongoService = LabMongo.MongoService;
            this.RequestQueue = new BlockingCollection<LabReport>(5000);

            this.InitWorkerPool();
        }
        #endregion

        #region
        public static LabService LService
        {
            get { return ServiceInstance; }
        }
        #endregion

        #region 生产者方法
        public void Save2Mongo(LabReport report)
        {
            this.RequestQueue.Add(report);
        }
        #endregion

        #region 消费者方法
        protected void Consumer()
        {
            foreach (LabReport report in this.RequestQueue.GetConsumingEnumerable())
            {
                this.SaveReport(report);
            }
        }
        #endregion

        #region 保存报告
        private void SaveReport(LabReport lr)
        {
            MReport mr = new MReport();
            this.ConvertReport(lr, mr);

            //
            this.MongoService.UpdateAndInsertReport(mr);
        }
        private void ConvertReport(LabReport lr, MReport mr)
        {
            mr.ReportID = lr.Info.ReportID;
            mr.ActiveFlag = 1;
            this.ConvertInfo(lr.Info, mr.Info);
            this.ConvertItems(lr.ItemList, mr.ItemCollection);
            this.ConvertCustoms(lr.CustomList, mr.CustomCollection);
            this.ConvertImages(lr.ImageList, mr.ImageMap);
        }
        private void ConvertInfo(InfoElement i1, Info i2)
        {
            i2.AgeStr = i1.AgeStr;
            i2.BedNo = i1.BedNo;
            i2.Checker = i1.Checker;
            i2.CheckerUrl = i1.CheckerUrl;
            i2.CheckTime = i1.CheckTime;
            i2.CID = i1.CID;
            i2.ClinicalDiagnosis = i1.ClinicalDiagnosis;
            i2.ClinicName = i1.ClinicName;
            i2.CollectTime = i1.CollectTime;
            i2.Comment = i1.Comment;
            i2.DeptName = i1.DeptName;
            i2.Description = i1.Description;
            i2.Doctor = i1.Doctor;
            i2.Explanation = i1.Explanation;
            i2.FinalTime = i1.FinalTime;
            i2.GenderName = i1.GenderName;
            i2.InceptTime = i1.InceptTime;
            i2.Memo = i1.Memo;
            i2.PatientID = i1.PatientID;
            i2.PatientName = i1.PatientName;
            i2.ReceiveTime = i1.ReceiveTime;
            i2.ReportContent = i1.ReportContent;
            i2.SampleNo = i1.SampleNo;
            i2.SampleTypeName = i1.SampleTypeName;
            i2.SampleTypeNo = i1.SampleTypeNo;
            i2.SectionNo = i1.SectionNo;
            i2.SerialNo = i1.SerialNo;
            i2.Technician = i1.Technician;
            i2.TechnicianUrl = i1.TechnicianUrl;
            i2.TestTime = i1.TestTime;
            i2.VisitTimes = i1.VisitTimes;
        }

        private void ConvertItems(List<ItemElement> ls1, List<Item> ls2)
        {
            Item item = null;
            foreach (ItemElement ie in ls1)
            {
                item = new Item();
                this.ConvertItem(ie, item);
                ls2.Add(item);
            }
        }
        private void ConvertItem(ItemElement i1, Item i2)
        {
            i2.CName = i1.CName;
            i2.EName = i1.EName;
            i2.ItemNo = i1.ItemNo;
            i2.RefRange = i1.RefRange;
            i2.Result = i1.Result;
            i2.Status = i1.Status;
            i2.SuperNo = i1.SuperNo;
            i2.Unit = i1.Unit;
        }

        private void ConvertCustoms(List<CustomElement> ls1, List<Custom> ls2)
        {
            Custom cc = null;
            foreach (CustomElement ce in ls1)
            {
                cc = new Custom();
                this.ConvertCustom(ce, cc);
                ls2.Add(cc);
            }
        }
        private void ConvertCustom(CustomElement c1, Custom c2)
        {
            c2.C0 = c1.C0;
            c2.C1 = c1.C1;
            c2.C2 = c1.C2;
            c2.C3 = c1.C3;
            c2.C4 = c1.C4;
            c2.C5 = c1.C5;
            c2.C6 = c1.C6;
            c2.C7 = c1.C7;
            c2.C8 = c1.C8;
            c2.C9 = c1.C9;
            c2.C10 = c1.C10;
            c2.C11 = c1.C11;
            c2.C12 = c1.C12;
            c2.C13 = c1.C13;
            c2.C14 = c1.C14;
            c2.C15 = c1.C15;
            c2.C16 = c1.C16;
            c2.C17 = c1.C17;
            c2.C18 = c1.C18;
            c2.C19 = c1.C19;
            c2.C20 = c1.C20;
            c2.C21 = c1.C21;
            c2.C22 = c1.C22;
            c2.C23 = c1.C23;
            c2.C24 = c1.C24;
            c2.C25 = c1.C25;
            c2.C26 = c1.C26;
            c2.C27 = c1.C27;
            c2.C28 = c1.C28;
            c2.C29 = c1.C29;
            c2.C30 = c1.C30;
            c2.C31 = c1.C31;
            c2.C32 = c1.C32;
            c2.C33 = c1.C33;
            c2.C34 = c1.C34;
            c2.C35 = c1.C35;
            c2.C36 = c1.C36;
            c2.C37 = c1.C37;
            c2.C38 = c1.C38;
            c2.C39 = c1.C39;
            c2.C40 = c1.C40;
            c2.C41 = c1.C41;
            c2.C42 = c1.C42;
            c2.C43 = c1.C43;
            c2.C44 = c1.C44;
            c2.C45 = c1.C45;
            c2.C46 = c1.C46;
            c2.C47 = c1.C47;
            c2.C48 = c1.C48;
            c2.C49 = c1.C49;
        }

        private void ConvertImages(List<ImageElement> imageList, Dictionary<string, string> images)
        {
            foreach (ImageElement image in imageList)
            {
                images.Add(image.Name, image.Url);
            }
        }
        #endregion

        #region
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
        #endregion
    }
}