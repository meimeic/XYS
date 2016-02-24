using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Common;
using XYS.Lis.Fill;
using XYS.Lis.Util;
using XYS.Lis.Core;
using XYS.Lis.Model;
using XYS.Lis.Export;
using XYS.Lis.Handler;
using XYS.Lis.Export.Model;
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
        private IReportExport m_exporter;
        private IReportExport m_defaultExport;

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
        public virtual IReportExport DefaultExport
        {
            get 
            {
                if (this.m_defaultExport == null)
                {
                    this.InitDefault();
                }
                return this.m_defaultExport;
            }
        }
        public virtual IReportExport Exporter
        {
            get
            {
                if (this.m_exporter == null)
                {
                    return this.DefaultExport;
                }
                else
                {
                    return this.m_exporter;
                }
            }
            set { this.m_exporter = value; }
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
        public void InitExport(ReportReport export, ReportKey RK)
        {
            ReportReportElement rre = new ReportReportElement();
            this.Filler.Fill(rre, RK);
            bool result = HandlerEvent(rre);
            if (result)
            {
                this.Exporter.export(rre, export);
            }
        }

        public void InitExport(List<ReportReport> exportList, List<ReportKey> RKList)
        {
            ReportReportElement rre = null;
            List<ReportReportElement> reportList = new List<ReportReportElement>(RKList.Count);
            foreach (ReportKey rk in RKList)
            {
                rre = new ReportReportElement();
                this.Filler.Fill(rre, rk);
                reportList.Add(rre);
            }
            bool result = HandlerEvent(reportList);
            if (result)
            {
                this.Exporter.export(reportList, exportList);
            }
        }
        #endregion

        #region 受保护的虚方法
        protected virtual bool HandlerEvent(ReportReportElement element)
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
        protected virtual bool HandlerEvent(List<ReportReportElement> reportElementList)
        {
            IReportHandler handler = this.HandlerHead;
            while (handler != null)
            {
                switch (handler.ReportOptions(reportElementList))
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
        protected virtual void SetExporter(Hashtable exportTable, string exportName)
        {
            IReportExport export = exportTable[exportName] as IReportExport;
            if (export != null)
            {
                this.Exporter = export;
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
            if (this.m_defaultExport == null)
            {
                this.m_defaultExport = this.Hierarchy.DefaultExporter;
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
            SetExporter(this.Hierarchy.ExporterMap, stratrgy.ExporterName);
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
