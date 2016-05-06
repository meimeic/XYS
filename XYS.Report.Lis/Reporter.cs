//using System;
//using System.Text;
//using System.Collections;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//using log4net;

//using XYS.Util;
//using XYS.Report.Lis.Model;
//using XYS.Report.Lis.Handler;
//using XYS.Report.Lis.Persistent;
//namespace XYS.Report.Lis
//{
//    public abstract class Reporter
//    {
//        #region 常量字段
//        static readonly ILog LOG = LogManager.GetLogger("LisReport");
//        #endregion

//        #region 私有字段
//        private ReportHandleService m_handleService;
//        private ReportMongoService m_mongoService;
//        #endregion

//        #region 构造函数
//        protected Reporter()
//        {
//            this.InitializeReporter();
//        }
//        #endregion

//        #region 实例属性
//        public ReportHandleService HandleService
//        {
//            get { return this.m_handleService; }
//        }
//        public ReportMongoService MongoService
//        {
//            get { return this.m_mongoService; }
//        }
//        #endregion

//        #region 同步
//        public void InitReport(ReportReportElement report)
//        {
//            this.HandleService.HandleReport(report);
//        }
//        public void InitReport(List<ReportReportElement> reportList)
//        {
//            foreach (ReportReportElement report in reportList)
//            {

//            }
//        }
//        public void OperateReport(ReportReportElement report)
//        {
//            LOG.Info("开始处理报告 主键为:" + report.ReportPK.ID);
//            this.HandleService.HandleReport(report);
//        }
//        public void OperateReport(List<ReportReportElement> reportList)
//        {
//            DateTime startTime = DateTime.Now;
//            foreach (ReportReportElement report in reportList)
//            {
//                this.HandleService.HandleReport(report);
//            }
//            DateTime endTime = DateTime.Now;
//            Console.WriteLine("处理运行时间为:" + endTime.Subtract(startTime).TotalMilliseconds + " ms");

//            startTime = DateTime.Now;
//            //foreach (ReportReportElement report in reportList)
//            //{
//            //    if (report.HandleResult.ResultCode > 0)
//            //    {
//            //        this.MongoService.InsertReport(report);
//            //    }
//            //}
//            this.MongoService.InsertReportMany(reportList);
//            endTime = DateTime.Now;
//            Console.WriteLine("保存运行时间为:" + endTime.Subtract(startTime).TotalMilliseconds + " ms");
//            foreach (ReportReportElement report in reportList)
//            {
//                this.Log(report);
//            }
//        }
//        #endregion

//        #region 异步
//        //public async Task OperateReportAsync(List<ReportReportElement> reportList, Action<ReportReportElement> callback = null)
//        //{
//        //    Task task = null;
//        //    List<Task> taskList = new List<Task>(1000);
//        //    DateTime startTime = DateTime.Now;
//        //    foreach (ReportReportElement report in reportList)
//        //    {
//        //        task = this.HandleService.HandleReportAsync(report, HandleReportComplete);
//        //        taskList.Add(task);
//        //    }
//        //    await Task.WhenAll(taskList);
//        //    DateTime endTime = DateTime.Now;
//        //    Console.WriteLine("处理运行时间为:" + endTime.Subtract(startTime).TotalMilliseconds + " ms");

//        //    startTime = DateTime.Now;
//        //    this.MongoService.InsertReportMany(reportList);
//        //    endTime = DateTime.Now;
//        //    Console.WriteLine("保存运行时间为:" + endTime.Subtract(startTime).TotalMilliseconds + " ms");

//        //    //await this.MongoService.InsertReportCurrentlyAsync(report);
//        //    foreach (ReportReportElement report in reportList)
//        //    {
//        //        await this.Log(report);
//        //        if (callback != null)
//        //        {
//        //            callback(report);
//        //        }
//        //    }
//        //}
//        #endregion

//        #region 多线程
//        private Task OperateReportTask(ReportReportElement report)
//        {
//            return Task.Run(() =>
//            {
//            });
//        }
//        #endregion

//        #region 处理代码
//        protected void HandleError(ReportReportElement report)
//        {
//            //
//            LOG.Error("处理报告失败，进入错误处理流程。", report.HandleResult.Exception);

//            //处理流程
//        }
//        protected void HandleSuccess(ReportReportElement report)
//        {
//            LOG.Info("处理报告成功,进入保存报告流程");
//            this.MongoService.InsertReport(report);
//        }

//        protected void SaveError(ReportReportElement report)
//        {
//            LOG.Error("保存报告失败，进入错误处理流程", report.HandleResult.Exception);
//        }
//        protected void SaveSuccess(ReportReportElement report)
//        {
//            LOG.Info("保存报告成功");
//        }
//        protected void UpdateAndSaveError(ReportReportElement report)
//        {
//        }
//        protected void UpdateAndSaveSuccess(ReportReportElement report)
//        {
//        }
//        #endregion

//        #region 辅助方法
//        private Task Log(ReportReportElement report)
//        {
//            return Task.Run(() =>
//            {
//            });
//        }
//        protected void SetHandlerResult(HandleResult result, int code, Type type = null, Exception ex = null)
//        {
//            result.ResultCode = code;
//            result.HandleType = type;
//            result.Exception = ex;
//        }
//        #endregion

//        #region 初始化
//        private void InitializeReporter()
//        {
//            this.m_handleService = new ReportHandleService();
//            this.m_mongoService = new ReportMongoService();

//            ////注册处理事件
//            this.HandleService.HandleErrorEvent += this.HandleError;
//            this.HandleService.HandleCompleteEvent += this.HandleSuccess;

//            ////注册保存事件
//            this.MongoService.InsertErrorEvent += this.SaveError;
//            this.MongoService.InsertSuccessEvent += this.SaveSuccess;
//            this.MongoService.UpdateErrorEvent += this.UpdateAndSaveError;
//            this.MongoService.UpdateSuccessEvent += this.UpdateAndSaveSuccess;
//        }
//        #endregion
//    }
//}
