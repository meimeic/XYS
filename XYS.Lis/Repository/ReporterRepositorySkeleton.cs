using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Model;
using XYS.Common;
using XYS.Lis.Core;
using XYS.Lis.Appender;
using XYS.Lis.Model;
using XYS.Lis.Util;
using XYS.Lis.Fill;
using XYS.Lis.Handler;
using XYS.Lis.Export;
namespace XYS.Lis.Repository
{
    public abstract class ReporterRepositorySkeleton : IReporterRepository
    {
        #region 私有字段
        private Hashtable m_name2FillerMap;
        private Hashtable m_name2HandlerMap;
        private Hashtable m_name2ExportMap;
        private Hashtable m_name2StrategyMap;
        private bool m_configured;
        private string m_repositoryName;
        private ICollection m_configurationMessages;
        private PropertiesDictionary m_properties;
        private event ReporterRepositoryConfigurationChangedEventHandler m_configurationChangedEvent;
        #endregion

        #region 构造函数
        protected ReporterRepositorySkeleton():this(new PropertiesDictionary())
        {
        }
        protected ReporterRepositorySkeleton(PropertiesDictionary properties)
        {
            this.m_configured = false;
            this.m_properties = properties;
            this.m_name2FillerMap = new Hashtable(5);
            this.m_name2HandlerMap = new Hashtable(7);
            this.m_name2ExportMap = new Hashtable(5);
            this.m_name2StrategyMap = new Hashtable(5);
        }
        #endregion

        #region 实现IReporterRepository接口属性
        public virtual string RepositoryName
        {
            get { return this.m_repositoryName; }
            set { this.m_repositoryName = value; }
        }
        public virtual Hashtable FillerMap
        {
            get { return this.m_name2FillerMap; }
        }
        public virtual Hashtable HandlerMap
        {
            get { return this.m_name2HandlerMap; }
        }
        public virtual Hashtable ExportMap
        {
            get { return this.m_name2ExportMap; }
        }
        public virtual Hashtable StrategyMap
        {
            get { return this.m_name2StrategyMap; }
        }
        public virtual bool Configured
        {
            get { return m_configured; }
            set { m_configured = value; }
        }
        public virtual ICollection ConfigurationMessages
        {
            get { return m_configurationMessages; }
            set { m_configurationMessages = value; }
        }
        public PropertiesDictionary Properties
        {
            get { return m_properties; }
        }
        #endregion

        #region 实现IReporterRepository接口方法
        public abstract ILisReporter Exists(ReporterKey key);
        
        public abstract ILisReporter[] GetCurrentReporters();

        public abstract ILisReporter GetReporter(ReporterKey key);
        #endregion
        
        #region 实现IReporterRepository接口事件
        public event ReporterRepositoryConfigurationChangedEventHandler ConfigurationChanged
        {
            add { m_configurationChangedEvent += value; }
            remove { m_configurationChangedEvent -= value; }
        }
        #endregion

        #region
        public void RaiseConfigurationChanged(EventArgs e)
        {
            OnConfigurationChanged(e);
        }
        protected virtual void OnConfigurationChanged(EventArgs e)
        {
            if (e == null)
            {
                e = EventArgs.Empty;
            }
            ReporterRepositoryConfigurationChangedEventHandler handler = this.m_configurationChangedEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        #region 公共方法
        public virtual void AddFiller(IReportFiller filler)
        {
            lock (this.m_name2FillerMap)
            {
                this.m_name2FillerMap[filler.FillerName] = filler;
            }
        }
        public virtual void AddHandler(IReportHandler handler)
        {
            lock (this.m_name2HandlerMap)
            {
                this.m_name2HandlerMap[handler.HandlerName] = handler;
            }
        }
        public virtual void AddExport(IReportExport export)
        {
            lock (this.m_name2ExportMap)
            {
                this.m_name2ExportMap[export.ExportName] = export;
            }
        }
        public virtual void AddStrategy(ReporterStrategy strategy)
        {
            lock (this.m_name2StrategyMap)
            {
                this.m_name2StrategyMap[strategy.StrategyName] = strategy;
            }
        }

        public virtual void ClearFiller()
        {
            lock (this.m_name2FillerMap)
            {
                this.m_name2FillerMap.Clear();
            }
        }
        public virtual void ClearHandler()
        {
            lock (this.m_name2HandlerMap)
            {
                this.m_name2HandlerMap.Clear();
            }
        }
        public virtual void ClearExport()
        {
            lock (this.m_name2ExportMap)
            {
                this.m_name2ExportMap.Clear();
            }
        }
        public virtual void ClearStrategy()
        {
            lock (this.m_name2StrategyMap)
            {
                this.m_name2StrategyMap.Clear();
            }
        }
        #endregion
        
        #region 没有调用的方法
        //public virtual void AddElementType(ReportElementType elementType)
        //{
        //    this.m_elementTypeMap.Add(elementType);
        //}

        //public virtual void AddSection(ReporterSection rs)
        //{
        //    this.m_sectionMap.Add(rs);
        //}
        //private void AddBuiltinElements()
        //{
        //    this.m_elementTypeMap.Add(ReportElementType.DEFAULTCUSTOM);
        //    this.m_elementTypeMap.Add(ReportElementType.DEFAULTEXAM);
        //    this.m_elementTypeMap.Add(ReportElementType.DEFAULTITEM);
        //    this.m_elementTypeMap.Add(ReportElementType.DEFAULTGRAPH);
        //    this.m_elementTypeMap.Add(ReportElementType.DEFAULTPATIENT);
        //    //this.m_reportElementMap.Add("reportelement", typeof(ReportReportElement));
        //}
        #endregion
    }
}
