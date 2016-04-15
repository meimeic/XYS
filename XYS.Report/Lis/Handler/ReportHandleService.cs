using System;
using System.Threading.Tasks;

using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public class ReportHandleService
    {
        #region 构造函数
        public ReportHandleService()
        {
        }
        #endregion

        #region 同步方法
        public HandlerResult HandleReport(ReportReportElement report)
        {
            IReportHandler header = new ReportHeadHandler();
            this.InitHandlerChain(header);
            return header.ReportOption(report);
        }
        #endregion

        #region 异步方法
        public async Task<HandlerResult> HandleReportAsync(ReportReportElement report, Func<ReportReportElement, HandlerResult> callback)
        {
            HandlerResult result = await HandleReportTask(report);
            if (result.Code != -1 && callback != null)
            {
                return callback(report);
            }
            else
            {
                return result;
            }
        }
        protected Task<HandlerResult> HandleReportTask(ReportReportElement report)
        {
            return Task.Run(() =>
            {
                IReportHandler header = new ReportHeadHandler();
                this.InitHandlerChain(header);
                return header.ReportOption(report);
            });
        }
        #endregion

        #region 辅助方法
        protected void InitHandlerChain(IReportHandler head)
        {
            //lock (this.m_addHandlerLock)
            //{
            IReportHandler tailHandler = head;
            IReportHandler currentHandler = null;
            //检验项处理器
            currentHandler = new ReportItemHandler();
            AddHandler(currentHandler, tailHandler);
            //图片项处理器
            currentHandler = new ReportGraphHandler();
            AddHandler(currentHandler, tailHandler);
            //自定义项处理器
            currentHandler = new ReportCustomHandler();
            AddHandler(currentHandler, tailHandler);
            //报告处理器
            currentHandler = new ReportReportHandler();
            AddHandler(currentHandler, tailHandler);
            //}
        }
        protected void AddHandler(IReportHandler current, IReportHandler tail)
        {
            tail.Next = current;
            tail = current;
        }
        #endregion
    }
}
