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
        public void OperateReport(ReportReportElement report)
        {
            if (report.LisPK == null || !report.LisPK.Configured)
            {
                this.SetHandlerResult(report.HandleResult, -1, this.GetType(), "the report key is null or not config!");
                return;
            }

            this.m_filler.FillReport(report);
            if (report.HandleResult.ResultCode == -1)
            {
                return;
            }

            this.m_handler.HandleReport(report);
            if (report.HandleResult.ResultCode == -1)
            {
                return;
            }

            this.m_mongo.Insert(report);
            return;
        }
        #endregion

        #region 异步实现
        public async Task OperateReportAsync(ReportReportElement report, Action<ReportReportElement> callback = null)
        {
            await OperateReportTask(report);
            if (callback != null)
            {
                callback(report);
            }
        }
        public async Task OperateReportWithSaveAsync(List<ReportReportElement> reportList,Action<ReportReportElement> callback=null)
        {
            await Task.Run(() => {
                foreach (ReportReportElement report in reportList)
                {
                    this.m_filler.FillReport(report);
                    if (report.HandleResult.ResultCode != -1)
                    {
                        this.m_handler.HandleReport(report);
                    }
                }
            });

        }
        #endregion

        #region 异步辅助方法
        private Task OperateReportTask(ReportReportElement report)
        {
            return this.m_filler.FillReportAsync(report, FillReportComplete);
        }
        private Task FillReportComplete(ReportReportElement report)
        {
            return this.m_handler.HandleReportAsync(report, HandleReportComplete);
        }
        private Task HandleReportComplete(ReportReportElement report)
        {
            return this.m_mongo.InsertAsync(report, SaveReportComplete);
        }
        private Task SaveReportComplete(ReportReportElement report)
        {
            if (this.m_extraOperate != null)
            {
                return this.m_extraOperate(report);
            }
            return null;
        }

        private Task OperateReportWithoutSaveTask(ReportReportElement report)
        {
            return this.m_filler.FillReportAsync(report, FillReportCompleteWithoutSave);
        }
        private Task FillReportCompleteWithoutSave(ReportReportElement report)
        {
            return this.m_handler.HandleReportAsync(report);
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
