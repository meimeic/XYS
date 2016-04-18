using System;
using System.Threading.Tasks;

using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public class ReportHandleService
    {
        #region 字段
        private IReportHandler m_headHandler;
        private IReportHandler m_tailHandler;
        #endregion

        #region 构造函数
        public ReportHandleService()
        {
        }
        #endregion

        #region 实例属性
        public IReportHandler HeadHandler
        {
            get
            {
                if (this.m_headHandler == null)
                {
                    this.InitHandlerChain();
                }
                return this.m_headHandler;
            }
        }
        #endregion

        #region 同步方法
        public void HandleReport(ReportReportElement report, HandlerResult result)
        {
            this.HeadHandler.ReportOption(report, result);
        }
        #endregion

        #region 异步方法
        public async Task HandleReportAsync(ReportReportElement report, HandlerResult result, Func<ReportReportElement, HandlerResult, Task> callback = null)
        {
            if (result.Code != -1)
            {
                await HandleReportTask(report, result);
            }
            if (callback != null)
            {
                await callback(report, result);
            }
        }
        protected Task HandleReportTask(ReportReportElement report, HandlerResult result)
        {
            return Task.Run(() =>
            {
                IReportHandler header = new ReportHeadHandler();
                this.InitHandlerChain(header);
                header.ReportOption(report, result);
            });
        }
        #endregion

        #region 辅助方法
        protected void InitHandlerChain()
        {
            IReportHandler currentHandler = null;
            //检验项处理器
            currentHandler = new ReportItemHandler();
            AddHandler(currentHandler);
            //图片项处理器
            currentHandler = new ReportGraphHandler();
            AddHandler(currentHandler);
            //自定义项处理器
            currentHandler = new ReportCustomHandler();
            AddHandler(currentHandler);
            //报告处理器
            currentHandler = new ReportReportHandler();
            AddHandler(currentHandler);
        }
        protected void InitHandlerChain(IReportHandler head)
        {
            //lock (this.m_addHandlerLock)
            //{
            IReportHandler tailHandler = head;
            IReportHandler currentHandler = null;
            //检验项处理器
            currentHandler = new ReportItemHandler();
            AddHandler(currentHandler,ref tailHandler);
            //图片项处理器
            currentHandler = new ReportGraphHandler();
            AddHandler(currentHandler, ref tailHandler);
            //自定义项处理器
            currentHandler = new ReportCustomHandler();
            AddHandler(currentHandler, ref tailHandler);
            //报告处理器
            currentHandler = new ReportReportHandler();
            AddHandler(currentHandler, ref tailHandler);
            //}
        }
        protected void AddHandler(IReportHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("filter param must not be null");
            }
            if (this.m_headHandler == null)
            {
                this.m_headHandler = this.m_tailHandler = handler;
            }
            else
            {
                this.m_tailHandler.Next = handler;
                this.m_tailHandler = handler;
            }
        }
        protected void AddHandler(IReportHandler current,ref IReportHandler tail)
        {
            tail.Next = current;
            tail = current;
        }
        #endregion
    }
}
