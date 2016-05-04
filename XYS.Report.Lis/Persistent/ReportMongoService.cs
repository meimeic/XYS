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
    public delegate void UpdateAndInsertErrorHandler(ReportReportElement report);
    public delegate void UpdateAndInsertSuccessHandler(ReportReportElement report);
    public class ReportMongoService
    {
        #region 常量字段
        static ILog LOG=LogManager.GetLogger("LisReportMongo");
        #endregion

        #region
        private LisMongo m_mongo;
        #endregion

        #region 实例字段
        private event InsertErrorHandler m_insertErrorEvent;
        private event InsertSuccessHandler m_insertSuccessEvent;
        private event UpdateAndInsertErrorHandler m_updateAndInsertErrorEvent;
        private event UpdateAndInsertSuccessHandler m_updateAndInsertSuccessEvent;
        #endregion

        #region 构造函数
        public ReportMongoService()
        {
            this.m_mongo = new LisMongo();
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
        public event UpdateAndInsertErrorHandler UpdateAndInsertErrorEvent
        {
            add { this.m_updateAndInsertErrorEvent += value; }
            remove { this.m_updateAndInsertErrorEvent -= value; }
        }
        public event UpdateAndInsertSuccessHandler UpdateAndInsertSuccessEvent
        {
            add { this.m_updateAndInsertSuccessEvent += value; }
            remove { this.m_updateAndInsertSuccessEvent -= value; }
        }
        #endregion

        #region 实例属性
        public LisMongo Mongo
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
            this.OnInsert(report);
        }
        public void InsertReportCurrently(ReportReportElement report)
        {
            this.Mongo.InsertReportCurrently(report);
            this.OnInsertCurrently(report);
        }
        public void InsertReportMany(IEnumerable<ReportReportElement> reportList)
        {

        }
        #endregion

        #region 辅助方法
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

        #region 触发事件
        private void OnInsert(ReportReportElement report)
        {
            switch (report.HandleResult.ResultCode)
            {
                case 200:
                    OnInsertSuccess(report);
                    break;
                case -200:
                    OnInsertError(report);
                    break;
                default:
                    OnInsertSuccess(report);
                    break;
            }
        }
        private void OnInsertCurrently(ReportReportElement report)
        {
            switch (report.HandleResult.ResultCode)
            {
                case 200:
                    OnInsertSuccess(report);
                    break;
                case 201:
                    OnUpdateAndInsertSuccess(report);
                    break;
                case -201:
                    OnUpdateAndInsertError(report);
                    break;
                case -202:
                    break;
            }
        }
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
        private void OnUpdateAndInsertError(ReportReportElement report)
        {
            UpdateAndInsertErrorHandler handler = this.m_updateAndInsertErrorEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        private void OnUpdateAndInsertSuccess(ReportReportElement report)
        {
            UpdateAndInsertSuccessHandler handler = this.m_updateAndInsertSuccessEvent;
            if (handler != null)
            {
                handler(report);
            }
        }
        #endregion
    }
}