using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

using log4net;
using XYS.Report.Lis.Model;
using XYS.Report.Lis.Handler;
using XYS.Report.Lis.Persistent;
namespace XYS.Report.Lis
{
    public delegate void InitErrorHandler(ReportReportElement report);
    public delegate void InitSuccessHandler(ReportReportElement report);
    public delegate void NewErrorHandler(ReportReportElement report);
    public delegate void NewSuccessHandler(ReportReportElement report);
    public delegate void ChangeErrorHandler(ReportReportElement report);
    public delegate void ChangeSuccessHandler(ReportReportElement report);
    public class ReportService
    {
        #region 静态字段
        private static int SaveWorkerCount;
        private static int HandleWorkerCount;
        private static readonly ILog LOG;
        private static readonly int QueueCapacity;
        private static readonly ReportService ServiceInstance;
        #endregion

        #region 私有字段
        private Thread[] m_handleWorkerPool;
        private Thread[] m_saveWorkerPool;
        private ReportPKService m_PKService;
        private ReportHandleService m_handleService;
        private ReportMongoService m_mongoService;

        private readonly BlockingCollection<ReportReportElement> InitRequestQueue;
        private readonly BlockingCollection<ReportReportElement> SaveRequestQueue;
        #endregion

        #region 事件字段
        private event InitErrorHandler m_initErrorEvent;
        private event InitSuccessHandler m_initSuccessEvent;
        private event NewErrorHandler m_newErrorEvent;
        private event NewSuccessHandler m_newSuccessEvent;
        private event ChangeErrorHandler m_changeErrorEvent;
        private event ChangeSuccessHandler m_changeSuccessEvent;
        #endregion

        #region 构造函数
        static ReportService()
        {
            QueueCapacity = 10000;
            SaveWorkerCount = 10;
            HandleWorkerCount = 10;
            ServiceInstance = new ReportService();
            LOG = LogManager.GetLogger("LisReport");
        }
        private ReportService()
        {
            this.InitRequestQueue = new BlockingCollection<ReportReportElement>(QueueCapacity);
            this.SaveRequestQueue = new BlockingCollection<ReportReportElement>(QueueCapacity);

            this.Init();
        }
        #endregion

        #region 静态属性
        public static ReportService LisService
        {
            get { return ServiceInstance; }
        }
        #endregion

        #region 事件属性
        public event InitErrorHandler InitErrorEvent
        {
            add { this.m_initErrorEvent += value; }
            remove { this.m_initErrorEvent -= value; }
        }
        public event InitSuccessHandler InitSuccessEvent
        {
            add { this.m_initSuccessEvent += value; }
            remove { this.m_initSuccessEvent -= value; }
        }
        public event NewErrorHandler NewErrorEvent
        {
            add { this.m_newErrorEvent += value; }
            remove { this.m_newErrorEvent -= value; }
        }
        public event NewSuccessHandler NewSuccessEvent
        {
            add { this.m_newSuccessEvent += value; }
            remove { this.m_newSuccessEvent -= value; }
        }
        public event ChangeErrorHandler ChangeErrorEvent
        {
            add { this.m_changeErrorEvent += value; }
            remove { this.m_changeErrorEvent -= value; }
        }
        public event ChangeSuccessHandler ChangeSuccessEvent
        {
            add { this.m_changeSuccessEvent += value; }
            remove { this.m_changeSuccessEvent -= value; }
        }
        #endregion

        #region 实例属性
        protected ReportPKService PKService
        {
            get { return this.m_PKService; }
        }
        protected ReportHandleService HandleService
        {
            get { return this.m_handleService; }
        }
        protected ReportMongoService MongoService
        {
            get { return this.m_mongoService; }
        }
        #endregion

        #region 主键值获取
        public void InitReportPK(Require req, List<ReportPK> PKList)
        {
            this.PKService.InitReportPK(req, PKList);
        }
        public void InitReportPK(string where, List<ReportPK> PKList)
        {
            this.PKService.InitReportPK(where, PKList);
        }
        #endregion

        #region 生产者方法
        public void InitReport(ReportReportElement report)
        {
            LOG.Info("将处理报告请求加入到处理请求队列");
            this.InitRequestQueue.Add(report);
        }
        public void SaveReport(ReportReportElement report)
        {
            LOG.Info("将保存报告请求加入到保存请求队列");
            this.SaveRequestQueue.Add(report);
        }
        #endregion

        #region 方法
        public void InitAndSave(ReportReportElement report)
        {
            this.HandleService.HandleReport(report);
            if (report.HandleResult.ResultCode > 0)
            {
                this.MongoService.InsertReport(report, SaveOptions.UpdateAndSave);
            }
        }
        #endregion

