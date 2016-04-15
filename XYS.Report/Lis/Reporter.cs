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
        private FillReport m_fill;
        private HandleReport m_handle;
        private SaveReport m_save;
        #endregion

        #region 构造函数
        protected Reporter()
        {
        }
        #endregion

        #region 实现IReporter接口
        public HandlerResult OperateReport(ReportReportElement report)
        {
            HandlerResult result = null;
            if (report.LisPK == null || !report.LisPK.Configured)
            {
                return new HandlerResult(-1, this.GetType(), "the report key is null or not config!");
            }
            result = this.m_filler.FillReport(report);
            if (result.Code != -1)
            {
                result = this.m_handler.HandleReport(report);
                if (result.Code != -1)
                {
                    result = this.m_mongo.Insert(report);
                }
            }
            return result;
        }
        #endregion

        #region 异步实现
        public async Task OperateReportAsync(ReportReportElement report, Action<ReportReportElement, HandlerResult> callback)
        {
            HandlerResult result = await OperateReportTask(report);
            if (callback != null)
            {
                callback(report, result);
            }
        }
        private Task<HandlerResult> OperateReportTask(ReportReportElement report)
        {

        }
        #endregion

        #region 受保护的虚方法
        #endregion

    }
}
