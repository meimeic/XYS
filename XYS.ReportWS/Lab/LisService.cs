using System;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;

using log4net;

using XYS.Model;
using XYS.Report;
using XYS.Lis.Report;
namespace XYS.ReportWS.Lab
{
    public class LisService
    {
        #region 静态只读字段
        private static int WorkerCount;
        private static readonly ILog LOG;
        private static readonly object GlobalLock;

        private static LisService ServiceInstance;
        #endregion

        #region 实例只读字段
        private readonly LabService LabService;
        private readonly XmlSerializer Serializer;

        private readonly FRService.LabPDFSoapClient FRClient;
        private readonly MongoService.LabMongoSoapClient MongoClient;

        private readonly List<Thread> ThreadPool;
        private readonly ConcurrentQueue<LabApplyInfo> RequestQueue;
        #endregion

        #region 构造函数
        static LisService()
        {
            GlobalLock = new object();
            WorkerCount = Config.GetWorkerCount();
            LOG = LogManager.GetLogger("ReportWS");
        }
        private LisService()
        {
            this.LabService = LabService.LService;
            this.LabService.HandleCompleteEvent += this.PrintAndSave;

            this.Serializer = new XmlSerializer(typeof(LabApplyInfo));
            this.FRClient = new FRService.LabPDFSoapClient("LabPDFSoap");
            this.MongoClient = new MongoService.LabMongoSoapClient("LabMongoSoap");

            this.ThreadPool = new List<Thread>(10);
            this.RequestQueue = new ConcurrentQueue<LabApplyInfo>();
            this.InitWorker();
        }
        #endregion

        #region 静态属性
        public static LisService LService
        {
            get { return ServiceInstance; }
        }
        #endregion

        #region 公共静态方法
        public static void RegistService()
        {
            lock (GlobalLock)
            {
                if (ServiceInstance == null)
                {
                    ServiceInstance = new LisService();
                }
            }
        }
        #endregion

        #region 生产者方法
        public void Handle(string applyInfo)
        {
            LOG.Info("获取的xml报文为:" + applyInfo);
            if (!string.IsNullOrEmpty(applyInfo))
            {
                StringReader reader = new StringReader(applyInfo);
                try
                {
                    LabApplyInfo info = (LabApplyInfo)this.Serializer.Deserialize(reader);
                    LOG.Info("处理xml报文成功");
                    this.RequestQueue.Enqueue(info);
                }
                catch (Exception ex)
                {
                    LOG.Error("处理xml报文异常:" + ex.Message);
                }
            }
            else
            {
                LOG.Warn("报文为空");
            }
        }
        #endregion

        #region 消费者方法
        protected void Consumer()
        {
            while (true)
            {
                LabApplyInfo info = null;
                while (RequestQueue.TryDequeue(out info))
                {
                    LOG.Info("开始处理报告,报告ID为:");
                    TimeSpan start = new TimeSpan(DateTime.Now.Ticks);
                    this.ApplyHandle(info);
                    TimeSpan end = new TimeSpan(DateTime.Now.Ticks);
                    LOG.Info("申请单处理时间为" + end.Subtract(start).TotalMilliseconds.ToString() + "ms");
                }
                Thread.Sleep(1000);
            }
        }
        #endregion

        #region 私有方法
        private void InitWorker()
        {
            for (int i = 0; i < WorkerCount; i++)
            {
                Thread th = new Thread(Consumer);
                th.IsBackground = true;
                th.Start();
                this.ThreadPool.Add(th);
            }
        }
        private void ApplyHandle(LabApplyInfo info)
        {
            if (info != null)
            {
                List<ApplyItem> applyCollection = info.ApplyCollection;
                if (applyCollection != null && applyCollection.Count > 0)
                {
                    foreach (ApplyItem item in applyCollection)
                    {
                        //获取审核的报告单主键
                        if (item.ApplyStatus == 7)
                        {
                            LOG.Info("申请单号为" + item.ApplyNo + "的报告加入处理队列");
                            this.ReportHandle(item.ApplyNo);
                        }
                    }
                }
            }
        }
        private void ReportHandle(string serialNo)
        {
            if (!string.IsNullOrEmpty(serialNo))
            {
                LOG.Info("根据申请单处理报告,申请号为:" + serialNo);
                string where = " where serialno='" + serialNo + "'";
                LabService.InitReport(where);
            }
        }
        private void PrintAndSave(LabReport report)
        {
            byte[] re = TransHelper.SerializeObject(report);

            //LOG.Info("获取报告成功，报告ID为:" + report.Info.ReportID + ",将报告发送到打印服务");
            //this.FRClient.PrintPDF(re);
            TimeSpan start = new TimeSpan(DateTime.Now.Ticks);
            this.MongoClient.SaveToMongo(re);
            TimeSpan end = new TimeSpan(DateTime.Now.Ticks);
            LOG.Info("将报告" + report.Info.ReportID + "发送到保存服务时间为" + end.Subtract(start).TotalMilliseconds.ToString() + "ms");
        }
        #endregion
    }
}