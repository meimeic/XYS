using System;
using System.Threading.Tasks;

using log4net;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public delegate void HandleErrorHandler(ReportReportElement report);
    public delegate void HandleSuccessHandler(ReportReportElement report);
    public class ReportHandleService
    {
        #region 静态变量
        protected static ILog LOG = LogManager.GetLogger("LisReportHandle");
        #endregion
        
        #region 私有字段
        private ILisReportHandle m_headHandle;
        private ILisReportHandle m_tailHandle;
        #endregion

        #region 私有事件
        private event HandleErrorHandler m_handleErrorEvent;
        private event HandleSuccessHandler m_handleSuccessEvent;
        #endregion

        #region 构造函数
        public ReportHandleService()
        {
            this.m_headHandle = this.m_tailHandle = new ReportFillHandle();
            this.InitHandlerChain();
        }
        #endregion

        #region 事件属性
        public event HandleErrorHandler HandleErrorEvent
        {
            add { this.m_handleErrorEvent += value; }
            remove { this.m_handleErrorEvent -= value; }
        }
        public event HandleSuccessHandler HandleSuccessEvent
        {
            add { this.m_handleSuccessEvent += value; }
            remove { this.m_handleSuccessEvent -= value; }
        }
        #endregion

        #region 实例属性
        public ILisReportHandle HeadHandle
        {
            get { return this.m_headHandle; }
        }
        #endregion

        #region 同步
        public void HandleReport(ReportReportElement report)
        {
            LOG.Info("报告处理服务进行处理");
            if (report.ReportPK != null && report.ReportPK.Configured)
            {
                this.HeadHandle.ReportOption(report);
            }
            this.OnCompleted(report);
        }
        #endregion

        #region 异步
        public async Task HandleReportAsync(ReportReportElement report)
        {
            if (report.HandleResult.ResultCode >= 0)
            {
                await HandleReportTask(report);
            }
        }
        #endregion

        #region 多线程
        public Task HandleReportTask(ReportReportElement report)
        {
            return Task.Run(() =>
            {
                this.m_headHandle.ReportOption(report);
            });
        }
        #endregion

        #region 辅助方法
        private void InitHandlerChain()
        {
            ILisReportHandle currentHandler = null;
            //检验项处理器
            currentHandler = new ReportItemHandle();
            AddHandler(currentHandler);
            //图片项处理器
            currentHandler = new ReportGraphHandle();
            AddHandler(currentHandler);
            //自定义项处理器
            currentHandler = new ReportCustomHandle();
            AddHandler(currentHandler);
            //报告处理器
            currentHandler = new ReportReportHandle();
            AddHandler(currentHandler);
        }
        protected void InitHandlerChain(ILisReportHandle head)
        {
            //lock (this.m_addHandlerLock)
            //{
            ILisReportHandle tailHandler = head;
            ILisReportHandle currentHandler = null;
            //检验项处理器
            currentHandler = new ReportItemHandle();
            AddHandler(currentHandler, ref tailHandler);
            //图片项处理器
            currentHandler = new ReportGraphHandle();
            AddHandler(currentHandler, ref tailHandler);
            //自定义项处理器
            currentHandler = new ReportCustomHandle();
            AddHandler(currentHandler, ref tailHandler);
            //报告处理器
            currentHandler = new ReportReportHandle();
            AddHandler(currentHandler, ref tailHandler);
            //}
        }
        protected void AddHandler(ILisReportHandle handler)
        {
            this.m_tailHandle.Next = handler;
            this.m_tailHandle = handler;
        }
        protected void AddHandler(ILisReportHandle current, ref ILisReportHandle tail)
        {
            tail.Next = current;
            tail = current;
        }
        #endregion

        #region 触发事件
        protected void OnCompleted(ReportReportElement report)
        {
            if (report.HandleResult.ResultCode > 0)
            {
                this.OnSuccess(report);
            }
            else
            {
                this.OnError(report);
            }
        }
        protected void OnError(ReportReportElement report)
        {
            HandleErrorHandler handler = this.m_handleErrorEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        protected void OnSuccess(ReportReportElement report)
        {
            HandleSuccessHandler handler = this.m_handleSuccessEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        #endregion
    }
}
