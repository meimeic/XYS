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
    public delegate Task ExraOperateReport(ReportReportElement report, HandleResult result);

    public abstract class Reporter
    {
        #region 私有字段
        private ReportFillService m_filler;
        private ReportHandleService m_handler;
        private ReportMongoService m_mongo;
        #endregion

        #region
        private ExraOperateReport m_extraOperate;
        #endregion

        #region 构造函数
        protected Reporter()
        {
            this.m_filler = new ReportFillService();
            this.m_handler = new ReportHandleService();
            this.m_mongo = new ReportMongoService();
            this.m_extraOperate = new ExraOperateReport(Log);
        }
        #endregion

        #region 同步实现
        public void OperateReport(ReportReportElement report, HandleResult result)
        {
            if (report.LisPK == null || !report.LisPK.Configured)
            {
                this.SetHandlerResult(result, -1, this.GetType(), "the report key is null or not config!");
                return;
            }

            this.m_filler.FillReport(report, result);
            if (result.ResultCode == -1)
            {
                return;
            }

            this.m_handler.HandleReport(report, result);
            if (result.ResultCode == -1)
            {
                return;
            }

            this.m_mongo.Insert(report, result);
            return;
        }
        #endregion

        #region 异步实现
        public async Task OperateReportAsync(ReportReportElement report, HandleResult result, Action<ReportReportElement, HandleResult> callback = null)
        {
            await OperateReportTask(report, result);
            if (callback != null)
            {
                callback(report, result);
            }
        }
        #endregion

        #region 异步辅助方法
        private Task OperateReportTask(ReportReportElement report, HandleResult result)
        {
            return this.m_filler.FillReportAsync(report, result, FillReportComplete);
        }
        private Task FillReportComplete(ReportReportElement report, HandleResult result)
        {
            return this.m_handler.HandleReportAsync(report, result, HandleReportComplete);
        }
        private Task HandleReportComplete(ReportReportElement report, HandleResult result)
        {
            return this.m_mongo.InsertAsync(report, result, SaveReportComplete);
        }
        private Task SaveReportComplete(ReportReportElement report, HandleResult result)
        {
            if (this.m_extraOperate != null)
            {
                return this.m_extraOperate(report, result);
            }
            return null;
        }
        #endregion

        #region 辅助方法
        private Task Log(ReportReportElement report, HandleResult result)
        {
            return Task.Run(() =>
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("ReportID:");
                sb.Append(report.ReportID);
                sb.Append("    ReportStatus:");
                sb.Append(result.ResultCode);
                sb.Append("    Message:");
                sb.Append(result.Message);
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
        protected void SetHandlerResult(HandleResult result, int code, Type type, string message, IReportKey key)
        {
            SetHandlerResult(result, code, type, message);
            result.ReportKey = key;
        }
        #endregion
    }
}
