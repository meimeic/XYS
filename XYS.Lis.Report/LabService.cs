using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

using log4net;

using XYS.Util;
using XYS.Report;
using XYS.Model.Lab;

using XYS.Lis.Report.Handler;
using XYS.Lis.Report.Persistent;
namespace XYS.Lis.Report
{
    public delegate void HandleErrorHandler(LabReport report);
    public delegate void HandleSuccessHandler(LabReport report);
    public class LabService
    {
        #region 静态变量
        private static ILog LOG;
        private static int WorkerCount;
        private static readonly LabService ServiceInstance;

        private readonly IHandle InfoHandler;
        private readonly IHandle ItemHandler;
        private readonly IHandle ImageHandler;

        private Thread[] m_workerPool;
        private readonly LabPKDAL PKDAL;
        private readonly LabReportDAL ReportDAL;
        private readonly BlockingCollection<LabReport> InitRequestQueue;
        #endregion

        #region 私有事件
        private event HandleErrorHandler m_handleErrorEvent;
        private event HandleSuccessHandler m_handleCompleteEvent;
        #endregion

        #region 构造函数
        static LabService()
        {
            WorkerCount = 2;
            LOG = LogManager.GetLogger("LisReportHandle");

            ServiceInstance = new LabService();
        }
        private LabService()
        {
            this.PKDAL = new LabPKDAL();
            this.ReportDAL = new LabReportDAL();

            this.InfoHandler = new InfoHandle(this.ReportDAL);
            this.ItemHandler = new ItemHandle(this.ReportDAL);
            this.ImageHandler = new ImageHandle(this.ReportDAL);

            this.InitRequestQueue = new BlockingCollection<LabReport>(1000);
            this.Init();
        }
        #endregion

        #region 静态属性
        public static LabService LService
        {
            get { return ServiceInstance; }
        }
        #endregion
        
        #region 事件属性
        public event HandleErrorHandler HandleErrorEvent
        {
            add { this.m_handleErrorEvent += value; }
            remove { this.m_handleErrorEvent -= value; }
        }
        public event HandleSuccessHandler HandleCompleteEvent
        {
            add { this.m_handleCompleteEvent += value; }
            remove { this.m_handleCompleteEvent -= value; }
        }
        #endregion

        #region 公共方法
        public void InitReport(Require req)
        {
            List<LabPK> PKList = new List<LabPK>(100);
            this.PKDAL.InitKey(req, PKList);
            this.HandleReport(PKList);
        }
        public void InitReport(string where)
        {
            List<LabPK> PKList = new List<LabPK>(100);
            this.PKDAL.InitKey(where, PKList);
            this.HandleReport(PKList);
        }
        #endregion

        #region 生产者方法
        private void HandleReport(List<LabPK> PKList)
        {
            LabReport report = null;
            foreach (LabPK pk in PKList)
            {
                report = new LabReport();
                report.ReportPK = pk;
                this.HandleReport(report);
            }
        }
        private void HandleReport(LabReport report)
        {
            LOG.Info("将处理报告请求加入到处理请求队列");
            this.InitRequestQueue.Add(report);
        }
        #endregion

        #region 消费者方法
        protected void InitRequestConsumer()
        {
            foreach (LabReport report in this.InitRequestQueue.GetConsumingEnumerable())
            {
                LOG.Info("报告处理线程开始处理报告,报告ID为:" + report.ReportPK.ID);
                this.InnerHandle(report);
            }
        }
        #endregion

        #region 触发事件
        protected void OnError(LabReport report)
        {
            HandleErrorHandler handler = this.m_handleErrorEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        protected void OnSuccess(LabReport report)
        {
            HandleSuccessHandler handler = this.m_handleCompleteEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        #endregion

        #region 私有方法
        private void Init()
        {
            this.InitWorkerPool();
        }
        private void InitWorkerPool()
        {
            Thread th = null;
            this.m_workerPool = new Thread[WorkerCount];
            for (int i = 0; i < WorkerCount; i++)
            {
                th = new Thread(InitRequestConsumer);
                this.m_workerPool[i] = th;
                th.Start();
            }
        }
        private void InnerHandle(LabReport report)
        {
            IReportKey RK = report.ReportPK;
            ReportElement RE = new ReportElement();
            bool result = this.InnerHandle(RE, RK);
            this.InnerConvert(report, RE);
            if (result)
            {
                this.OnSuccess(report);
            }
            else
            {
                this.OnError(report);
            }
        }
        private bool InnerHandle(ReportElement report, IReportKey RK)
        {
            bool result = false;
            //处理info
            result = this.InfoHandler.HandleElement(report.Info, RK);
            if (!result)
            {
                //处理item
                result = this.ItemHandler.HandleElement(report.ItemList, RK, typeof(ItemElement));
                if (!result)
                {
                    //处理image
                    result = this.ImageHandler.HandleElement(report.ImageList, RK, typeof(GraphElement));
                }
            }
            return result;
        }
        private void InnerConvert(LabReport lr, ReportElement re)
        {
            ItemElement item = null;
            ImageElement image = null;

            lr.Info = re.Info as InfoElement;
            foreach (IFillElement element in re.ItemList)
            {
                item = element as ItemElement;
                if (item != null)
                {
                    lr.ItemList.Add(item);
                }
            }
            foreach (IFillElement element in re.ImageList)
            {
                image = element as ImageElement;
                if (image != null)
                {
                    lr.ImageList.Add(image);
                }
            }
        }
        #endregion
    }
}