        #region 内部处理
        protected void InitError(ReportReportElement report)
        {
            //内部处理
            LOG.Info("进入报告处理错误模块");

            //事件转播
            this.OnInitError(report);
            LOG.Info("退出报告处理错误模块");
        }
        protected void InitSuccess(ReportReportElement report)
        {
            //内部处理
            LOG.Info("进入处理报告成功模块");
            this.SetHandlerResult(report.HandleResult, 100);
            this.SaveReport(report);
            //事件转播
            this.OnInitSuccess(report);
            LOG.Info("退出处理报告成功模块");
        }
        
        protected void NewError(ReportReportElement report)
        {
            //内部处理
            LOG.Info("进入报告保存错误模块");
            //事件转播
            this.OnNewError(report);
            LOG.Info("退出报告保存错误模块");
        }
        protected void NewSuccess(ReportReportElement report)
        {
            //内部处理
            LOG.Info("进入报告保存成功模块");

            //事件转播
            this.OnNewSuccess(report);
            LOG.Info("退出报告保存成功模块");
        }
        
        protected void ChangeError(ReportReportElement report)
        {
            //内部处理
            LOG.Info("进入历史报告修改错误模块");
            //事件转播
            this.OnChangeError(report);
            LOG.Info("退出历史报告修改错误模块");
        }
        protected void ChangeSuccess(ReportReportElement report)
        {
            //内部处理
            LOG.Info("进入历史报告成功模块");
            //事件转播
            this.OnChangeSuccess(report);
            LOG.Info("退出历史报告成功模块");
        }
        protected void SetHandlerResult(HandleResult result, int code, string message = null, Type type = null, Exception ex = null)
        {
            result.ResultCode = code;
            result.Message = message;
            result.HandleType = type;
            result.Exception = ex;
        }
        #endregion

        #region 触发事件
        protected void OnInitError(ReportReportElement report)
        {
            InitErrorHandler handler = this.m_initErrorEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        protected void OnInitSuccess(ReportReportElement report)
        {
            InitSuccessHandler handler = this.m_initSuccessEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        protected void OnNewError(ReportReportElement report)
        {
            NewErrorHandler handler = this.m_newErrorEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        protected void OnNewSuccess(ReportReportElement report)
        {
            NewSuccessHandler handler = this.m_newSuccessEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        protected void OnChangeError(ReportReportElement report)
        {
            ChangeErrorHandler handler = this.m_changeErrorEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        protected void OnChangeSuccess(ReportReportElement report)
        {
            ChangeSuccessHandler handler = this.m_changeSuccessEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        #endregion

        #region 私有方法
        private void Init()
        {
            this.m_PKService = new ReportPKService();
            this.m_handleService = new ReportHandleService();
            this.HandleService.HandleErrorEvent += this.InitError;
            this.HandleService.HandleCompleteEvent += this.InitSuccess;

            this.m_mongoService = new ReportMongoService();
            this.MongoService.InsertErrorEvent += this.NewError;
            this.MongoService.InsertSuccessEvent += this.NewSuccess;
            this.MongoService.UpdateErrorEvent += this.ChangeError;
            this.MongoService.UpdateSuccessEvent += this.ChangeSuccess;

            //初始化线程池
            this.InitWorkerPool();
        }
        private void InitWorkerPool()
        {
            Thread th = null;
            this.m_handleWorkerPool = new Thread[HandleWorkerCount];
            for (int i = 0; i < HandleWorkerCount; i++)
            {
                th = new Thread(InitRequestConsumer);
                this.m_handleWorkerPool[i] = th;
                th.Start();
            }

            this.m_saveWorkerPool = new Thread[SaveWorkerCount];
            for (int i = 0; i < SaveWorkerCount; i++)
            {
                th = new Thread(SaveRequestConsumer);
                this.m_saveWorkerPool[i] = th;
                th.Start();
            }
        }
        #endregion

        #region 消费者方法
        protected void InitRequestConsumer()
        {
            foreach (ReportReportElement report in this.InitRequestQueue.GetConsumingEnumerable())
            {
                LOG.Info("报告处理线程开始处理报告,报告ID为:"+report.ReportPK.ID);
                this.HandleService.HandleReport(report);
            }
        }
        protected void SaveRequestConsumer()
        {
            foreach (ReportReportElement report in this.SaveRequestQueue.GetConsumingEnumerable())
            {
                LOG.Info("报告保存线程开始保存报告,报告ID为:" + report.ReportID);
                this.MongoService.InsertReport(report, SaveOptions.UpdateAndSave);
            }
        }
        #endregion
    }
}
