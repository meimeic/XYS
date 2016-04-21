using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using XYS.Report.Lis.IO;
using XYS.Report.Lis.Model;
using XYS.Report.Lis.Handler;
namespace XYS.Report.Lis
{
    public delegate Task ExraOperateReport(ReportReportElement report);

    public abstract class Reporter
    {
        #region 私有字段
        private ReportHandleService m_handler;
        private ReportMongoService m_mongo;
        #endregion

        #region 私有变量
        private ExraOperateReport m_extraOperate;
        #endregion

        #region 构造函数
        protected Reporter()
        {
            this.m_handler = new ReportHandleService();
            this.m_mongo = new ReportMongoService();
            this.m_extraOperate = new ExraOperateReport(Log);
        }
        #endregion
        public ReportHandleService HandleService
        {
            get { return this.m_handler; }
        }
        public ReportMongoService MongoService
        {
            get { return this.m_mongo; }
        }
        #region 实例属性

        #endregion

        #region 同步实现
        public void OperateReport(ReportReportElement report, Action<ReportReportElement> callback = null)
        {
            this.HandleService.HandleReport(report);
            if (report.HandleResult.ResultCode > 0)
            {
                this.MongoService.InsertReport(report);
            }
            if (callback != null)
            {
                callback(report);
            }
        }
        #endregion

        #region 异步实现
        public async Task OperateReportAsync(ReportReportElement report, Action<ReportReportElement> callback = null)
        {
            await this.HandleService.HandleReportAsync(report);
            await this.MongoService.InsertReportCurrentlyAsync(report);
            if (callback != null)
            {
                callback(report);
            }
        }
        #endregion

        #region 异步辅助方法
        private Task OperateReportTask(ReportReportElement report, Action<ReportReportElement> callback = null)
        {
            return Task.Run(() =>
            {
                this.OperateReport(report, callback);
            });
        }
        protected void HandleReportComplete(ReportReportElement report)
        {
        }
        protected void SaveReportComplete(ReportReportElement report)
        {

        }
        #endregion

        #region 辅助方法
        private Task Log(ReportReportElement report)
        {
            return Task.Run(() =>
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("ReportID:");
                sb.Append(report.ReportID);
                sb.Append("    ReportStatus:");
                sb.Append(report.HandleResult.ResultCode);
                sb.Append("    Message:");
                sb.Append(report.HandleResult.Message);
                Console.WriteLine(sb.ToString());
            });
        }

        protected void SetHandlerResult(HandleResult result, int code, string message)
        {
            result.ResultCode = code;
            result.Message = message;
        }
        protected void SetHandlerResult(HandleResult result, int code, Type type, string message)
        {
            SetHandlerResult(result, code, message);
            result.HandleType = type;
        }
        #endregion
    }
}
