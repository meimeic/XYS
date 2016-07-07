using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using log4net;

using XYS.Util;
using XYS.Report;
using XYS.Lis.Report.Model;
using XYS.Lis.Report.Persistent;
namespace XYS.Lis.Report
{
    public class ReportService
    {
        #region 静态变量
        private static ReportPKDAL PKDAL;
        private static ILog LOG = LogManager.GetLogger("LisReportHandle");
        #endregion

        #region
        static ReportService()
        {
            //初始化handleTable

            PKDAL = new ReportPKDAL();
        }
        #endregion

        #region 构造函数
        public ReportService()
        {
            this.Init();
        }
        #endregion


        #region 公共方法
        public void InitReportPK(Require req, List<ReportPK> PKList)
        {
            PKDAL.InitReportKey(req, PKList);
        }
        public void InitReportPK(string where, List<ReportPK> PKList)
        {
            PKDAL.InitReportKey(where, PKList);
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

        #region
        protected void HandleReport(ReportElement report)
        {
            if (report.ReportPK == null)
            {
                //错误
            }
            else
            {
 
            }
        }
        #endregion
    }
}
