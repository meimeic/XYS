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
        private ReportFillHandle m_fillHandle;
        private ReportInfoHandle m_infoHandle;
        private ReportItemHandle m_itemHandle;
        private ReportCustomHandle m_customHandle;
        private ReportGraphHandle m_graphHandle;
        private ReportReportHandle m_reportHandle;
        #endregion

        #region 私有事件
        private event HandleErrorHandler m_handleErrorEvent;
        private event HandleSuccessHandler m_handleSuccessEvent;
        #endregion

        #region 构造函数
        public ReportHandleService()
        {
            this.Init();
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

        #region 处理
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
                this.SetHandlerResult(report.HandleResult, -1, this.GetType(), new ArgumentNullException("错误，当前报告不存在主键！"));
                this.OnError(report);
            }
            LOG.Info("退出报告处理流程");
        }
        #endregion

        #region 辅助方法
        protected void FillSuccess(ReportReportElement report)
        {
            //内部处理
            LOG.Info("报告填充成功");
            this.GraphHandle.ReportHandle(report);
        }
        protected void FillError(ReportReportElement report)
        {
            //内部处理

            //转播事件
            this.OnError(report);
        }
        protected void GraphError(ReportReportElement report)
        {
            //内部处理

            //转播事件
            this.OnError(report);
        }
        protected void GraphSuccess(ReportReportElement report)
        {
            Task infoTask = Task.Run(() => { this.InfoHandle.ReportHandle(report); });
            Task itemTask = Task.Run(() => { this.ItemHandle.ReportHandle(report); });
            Task customTask = Task.Run(() => { this.CustomHandle.ReportHandle(report); });
            try
            {
                Task.WaitAll(infoTask, itemTask, customTask);
                this.ReportHandle.ReportHandle(report);
            }
            catch (Exception ex)
            {
                this.SetHandlerResult(report.HandleResult, -1, this.GetType(), ex);
                this.OnError(report);
            }
        }
        protected void ReportSuccess(ReportReportElement report)
        {
            //内部处理
            
            //触发处理成功事件
            this.OnSuccess(report);
        }
        protected void SetHandlerResult(HandleResult result, int code, Type type = null, Exception ex = null)
        {
            result.ResultCode = code;
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
        protected void OnSuccess(ReportReportElement report)
        {
            HandleSuccessHandler handler = this.m_handleSuccessEvent;
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
            this.FillHandle.HandleReportErrorEvent += this.FillError;
            this.FillHandle.HandleReportSuccessEvent += this.FillSuccess;

            this.m_graphHandle = new ReportGraphHandle();
            this.GraphHandle.HandleReportErrorEvent += this.GraphError;
            this.GraphHandle.HandleReportSuccessEvent += this.GraphSuccess;

            this.m_reportHandle = new ReportReportHandle();
            this.ReportHandle.HandleReportSuccessEvent += this.ReportSuccess;
        }
        #endregion
    }
}
