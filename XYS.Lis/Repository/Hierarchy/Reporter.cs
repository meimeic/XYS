﻿using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;
using XYS.Lis.Fill;
using XYS.Lis.Util;
using XYS.Lis.Core;
using XYS.Lis.Model;
using XYS.Lis.Handler;
namespace XYS.Lis.Repository.Hierarchy
{
    public abstract class Reporter : IReporter
    {
        #region 变量
        private readonly string m_callerName;
        private readonly string m_strategyName;
        private Hierarchy m_hierarchy;

        private IReportFiller m_defaultFill;
        private IReportFiller m_filler;

        private IReportHandler m_headHandler;
        private IReportHandler m_tailHandler;
        #endregion

        #region 构造函数
        protected Reporter(string callerName, string strategyName)
        {
            this.m_callerName = callerName;
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
        public IReporterRepository Repository
        {
            get { return this.m_hierarchy; }
        }
        public void FillReport(ReportReportElement report, ReportKey RK)
        {
            this.Filler.Fill(report, RK);
        }
        public bool OptionReport(ReportReportElement report)
        {
            return this.HandlerReportEvent(report);
        }
        public bool OptionReport(List<ILisReportElement> reportList)
        {
            return this.HandlerReportEvent(reportList);
        }
        #endregion

        #region 受保护的虚方法
        protected virtual bool HandlerReportEvent(ReportReportElement report)
        {
            return HandlerEvent(report);
        }
        protected virtual bool HandlerReportEvent(List<ILisReportElement> reportList)
        {
            return HandlerEvent(reportList, typeof(ReportReportElement));
        }
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
        protected virtual bool HandlerEvent(List<ILisReportElement> reportElementList, Type type)
        {
            IReportHandler handler = this.HandlerHead;
            while (handler != null)
            {
                switch (handler.ReportOptions(reportElementList, type))
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
            //清空
            this.m_headHandler = this.m_tailHandler = null;
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

        #region 私有方法
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
            ReporterStrategy stratrgy = this.Hierarchy.StrategyMap[this.m_strategyName] as ReporterStrategy;
            if (stratrgy == null)
            {
                throw new ArgumentNullException("can not find the stratrgy [" + this.m_strategyName + "]");
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
