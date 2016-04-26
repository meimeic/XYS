using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using XYS.Util;
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
            this.Log(report);
            if (callback != null)
            {
                callback(report);
            }
        }
        public void OperateReport(List<ReportReportElement> reportList, Action<ReportReportElement> callback = null)
        {
            DateTime startTime = DateTime.Now;
            foreach (ReportReportElement report in reportList)
            {
                this.HandleService.HandleReport(report);
            }
            DateTime endTime = DateTime.Now;
            Console.WriteLine("处理运行时间为:" + endTime.Subtract(startTime).TotalMilliseconds + " ms");

            startTime = DateTime.Now;
            //foreach (ReportReportElement report in reportList)
            //{
            //    if (report.HandleResult.ResultCode > 0)
            //    {
            //        this.MongoService.InsertReport(report);
            //    }
            //}
            this.MongoService.InsertReportMany(reportList);
            endTime = DateTime.Now;
            Console.WriteLine("保存运行时间为:" + endTime.Subtract(startTime).TotalMilliseconds + " ms");
            foreach (ReportReportElement report in reportList)
            {
                this.Log(report);
                if (callback != null)
                {
                    callback(report);
                }
            }
        }
        #endregion

        #region 异步实现
        public async Task OperateReportAsync(List<ReportReportElement> reportList, Action<ReportReportElement> callback = null)
        {
            Task task = null;
            List<Task> taskList = new List<Task>(1000);
            DateTime startTime = DateTime.Now;
            foreach (ReportReportElement report in reportList)
            {
                task = this.HandleService.HandleReportAsync(report, HandleReportComplete);
                taskList.Add(task);
            }
            await Task.WhenAll(taskList);
            DateTime endTime = DateTime.Now;
            Console.WriteLine("处理运行时间为:" + endTime.Subtract(startTime).TotalMilliseconds + " ms");

            startTime = DateTime.Now;
            this.MongoService.InsertReportMany(reportList);
            endTime = DateTime.Now;
            Console.WriteLine("保存运行时间为:" + endTime.Subtract(startTime).TotalMilliseconds + " ms");

            //await this.MongoService.InsertReportCurrentlyAsync(report);
            foreach (ReportReportElement report in reportList)
            {
                await this.Log(report);
                if (callback != null)
                {
                    callback(report);
                }
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
            Console.WriteLine("handle report complete");
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
