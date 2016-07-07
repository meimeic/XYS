using System;
using System.Threading.Tasks;

using log4net;
using XYS.Report.Lis.Model;
namespace XYS.Lis
{
    public class ReportHandleService
    {
        #region 静态变量
        private static ILog LOG = LogManager.GetLogger("LisReportHandle");
        #endregion

        #region 私有字段
        private ReportFillHandle m_fillHandle;
        private ReportInfoHandle m_infoHandle;
        private ReportItemHandle m_itemHandle;
        private ReportCustomHandle m_customHandle;
        private ReportGraphHandle m_graphHandle;
        private ReportReportHandle m_reportHandle;
        #endregion

        #region 私有事件
        private event HandleErrorHandler m_handleErrorEvent;
        private event HandleSuccessHandler m_handleCompleteEvent;
        #endregion

        #region 构造函数
        public ReportHandleService()
        {
            this.Init();
        }
        #endregion

        #region 事件属性
        internal event HandleErrorHandler HandleErrorEvent
        {
            add { this.m_handleErrorEvent += value; }
            remove { this.m_handleErrorEvent -= value; }
        }
        internal event HandleSuccessHandler HandleCompleteEvent
        {
            add { this.m_handleCompleteEvent += value; }
            remove { this.m_handleCompleteEvent -= value; }
        }
        #endregion

        #region 实例属性
        protected ReportFillHandle FillHandle
        {
            get { return this.m_fillHandle; }
        }
        protected ReportInfoHandle InfoHandle
        {
            get { return this.m_infoHandle; }
        }
        protected ReportItemHandle ItemHandle
        {
            get { return this.m_itemHandle; }
        }
        protected ReportCustomHandle CustomHandle
        {
            get { return this.m_customHandle; }
        }
        protected ReportGraphHandle GraphHandle
        {
            get { return this.m_graphHandle; }
        }
        protected ReportReportHandle ReportHandle
        {
            get { return this.m_reportHandle; }
        }
        #endregion

        #region 公共方法
        public void HandleReport(ReportReportElement report)
        {
            LOG.Info("进入报告处理流程");
            if (report.ReportPK != null && report.ReportPK.Configured)
            {
                this.SetHandlerResult(report.HandleResult, 1);
                this.FillHandle.ReportHandle(report);
            }
            else
            {
                this.SetHandlerResult(report.HandleResult, -1, "当前报告不存在主键异常", this.GetType(), new ArgumentNullException("错误，当前报告不存在主键！"));
                LogError(report);
                this.OnError(report);
            }
            LOG.Info("退出报告处理流程");
        }
        #endregion

        #region 辅助方法
        protected void FillError(ReportReportElement report)
        {
            //内部处理
            LogError(report);
            //转播事件
            this.OnError(report);
        }
        protected void FillSuccess(ReportReportElement report)
        {
            //内部处理
            LOG.Info("填充报告处理成功");
            this.GraphHandle.ReportHandle(report);
        }
        protected void GraphError(ReportReportElement report)
        {
            //内部处理
            LogError(report);
            //转播事件
            this.OnError(report);
        }
        protected void GraphSuccess(ReportReportElement report)
        {
            //内部处理
            LOG.Info("报告图片项处理成功,开始reportinfo、reportitem、reportcustom的并发处理");
            Task infoTask = Task.Run(() => { this.InfoHandle.ReportHandle(report); });
            Task itemTask = Task.Run(() => { this.ItemHandle.ReportHandle(report); });
            Task customTask = Task.Run(() => { this.CustomHandle.ReportHandle(report); });
            try
            {
                Task.WaitAll(infoTask, itemTask, customTask);
                LOG.Info("reportinfo、reportitem、reportcustom的并发处理全部成功");
                this.ReportHandle.ReportHandle(report);
            }
            catch (Exception ex)
            {
                this.SetHandlerResult(report.HandleResult, -1, "reportinfo、reportitem、reportcustom的并发处理异常", this.GetType(), ex);
                LogError(report);
                //触发事件
                this.OnError(report);
            }
        }
        protected void ReportSuccess(ReportReportElement report)
        {
            //内部处理
            LOG.Info("报告整体处理成功");
            //转播事件
            this.OnComplete(report);
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
        protected void OnError(ReportReportElement report)
        {
            HandleErrorHandler handler = this.m_handleErrorEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        protected void OnComplete(ReportReportElement report)
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
            this.m_infoHandle = new ReportInfoHandle();
            this.m_itemHandle = new ReportItemHandle();
            this.m_customHandle = new ReportCustomHandle();

            this.m_fillHandle = new ReportFillHandle();
            this.FillHandle.HandleErrorEvent += this.FillError;
            this.FillHandle.HandleSuccessEvent += this.FillSuccess;

            this.m_graphHandle = new ReportGraphHandle();
            this.GraphHandle.HandleErrorEvent += this.GraphError;
            this.GraphHandle.HandleSuccessEvent += this.GraphSuccess;

            this.m_reportHandle = new ReportReportHandle();
            this.ReportHandle.HandleSuccessEvent += this.ReportSuccess;
        }
        private void LogError(ReportReportElement report)
        {
            LOG.Error(report.HandleResult.Message, report.HandleResult.Exception);
        }
        #endregion
    }
}
