using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Common;
using XYS.Lis.Core;
using XYS.Lis.Fill;
using XYS.Lis.Handler;
using XYS.Lis.Util;

namespace XYS.Lis.Repository.Hierarchy
{
    public abstract class Reporter : IReporter
    {
        #region 变量
        private readonly string m_reporterName;
        private string m_strategyName;
        private Hierarchy m_hierarchy;

        private IReportFiller m_defaultFill;
        private IReportFiller m_filler;
        private IReportHandler m_headHandler;
        private IReportHandler m_tailHandler;

        #endregion

        #region 构造函数
        protected Reporter(string name)
        {
            this.m_reporterName = name;
        }
        protected Reporter(string name, string strategyName)
            : this(name)
        {
            this.m_strategyName = strategyName;
        }
        #endregion

        #region 实例属性
        public virtual IReportFiller DefaultFill
        {
            get
            {
                if (this.m_defaultFill == null)
                {
                    this.InitDefault();
                }
                return this.m_defaultFill;
            }
        }
        public virtual IReportFiller Filler
        {
            get
            {
                if (this.m_filler == null)
                {
                    return this.DefaultFill;
                }
                else
                {
                    return this.m_filler;
                }
            }
            set { this.m_filler = value; }
        }
        public virtual Hierarchy Hierarchy
        {
            get { return this.m_hierarchy; }
            set { this.m_hierarchy = value; }
        }
        public virtual IReportHandler HandlerHead
        {
            get { return this.m_headHandler; }
        }
        #endregion

        #region 实现ILisReport接口
        public string ReporterName
        {
            get { return this.m_reporterName; }
        }
        public virtual string StrategyName
        {
            get
            {
                if (this.m_strategyName != null)
                {
                    return this.m_strategyName.ToLower();
                }
                return null;
            }
            protected set { this.m_strategyName = value; }
        }
        public IReporterRepository Repository
        {
            get { return this.m_hierarchy; }
        }

        public virtual void FillReportElement(IReportElement reportElement, ReportKey key)
        {
            this.Filler.Fill(reportElement, key);
        }
        public virtual void FillReportElement(List<IReportElement> reportElementList, ReportKey key, ReportElementTag elementTag)
        {
            this.Filler.Fill(reportElementList, key, elementTag);
        }

        public virtual bool Option(IReportElement reportElement)
        {
            bool rs = HandlerEvent(reportElement);
            return rs;
        }
        public virtual bool Option(List<IReportElement> reportElementList, ReportElementTag elementTag)
        {
            bool rs = HandlerEvent(reportElementList, elementTag);
            return rs;
        }
        #endregion

        #region 受保护的虚方法
        protected virtual bool HandlerEvent(ILisReportElement element)
        {
            IReportHandler handler = this.HandlerHead;
            while (handler != null)
            {
                switch (handler.ReportOptions(element))
                {
                    case HandlerResult.Fail:
                        return false;
                    case HandlerResult.HasErrorButContinue:
                        handler = handler.Next;
                        break;
                    case HandlerResult.Continue:
                        handler = handler.Next;
                        break;
                    case HandlerResult.Success:
                        handler = null;
                        break;
                    default:
                        return true;
                }
            }
            return true;
        }
        protected virtual bool HandlerEvent(List<ILisReportElement> reportElementList, ReportElementTag elementTag)
        {
            IReportHandler handler = this.HandlerHead;
            while (handler != null)
            {
                switch (handler.ReportOptions(reportElementList, elementTag))
                {
                    case HandlerResult.Fail:
                        return false;
                    case HandlerResult.HasErrorButContinue:
                        handler = handler.Next;
                        break;
                    case HandlerResult.Continue:
                        handler = handler.Next;
                        break;
                    case HandlerResult.Success:
                        handler = null;
                        break;
                    default:
                        return true;
                }
            }
            return true;
        }

        protected virtual void AddHandler(Hashtable handlerTable, IList<string> handlerNameList)
        {
            IReportHandler handler;
            foreach (string name in handlerNameList)
            {
                handler = handlerTable[name] as IReportHandler;
                if (handler != null)
                {
                    AddHandler(handler);
                }
            }
        }
        protected virtual void AddHandler(IReportHandler handler)
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

        protected virtual void SetFiller(Hashtable fillerTable, string fillerName)
        {
            IReportFiller filler = fillerTable[fillerName] as IReportFiller;
            if (filler != null)
            {
                this.Filler = filler;
            }
        }
        #endregion

        #region
        private void InitDefault()
        {
            if (this.Hierarchy == null)
            {
                throw new ArgumentNullException("Hierarchy");
            }
            if (this.m_defaultFill == null)
            {
                this.m_defaultFill = this.Hierarchy.DefaultFiller;
            }
        }
        #endregion

        #region
        public virtual void InnerConfig()
        {
            if (this.Hierarchy == null)
            {
                throw new ArgumentNullException("Hierarchy");
            }
            ReporterStrategy stratrgy = this.Hierarchy.StrategyMap[this.StrategyName] as ReporterStrategy;
            if (stratrgy == null)
            {
                throw new ArgumentNullException("can not find the stratrgy [" + this.StrategyName + "]");
            }
            SetFiller(this.Hierarchy.FillerMap, stratrgy.FillerName);
            AddHandler(this.Hierarchy.HandlerMap, stratrgy.HandlerList);
        }
        public virtual void Reset()
        {
            lock (this)
            {
                InnerConfig();
            }
        }
        #endregion
    }
}
