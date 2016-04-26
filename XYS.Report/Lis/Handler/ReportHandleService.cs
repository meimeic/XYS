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
            this.m_headHandler = this.m_tailHandler = new ReportFillHandler();
            this.InitHandlerChain();
        }
        #endregion

        #region 同步方法
        public void HandleReport(ReportReportElement report)
        {
            this.m_headHandler.ReportOption(report);
        }
        #endregion

        #region 异步方法
        public async Task HandleReportAsync(ReportReportElement report, Action<ReportReportElement> callback = null)
        {
            if (report.HandleResult.ResultCode >= 0)
            {
                await HandleReportTask(report, callback);
            }
        }
        protected Task HandleReportTask(ReportReportElement report, Action<ReportReportElement> callback = null)
        {
            return Task.Run(() =>
            {
                this.m_headHandler.ReportOption(report);
                if (callback != null)
                {
                    callback(report);
                }
            });
        }
        #endregion

        #region 辅助方法
        private void InitHandlerChain()
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
            AddHandler(currentHandler, ref tailHandler);
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
            this.m_tailHandler.Next = handler;
            this.m_tailHandler = handler;
        }
        protected void AddHandler(IReportHandler current, ref IReportHandler tail)
        {
            tail.Next = current;
            tail = current;
        }
        #endregion
    }
}
