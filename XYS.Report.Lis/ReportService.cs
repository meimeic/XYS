using System;
using System.Collections.Generic;

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
        private static readonly ILog LOG;
        private static readonly ReportService ServiceInstance;
        #endregion

        #region 私有字段
        private ReportPKService m_PKService;
        private ReportHandleService m_handleService;
        private ReportMongoService m_mongoService;
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
            ServiceInstance = new ReportService();
            LOG = LogManager.GetLogger("LisReport");
        }
        private ReportService()
        {
            this.Init();
        }
        #endregion

        #region 静态属性
        public static ReportService LisService
        {
            get { return ServiceInstance; }
        }
        #endregion

        #region
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

        #region 报告主键值获取
        public void InitReportPK(Require req, List<ReportPK> PKList)
        {
            this.PKService.InitReportPK(req, PKList);
        }
        public void InitReportPK(string where, List<ReportPK> PKList)
        {
            this.PKService.InitReportPK(where, PKList);
        }
        #endregion

        #region 报告处理
        public void InitReport(ReportReportElement report)
        {
            this.HandleService.HandleReport(report);
        }
        public void SaveReport(ReportReportElement report)
        {
            this.MongoService.InsertReport(report);
        }
        public void UpdateAndSaveReport(ReportReportElement report)
        {
            this.MongoService.InsertReportCurrently(report);
        }
        public void InitThenSave(ReportReportElement report)
        {
            this.InitReport(report);
            if (report.HandleResult.ResultCode > 0)
            {
                this.SaveReport(report);
            }
        }
        public void InitThenUpdateAndSave(ReportReportElement report)
        {
            this.InitReport(report);
            if (report.HandleResult.ResultCode > 0)
            {
                this.UpdateAndSaveReport(report);
            }
        }
        #endregion

        #region 内部处理
        protected void InitError(ReportReportElement report)
        {
            //内部处理

            //事件转播
            this.OnInitError(report);
        }
        protected void InitSuccess(ReportReportElement report)
        {
            //内部处理
            this.SetHandlerResult(report.HandleResult, 100);

            //事件转播
            this.OnInitSuccess(report);
        }
        protected void NewError(ReportReportElement report)
        {
            //内部处理

            //事件转播
            this.OnNewError(report);
        }
        protected void NewSuccess(ReportReportElement report)
        {
            //内部处理

            //事件转播
            this.OnNewSuccess(report);
        }
        protected void ChangeError(ReportReportElement report)
        {
            //内部处理

            //事件转播
            this.OnChangeError(report);
        }
        protected void ChangeSuccess(ReportReportElement report)
        {
            //内部处理

            //事件转播
            this.OnChangeSuccess(report);
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
        }
        #endregion

    }
}
