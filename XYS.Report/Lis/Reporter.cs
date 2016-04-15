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
        Action<ReportReportElement> FillHandler=null;
        #endregion

        #region 构造函数
        protected Reporter()
        {
        }
        #endregion

        #region 实例属性
        public virtual IReportHandler HandlerHead
        {
            get { return this.m_headHandler; }
        }
        #endregion

        #region 实现IReporter接口
        public HandlerResult OperateReport(ReportReportElement report)
        {
            if (report.LisPK == null || !report.LisPK.Configured)
            {
                throw new ArgumentNullException("the report key is null or not config!");
            }

            return HandlerEvent(report);
        }
        #endregion

        #region

        #endregion

        #region 受保护的虚方法
        protected virtual void InitReporter()
        {
            IReportHandler handler = new ReportHeadHandler();
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
        
        protected void InitHandlerChain(IReportHandler head)
        {
            //lock (this.m_addHandlerLock)
            //{
            IReportHandler tailHandler = head;
            IReportHandler currentHandler = null;
            //检验项处理器
            currentHandler = new ReportItemHandler();
            AddHandler(currentHandler, tailHandler);
            //图片项处理器
            currentHandler = new ReportGraphHandler();
            AddHandler(currentHandler, tailHandler);
            //自定义项处理器
            currentHandler = new ReportCustomHandler();
            AddHandler(currentHandler, tailHandler);
            //报告处理器
            currentHandler = new ReportReportHandler();
            AddHandler(currentHandler, tailHandler);
            //}
        }
        protected void AddHandler(IReportHandler current, IReportHandler tail)
        {
            tail.Next = current;
            tail = current;
        }
        
        protected HandlerResult HandlerEvent(ReportReportElement element)
        {
            HandlerResult result = null;
            IReportHandler handler = this.HandlerHead;
            while (handler != null)
            {
                result = handler.ReportOption(element);
                switch (result.Code)
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
