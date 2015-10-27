using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Common;
using XYS.Lis.Core;
using XYS.Model;
using XYS.Lis.Model;
using XYS.Lis.Fill;
using XYS.Lis.Handler;
using XYS.Lis.Export;
using XYS.Lis.Util;
using XYS.Lis.Repository;

namespace XYS.Lis.Repository.Hierarchy
{
    public abstract class Reporter:ILisReporter
    {
        #region
        private readonly string m_reporterName;
        private string m_strategyName;
        private Hierarchy m_hierarchy;

        private IReportFiller m_defaultFill;
        private IReportExport m_defaultExport;
        private IReportFiller m_filler;
        private IReportExport m_exporter;
        private IReportHandler m_headHandler;
        private IReportHandler m_tailHandler;
        //private readonly Hashtable m_name2ReportFiller;
        //private readonly Hashtable m_tag2ReportExport;

        #endregion

        #region 构造函数
        protected Reporter(string name)
        {
            this.m_reporterName = name;
            //this.m_name2ReportFiller = new Hashtable(3);
            //this.m_tag2ReportExport = new Hashtable(5);
        }
        protected Reporter(string name, string strategyName)
            : this(name)
        {
            this.m_strategyName = strategyName;
        }
        #endregion
        
        #region
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
        public string ReporterName
        {
            get { return this.m_reporterName; }
        }
        public virtual string StrategyName
        {
            get { return this.m_strategyName; }
            protected set { this.m_strategyName = value; }
        }
        
        public IReporterRepository Repository
        {
            get { return this.m_hierarchy; }
        }

        public virtual void FillReportElement(ILisReportElement reportElement, ReportKey key)
        {
            this.Filler.Fill(reportElement, key);
           // IReportFiller filler = this.GetFiller(fillerName);
            //filler.Fill(reportElement, key);
        }
        public virtual void FillReportElement(List<ILisReportElement> reportElementList, ReportKey key, ReportElementTag elementTag)
        {
            this.Filler.Fill(reportElementList, key, elementTag);
            //IReportFiller filler = this.GetFiller(fillerName);
            //filler.Fill(reportElementList, key, elementTag);
        }

        public virtual bool Option(ILisReportElement reportElement)
        {
            bool rs = HandlerEvent(reportElement);
            return rs;
        }
        public virtual bool Option(List<ILisReportElement> reportElementList, ReportElementTag elementTag)
        {
            bool rs = HandlerEvent(reportElementList, elementTag);
            return rs;
        }
        
        public virtual string Export(ILisReportElement reportElement)
        {
            return this.Exporter.export(reportElement);
            //IReportExport export = this.GetExport(exportTag);
            //return export.export(reportElement);
        }
        public virtual string Export(List<ILisReportElement> reportElementList, ReportElementTag elementTag)
        {
            return this.Exporter.export(reportElementList, elementTag);
            //IReportExport export = this.GetExport(exportTag);
            //return export.export(reportElementList, elementTag);
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
        protected virtual bool HandlerEvent(List<ILisReportElement> elementList,ReportElementTag elementTag)
        {
            IReportHandler handler = this.HandlerHead;
            while (handler != null)
            {
                switch (handler.ReportOptions(elementList, elementTag))
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
        //protected virtual IReportFiller GetFiller(string fillerName)
        //{
        //    if (fillerName != null&&!fillerName.Equals(""))
        //    {
        //        IReportFiller filler = this.m_name2ReportFiller[fillerName] as IReportFiller;
        //        if (filler != null)
        //        {
        //            return filler;
        //        }
        //    }
        //    return this.DefaultFill;
        //}
        //protected virtual IReportExport GetExport(ExportTag exportTag)
        //{
        //    IReportExport export = this.m_tag2ReportExport[exportTag] as IReportExport;
        //    if (export != null)
        //    {
        //        return export;
        //    }
        //    return this.DefaultExport;
        //}
        //protected virtual void AddFiller(Hashtable fillerTable, List<string> fillerNameList)
        //{
        //    IReportFiller filler;
        //    foreach (string name in fillerNameList)
        //    {
        //        filler = fillerTable[name] as IReportFiller;
        //        if (filler != null)
        //        {
        //            AddFiller(filler);
        //        }
        //    }
        //}
        protected virtual void AddHandler(Hashtable handlerTable, List<string> handlerNameList)
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
        //protected virtual void AddExport(Hashtable exportTable, List<string> exportNameList)
        //{
        //    IReportExport export;
        //    foreach (string name in exportNameList)
        //    {
        //        export = exportTable[name] as IReportExport;
        //        if (export != null)
        //        {
        //            AddExport(export);
        //        }
        //    }
        //}
        //protected virtual void AddFiller(IReportFiller filler)
        //{
        //    if (filler == null)
        //    {
        //        throw new ArgumentNullException("filler");
        //    }
        //    else
        //    {
        //        this.m_name2ReportFiller[filler.FillerName] = filler;
        //    }
        //}
        //protected virtual void ClearFiller()
        //{
        //    this.m_name2ReportFiller.Clear();
        //}

        //protected virtual void ClearHandler()
        //{
        //    this.m_headHandler = this.m_tailHandler = null;
        //}
        //protected virtual void AddExport(IReportExport export)
        //{
        //    if (export == null)
        //    {
        //        throw new ArgumentNullException("export");
        //    }
        //    else
        //    {
        //        this.m_tag2ReportExport[export.ExportTag] = export;
        //    }
        //}
        //protected virtual void ClearExport()
        //{
        //    this.m_tag2ReportExport.Clear();
        //}
        protected virtual void SetFiller(Hashtable fillerTable,string fillerName)
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
            if (this.m_defaultExport == null)
            {
                this.m_defaultExport = this.Hierarchy.DefaultExport;
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
                throw new ArgumentNullException("can not find stratrgy");
            }
            SetFiller(this.Hierarchy.FillerMap, stratrgy.FillerName);
            AddHandler(this.Hierarchy.HandlerMap, stratrgy.HandlerList);
            SetExporter(this.Hierarchy.ExportMap, stratrgy.ExportName);
        }
        #endregion
    }
}
