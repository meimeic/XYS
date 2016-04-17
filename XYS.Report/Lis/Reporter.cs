using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using XYS.Report.Lis.IO;
using XYS.Report.Lis.Model;
using XYS.Report.Lis.Handler;
namespace XYS.Report.Lis
{
    public abstract class Reporter : IReporter
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
        public void OperateReport(ReportReportElement report, HandlerResult result)
        {
            if (report.LisPK == null || !report.LisPK.Configured)
            {
                this.SetHandlerResult(result, -1, this.GetType(), "the report key is null or not config!");
                return;
            }

            this.m_filler.FillReport(report, result);
            if (result.Code == -1)
            {
                return;
            }

            this.m_handler.HandleReport(report, result);
            if (result.Code == -1)
            {
                return;
            }

            this.m_mongo.Insert(report, result);
            return;
        }
        #endregion

        #region 异步实现
        public async Task OperateReportAsync(ReportReportElement report, HandlerResult result, Action<ReportReportElement, HandlerResult> callback = null)
        {
            await OperateReportTask(report, result);
            if (callback != null)
            {
                callback(report, result);
            }
            return;
        }
        #endregion

        #region 
        private Task OperateReportTask(ReportReportElement report, HandlerResult result)
        {
            return this.FillReportComplete(report, result);
        }
        private Task FillReportComplete(ReportReportElement report, HandlerResult result)
        {
            return this.m_handler.HandleReportAsync(report, result, HandleReportComplete);
        }
        private Task HandleReportComplete(ReportReportElement report, HandlerResult result)
        {
            return this.m_mongo.InsertAsync(report, result, SaveReportComplete);
        }
        private Task SaveReportComplete(ReportReportElement report, HandlerResult result)
        {
            if (this.m_extraOperate != null)
            {
                return this.m_extraOperate(report, result);
            }
            return null;
        }
        #endregion

        #region 辅助方法
        private Task Log(ReportReportElement report, HandlerResult result)
        {
            return Task.Run(() =>
            {
                Console.WriteLine(report.ReportID + ";"+result.Code);
            });
        }

        protected void SetHandlerResult(HandlerResult result, int code, string message)
        {
            result.Code = code;
            result.Message = message;
        }
        protected void SetHandlerResult(HandlerResult result, int code, Type type, string message)
        {
            SetHandlerResult(result, code, message);
            result.FinalType = type;
        }
        protected void SetHandlerResult(HandlerResult result, int code, Type type, string message, IReportKey key)
        {
            SetHandlerResult(result, code, type, message);
            result.ReportKey = key;
        }
        #endregion
    }
}
