using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using log4net;

using XYS.Report.Lis.Model;
using XYS.Report.Lis.Persistent.Mongo;
namespace XYS.Report.Lis.Persistent
{
    delegate void InsertErrorHandler(ReportReportElement report);
    delegate void InsertSuccessHandler(ReportReportElement report);
    delegate void UpdateErrorHandler(ReportReportElement report);
    delegate void UpdateSuccessHandler(ReportReportElement report);
    public enum SaveOptions
    {
        DirectlySave = 1,
        UpdateAndSave
    }
    public class ReportMongoService
    {
        #region 常量字段
        private static ILog LOG = LogManager.GetLogger("LisReportMongo");
        #endregion

        #region 私有字段
        private LisMongo m_mongo;
        #endregion

        #region 事件字段
        private event InsertErrorHandler m_insertErrorEvent;
        private event InsertSuccessHandler m_insertSuccessEvent;
        private event UpdateErrorHandler m_updateErrorEvent;
        private event UpdateSuccessHandler m_updateSuccessEvent;
        #endregion

        #region 构造函数
        public ReportMongoService()
        {
            this.Init();
        }
        #endregion

        #region 事件属性
        internal event InsertErrorHandler InsertErrorEvent
        {
            add { this.m_insertErrorEvent += value; }
            remove { this.m_insertErrorEvent -= value; }
        }
        internal event InsertSuccessHandler InsertSuccessEvent
        {
            add { this.m_insertSuccessEvent += value; }
            remove { this.m_insertSuccessEvent -= value; }
        }
        internal event UpdateErrorHandler UpdateErrorEvent
        {
            add { this.m_updateErrorEvent += value; }
            remove { this.m_updateErrorEvent -= value; }
        }
        internal event UpdateSuccessHandler UpdateSuccessEvent
        {
            add { this.m_updateSuccessEvent += value; }
            remove { this.m_updateSuccessEvent -= value; }
        }
        #endregion

        #region 实例属性
        protected LisMongo Mongo
        {
            get { return this.m_mongo; }
        }
        #endregion

        #region 同步方法
        public void InsertReport(ReportReportElement report, SaveOptions options)
        {
            switch (options)
            {
                case SaveOptions.DirectlySave:
                    LOG.Info("进入保存报告流程");
                    this.Mongo.InsertReport(report);
                    LOG.Info("退出保存报告流程");
                    break;
                case SaveOptions.UpdateAndSave:
                    LOG.Info("进入更新并保存报告流程");
                    this.Mongo.UpdateAndInsertReport(report);
                    LOG.Info("退出更新并保存报告流程");
                    break;
                default:
                    LOG.Info("进入更新并保存报告流程");
                    this.Mongo.UpdateAndInsertReport(report);
                    LOG.Info("退出更新并保存报告流程");
                    break;
            }
        }
        public void InsertReportMany(IEnumerable<ReportReportElement> reportList)
        {
        }
        #endregion

        #region 内部处理方法
        protected void InsertError(ReportReportElement report)
        {
            //内部处理
            this.LogError(report);
            //转播事件
            this.OnInsertError(report);
        }
        protected void InsertSuccess(ReportReportElement report)
        {
            //内部处理
            LOG.Info("写入Mongo成功");
            //转播事件
            this.OnInsertSuccess(report);
        }
        protected void UpdateError(ReportReportElement report)
        {
            //内部处理
            this.LogError(report);
            //转播事件
            this.OnUpdateError(report);
        }
        protected void UpdateSuccess(ReportReportElement report)
        {
            //内部处理
            LOG.Error("更新Mongo成功,报告ID:" + report.ReportID);
            //转播事件
            this.OnUpdateSuccess(report);
        }
        #endregion

        #region 辅助方法
        protected void SetHandlerResult(HandleResult result, int code, Type type = null, Exception ex = null)
        {
            result.ResultCode = code;
            result.HandleType = type;
            result.Exception = ex;
        }
        #endregion

        #region 触发事件
        private void OnInsertError(ReportReportElement report)
        {
            InsertErrorHandler handler = this.m_insertErrorEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        private void OnInsertSuccess(ReportReportElement report)
        {
            InsertSuccessHandler handler = this.m_insertSuccessEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        private void OnUpdateError(ReportReportElement report)
        {
            UpdateErrorHandler handler = this.m_updateErrorEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        private void OnUpdateSuccess(ReportReportElement report)
        {
            UpdateSuccessHandler handler = this.m_updateSuccessEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        #endregion

        #region 私有方法
        private void Init()
        {
            this.m_mongo = new LisMongo();
            this.Mongo.InsertErrorEvent += this.InsertError;
            this.Mongo.InsertSuccessEvent += this.InsertSuccess;
            this.Mongo.UpdateErrorEvent += this.UpdateError;
            this.Mongo.UpdateSuccessEvent += this.UpdateSuccess;
        }
        private void LogError(ReportReportElement report)
        {
            LOG.Error(report.HandleResult.Message + ",报告ID:" + report.ReportID, report.HandleResult.Exception);
        }
        #endregion
    }
}