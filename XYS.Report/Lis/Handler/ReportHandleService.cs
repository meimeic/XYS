using System;
using System.Threading.Tasks;

using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public class ReportHandleService
    {
        #region 私有字段
        private IReportHandle m_headHandle;
        private IReportHandle m_tailHandle;
        #endregion

        #region 构造函数
        public ReportHandleService()
        {
            this.m_headHandle = this.m_tailHandle = new ReportFillHandle();
            this.InitHandlerChain();
        }
        #endregion

        #region 同步
        public void HandleReport(ReportReportElement report)
        {
            this.m_headHandle.ReportOption(report);
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
