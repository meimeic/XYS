using System;
using System.Threading.Tasks;

using log4net;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Handler
{
    public class ReportHandleService
    {
        #region 静态变量
        protected static ILog LOG = LogManager.GetLogger("LisReportHandle");
        #endregion
        
        #region 私有字段
        private ILisReportHandle m_headHandle;
        private ILisReportHandle m_tailHandle;
        #endregion

        #region 构造函数
        public ReportHandleService()
        {
            this.m_headHandle = this.m_tailHandle = new ReportFillHandle();
            this.InitHandlerChain();
        }
        #endregion

        #region 属性
        public ILisReportHandle HeadHandle
        {
 
        }
        #endregion

        #region 同步
        public void HandleReport(ReportReportElement report)
        {
            LOG.Info("报告处理服务进行处理");
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
    }
}
