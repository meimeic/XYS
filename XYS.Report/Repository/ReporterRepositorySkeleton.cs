using System;
using System.Collections;

using XYS.Util;
using XYS.Report.Fill;
using XYS.Report.Core;
using XYS.Report.Handler;
using XYS.Report.Util;
namespace XYS.Report.Repository
{
    public abstract class ReporterRepositorySkeleton : IReporterRepository
    {
        #region 私有字段
        private bool m_configured;
        private string m_repositoryName;
        private Hashtable m_name2FillerMap;
        private Hashtable m_name2HandlerMap;
        private Hashtable m_name2StrategyMap;
        private ICollection m_configurationMessages;
        private PropertiesDictionary m_properties;
        private event ReporterRepositoryConfigurationChangedEventHandler m_configurationChangedEvent;
        #endregion

        #region 构造函数
        protected ReporterRepositorySkeleton()
            : this(new PropertiesDictionary())
        {
        }
        protected ReporterRepositorySkeleton(PropertiesDictionary properties)
        {
            this.m_configured = false;
            this.m_properties = properties;
            this.m_name2FillerMap = SystemInfo.CreateCaseInsensitiveHashtable(2);
            this.m_name2HandlerMap = SystemInfo.CreateCaseInsensitiveHashtable(5);
            this.m_name2StrategyMap = SystemInfo.CreateCaseInsensitiveHashtable(5);
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
        public abstract IReporter Exists(ReporterKey key);
        public abstract IReporter[] GetCurrentReporters();
        public abstract IReporter GetReporter(ReporterKey key);
        #endregion

        #region 实现IReporterRepository接口事件
        public event ReporterRepositoryConfigurationChangedEventHandler ConfigurationChanged
        {
            add { m_configurationChangedEvent += value; }
            remove { m_configurationChangedEvent -= value; }
        }
        #endregion

        #region 触发器
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
        public virtual void ClearStrategy()
        {
            lock (this.m_name2StrategyMap)
            {
                this.m_name2StrategyMap.Clear();
            }
        }

        #endregion
    }
}
