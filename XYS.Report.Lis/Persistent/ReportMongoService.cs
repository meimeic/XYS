using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using log4net;

using XYS.Report.Lis.Model;
using XYS.Report.Lis.Persistent.Mongo;
namespace XYS.Report.Lis.Persistent
{
    public delegate void InsertErrorHandler(ReportReportElement report);
    public delegate void InsertSuccessHandler(ReportReportElement report);
    public delegate void UpdateErrorHandler(ReportReportElement report);
    public delegate void UpdateSuccessHandler(ReportReportElement report);
    public class ReportMongoService
    {
        #region 常量字段
        static ILog LOG = LogManager.GetLogger("LisReportMongo");
        #endregion

        #region
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
        public event InsertErrorHandler InsertErrorEvent
        {
            add { this.m_insertErrorEvent += value; }
            remove { this.m_insertErrorEvent -= value; }
        }
        public event InsertSuccessHandler InsertSuccessEvent
        {
            add { this.m_insertSuccessEvent += value; }
            remove { this.m_insertSuccessEvent -= value; }
        }
        public event UpdateErrorHandler UpdateErrorEvent
        {
            add { this.m_updateErrorEvent += value; }
            remove { this.m_updateErrorEvent -= value; }
        }
        public event UpdateSuccessHandler UpdateSuccessEvent
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
        public void InsertReport(ReportReportElement report)
        {
            LOG.Info("进入保存报告流程");
            this.Mongo.InsertReport(report);
            LOG.Info("退出保存报告流程");
        }
        public void InsertReportCurrently(ReportReportElement report)
        {
            LOG.Info("进入更新并保存报告流程");
            this.Mongo.UpdateAndInsertReport(report);
            LOG.Info("退出更新并保存报告流程");
        }
        public void InsertReportMany(IEnumerable<ReportReportElement> reportList)
        {

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

        #region
        protected void InsertError(ReportReportElement report)
        {
            //内部处理
            LOG.Error("写入Mongo失败",report.HandleResult.Exception);
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
            LOG.Error("更新Mongo失败,更新报告ID:" + report.ReportID, report.HandleResult.Exception);
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

        #region
        private void Init()
        {
            this.m_mongo = new LisMongo();
            this.Mongo.InsertErrorEvent += this.InsertError;
            this.Mongo.InsertSuccessEvent += this.InsertSuccess;
            this.Mongo.UpdateErrorEvent += this.UpdateError;
            this.Mongo.UpdateSuccessEvent += this.UpdateSuccess;
        }
        #endregion
    }
}