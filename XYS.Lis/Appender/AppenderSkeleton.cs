using System;
using System.Collections;
using XYS.Lis.Core;
using XYS.Lis.Export;
using XYS.Lis.Handler;
namespace XYS.Lis.Appender
{
    public abstract class AppenderSkeleton : IAppender
    {

        #region 私有字段
        private IReportHandler m_headHandler;
        private IReportHandler m_tailHandler;
        private Hashtable m_tag2Export;
        private string m_appenderName;
        #endregion

        #region 实现 IAppender接口
        public string AppenderName
        {
            get { return this.m_appenderName; }
            set { this.m_appenderName = value; }
        }
        public virtual void AddExport(IReportExport export)
        {
            lock (this)
            {
                this.m_tag2Export.Add(export.ExportTag, export);
            }
        }
        public void ClearExport()
        {
            this.m_tag2Export.Clear();
        }
        public virtual void ClearHandler()
        {
            this.m_headHandler = this.m_tailHandler = null;
        }
        public virtual void AddHandler(IReportHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("filter param must not be null");
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
        public void Close()
        {
            throw new NotImplementedException();
        }
        public string DoAppend(ILisReportElement reportElement,ExportTag exportTag)
        {
            if (this.HandlerEvent(reportElement)&&this.PreAppendCheck())
            {

                return this.Append(reportElement, exportTag);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region
        protected abstract string Append(ILisReportElement reportElement, ExportTag exportTag);
        #endregion

        #region 
        protected string RenderExport(ILisReportElement reportElement,ExportTag exportTag)
        {
            IReportExport exp = GetExport(exportTag);
            if (exp == null)
            {
                throw new InvalidOperationException("can not find the spec export");
            }
            else
            {
                return exp.export(reportElement);
            }
        }
        protected IReportExport GetExport(ExportTag exportTag)
        {
            lock (this)
            {
                return this.m_tag2Export[exportTag] as IReportExport;
            }
        }
        #endregion

        #region
        public virtual IReportHandler HandlerHead
        {
            get { return this.m_headHandler; }
        }
        protected virtual bool RequiresExport
        {
            get { return true; }
        }
        #endregion

        #region
        protected virtual bool HandlerEvent(ILisReportElement element)
        {
            IReportHandler handler = this.HandlerHead;
            while (handler != null)
            {
                switch (handler.ReportOptions(element))
                {
                    case HandlerResult.Fail:
                        return false;
                    case HandlerResult.Success:
                        handler = null;
                        break;
                    case HandlerResult.Continue:
                        handler = handler.Next;
                        break;
                }
            }
            return true;
        }
        protected virtual bool PreAppendCheck()
        {
            if (this.m_tag2Export == null && this.RequiresExport)
            {
                return false;
            }
            return true;
        }
        #endregion
}
}
