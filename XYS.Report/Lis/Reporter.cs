using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Report.Lis.Model;
using XYS.Report.Lis.Handler;
using XYS.Report.Lis.Persistence;
namespace XYS.Report.Lis
{
    public abstract class Reporter : IReporter
    {
        #region 私有字段
        private IReportHandler m_headHandler;
        private IReportHandler m_tailHandler;
        #endregion

        #region 构造函数
        protected Reporter()
        {
            this.InitReporter();
        }
        #endregion

        #region 实例属性
        public virtual IReportHandler HandlerHead
        {
            get { return this.m_headHandler; }
        }
        #endregion

        #region 实现IReport接口
        public string OperateReport(ReportReportElement report)
        {

            HandlerResult result = HandlerEvent(report);
            return result.Message;
        }
        #endregion

        #region 受保护的虚方法    
        protected virtual void InitReporter()
        {
            IReportHandler handler = new ReportFillByDBHandler();
            AddHandler(handler);
            handler = new ReportItemHandler();
            AddHandler(handler);
            handler = new ReportGraphHandler();
            AddHandler(handler);
            handler = new ReportCustomHandler();
            AddHandler(handler);
            handler = new ReportReportHandler();
            AddHandler(handler);
        }
        protected void AddHandler(IReportHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler param must not be null");
            }
            if (this.m_headHandler == null)
            {
                this.m_headHandler = this.m_tailHandler = handler;
            }
            else
            {
                this.m_tailHandler.Next = handler;
                this.m_tailHandler = handler;
            }
        }
        protected virtual HandlerResult HandlerEvent(ReportReportElement element)
        {
            HandlerResult result = null;
            IReportHandler handler = this.HandlerHead;
            while (handler != null)
            {
                result = handler.ReportOption(element);
                switch (result.StatusCode)
                {
                    case 0:
                        return result;
                    case 1:
                        handler = handler.Next;
                        break;
                    default:
                        handler = handler.Next;
                        break;
                }
            }
            return result;
        }
        #endregion

        #region 重置
        public virtual void Reset()
        {
            lock (this)
            {
                InitReporter();
            }
        }
        #endregion
    }
}
