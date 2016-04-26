using System;
using System.Threading.Tasks;

using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public delegate void HandleReportCompletedHandler(ReportReportElement report);
    public class ReportHandleService
    {
        #region 字段
        private IReportHandle m_headHandle;
        private IReportHandle m_tailHandle;
        #endregion

        #region
        private event HandleReportCompletedHandler m_handleReportCompletedEvent;
        #endregion

        #region 构造函数
        public ReportHandleService()
        {
            this.m_headHandle = this.m_tailHandle = new ReportFillHandle();
            this.InitHandlerChain();
        }
        #endregion

        #region
        public event HandleReportCompletedHandler HandleReportCompletedEvent
        {
            add { this.m_handleReportCompletedEvent += value; }
            remove { this.m_handleReportCompletedEvent -= value; }
        }
        #endregion

        #region 同步方法
        public void HandleReport(ReportReportElement report)
        {
            this.m_headHandle.ReportOption(report);
            this.OnHandleReportCompleted(report);
        }
        #endregion

        #region 异步方法
        public async Task HandleReportAsync(ReportReportElement report)
        {
            if (report.HandleResult.ResultCode >= 0)
            {
                await HandleReportTask(report);
            }
            this.OnHandleReportCompleted(report);
        }
        protected Task HandleReportTask(ReportReportElement report)
        {
            return Task.Run(() =>
            {
                this.m_headHandle.ReportOption(report);
            });
        }
        #endregion

        #region
        protected void Handle(ReportReportElement report)
        {
            this.m_headHandle.ReportOption(report);
            this.OnHandleReportCompleted(report);
        }
        protected void OnHandleReportCompleted(ReportReportElement report)
        {
            HandleReportCompletedHandler handler = this.m_handleReportCompletedEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        #endregion


        #region 辅助方法
        private void InitHandlerChain()
        {
            IReportHandle currentHandler = null;
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
        protected void InitHandlerChain(IReportHandle head)
        {
            //lock (this.m_addHandlerLock)
            //{
            IReportHandle tailHandler = head;
            IReportHandle currentHandler = null;
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
        protected void AddHandler(IReportHandle handler)
        {
            this.m_tailHandle.Next = handler;
            this.m_tailHandle = handler;
        }
        protected void AddHandler(IReportHandle current, ref IReportHandle tail)
        {
            tail.Next = current;
            tail = current;
        }
        #endregion
    }
}
