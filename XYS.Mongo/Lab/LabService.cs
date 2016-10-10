using System;
using System.IO;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;

using log4net;

using XYS.Model;
using XYS.Model.Lab;
using XYS.Mongo.Lab.Model;
using XYS.Mongo.Util;
namespace XYS.Mongo.Lab
{
    public class LabService
    {
        static readonly ILog LOG;
        private static LabService ServiceInstance;
        private static readonly object GlobalLock;

        private readonly LabMongo MongoService;

        #region 构造函数
        static LabService()
        {
            GlobalLock = new object();
            LOG = LogManager.GetLogger("LabMongo");
        }
        private LabService()
        {
            this.MongoService = LabMongo.MongoService;
        }
        #endregion

        #region 静态属性
        public static LabService LService
        {
            get { return ServiceInstance; }
        }
        #endregion

        #region 静态公共方法
        public static void RegistService()
        {
            lock (GlobalLock)
            {
                if (ServiceInstance == null)
                {
                    ServiceInstance = new LabService();
                }
            }
        }
        #endregion

        #region 生产者方法
        public void Save2Mongo(LabReport lr)
        {
            TimeSpan start = new TimeSpan(DateTime.Now.Ticks);
            MReport mr = new MReport();
            this.ConvertReport(lr, mr);
            this.MongoService.UpdateAndInsertReport(mr);
            TimeSpan end = new TimeSpan(DateTime.Now.Ticks);
            LOG.Info("处理保存报告" + mr.ID + "时间为" + end.Subtract(start).TotalMilliseconds.ToString() + "ms");
        }
        #endregion

        #region 保存报告
        private void ConvertReport(LabReport lr, MReport mr)
        {
            mr.ActiveFlag = 1;
            this.ConvertInfo(lr.Info, mr);
            this.ConvertItems(lr.ItemList, mr.ItemCollection);
            this.ConvertCustoms(lr.CustomList, mr.CustomCollection);
            this.ConvertImages(lr.ImageList, mr.ImageMap);
        }
        private void ConvertInfo(InfoElement info, MReport mr)
        {
            mr.ReportID = info.ReportID;
            mr.AgeStr = info.AgeStr;
            mr.BedNo = info.BedNo;
            mr.Checker = info.Checker;
            mr.CheckerUrl = info.CheckerUrl;
            mr.CheckTime = info.CheckTime;
            mr.CID = info.CID;
            mr.ClinicalDiagnosis = info.ClinicalDiagnosis;
            mr.ClinicName = info.ClinicName;
            mr.CollectTime = info.CollectTime;
            mr.Comment = info.Comment;
            mr.DeptName = info.DeptName;
            mr.Description = info.Description;
            mr.Doctor = info.Doctor;
            mr.Explanation = info.Explanation;
            mr.FinalTime = info.FinalTime;
            mr.GenderName = info.GenderName;
            mr.InceptTime = info.InceptTime;
            mr.Memo = info.Memo;
            mr.PatientID = info.PatientID;
            mr.PatientName = info.PatientName;
            mr.ReceiveTime = info.ReceiveTime;
            mr.ReportContent = info.ReportContent;
            mr.SampleNo = info.SampleNo;
            mr.SampleTypeName = info.SampleTypeName;
            mr.SampleTypeNo = info.SampleTypeNo;
            mr.SectionNo = info.SectionNo;
            mr.SerialNo = info.SerialNo;
            mr.Technician = info.Technician;
            mr.TechnicianUrl = info.TechnicianUrl;
            mr.TestTime = info.TestTime;
            mr.VisitTimes = info.VisitTimes;
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
        }

        private void ConvertImages(List<ImageElement> imageList, Dictionary<string, string> images)
        {
            foreach (ImageElement image in imageList)
            {
                images.Add(image.Name, image.Url);
            }
        }
        #endregion
    }
}