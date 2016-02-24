using System;
using System.Collections;
using System.Xml;

using XYS.Lis.Repository;
using XYS.Lis.Core;
using XYS.Lis.Util;
using XYS.Lis.Fill;
using XYS.Lis.Export;
namespace XYS.Lis.Repository.Hierarchy
{
    public delegate void ReporterCreationEventHandler(object sender, ReporterCreationEventArgs e);

    public class ReporterCreationEventArgs : EventArgs
    {
        private Reporter m_reporter;
        public ReporterCreationEventArgs(Reporter r)
        {
            this.m_reporter = r;
        }
        public Reporter Reporter
        {
            get { return this.m_reporter; }
        }
    }

    public class Hierarchy : ReporterRepositorySkeleton, IXmlRepositoryConfigurator
    {

        #region
        private Reporter m_defaultReporter;
        private Hashtable m_key2ReporterMap;
        private IReportFiller m_defaultFiller;
        private IReportExport m_defaultExport;
        private IReporterFactory m_defaultFactory;
        private event ReporterCreationEventHandler m_reporterCreatedEvent;
        #endregion

        #region 事件属性
        public event ReporterCreationEventHandler ReporterCreatedEvent
        {
            add { this.m_reporterCreatedEvent += value; }
            remove { this.m_reporterCreatedEvent -= value; }
        }
        #endregion

        #region 构造函数
        public Hierarchy()
            : this(new DefaultReporterFactory())
        { }
        public Hierarchy(IReporterFactory reporterFactory)
        {
            if (reporterFactory == null)
            {
                throw new ArgumentNullException("reporterFactory");
            }
            this.m_defaultFactory = reporterFactory;
            this.m_key2ReporterMap = new Hashtable(10);
        }
        #endregion

        #region 实例属性
        public virtual IReportFiller DefaultFiller
        {
            get
            {
                if (this.m_defaultFiller == null)
                {
                    this.m_defaultFiller = new ReportFillByDB();
                }
                return this.m_defaultFiller;
            }
            set { this.m_defaultFiller = value; }
        }
        public virtual IReportExport DefaultExporter
        {
            get
            {
                if (this.m_defaultExport == null)
                {
                    this.m_defaultExport = new PDFExport();
                }
                return this.m_defaultExport;
            }
            set { this.m_defaultExport = value; }
        }
        //获取默认reporter
        public Reporter DefaultReporter
        {
            get
            {
                if (this.m_defaultReporter == null)
                {
                    lock (this)
                    {
                        if (this.m_defaultReporter == null)
                        {
                            Reporter def = this.m_defaultFactory.CreateReporter(this, null);
                            def.Hierarchy = this;
                            this.m_defaultReporter = def;
                            def.InnerConfig();
                        }
                    }
                }
                return this.m_defaultReporter;
            }
        }
        #endregion

        #region 实现IXmlRepositoryConfigurator接口
        public void Configure(XmlElement element)
        {
            XmlRepositoryConfigure(element);
        }
        #endregion

        #region 实现ReporterRepositorySkeleton抽象类
        //查看某个名字的reporter是否存在
        public override IReporter Exists(ReporterKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            return this.m_key2ReporterMap[key] as Reporter;
        }
        //获取当前库中所有reporter
        public override IReporter[] GetCurrentReporters()
        {
            ArrayList reporters = new ArrayList(this.m_key2ReporterMap.Count);
            foreach (object node in this.m_key2ReporterMap.Values)
            {
                if (node is Reporter)
                {
                    reporters.Add(node);
                }
            }
            return (IReporter[])reporters.ToArray(typeof(Reporter));
        }
        //根据名称获取reporter
        public override IReporter GetReporter(ReporterKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            return this.GetReporter(key, this.m_defaultFactory);
        }
        #endregion

        #region 事件处理
        protected virtual void OnReporter(Reporter reporter)
        {
            reporter.InnerConfig();
            OnReporterCreationEvent(reporter);
        }
        //通知事件监听器，处理新建reporter事件
        protected virtual void OnReporterCreationEvent(Reporter reporter)
        {
            ReporterCreationEventHandler handler = this.m_reporterCreatedEvent;
            if (handler != null)
            {
                handler(this, new ReporterCreationEventArgs(reporter));
            }
        }
        #endregion

        #region
        protected void XmlRepositoryConfigure(XmlElement element)
        {
            ArrayList configurationMessages = new ArrayList();
            using (new ReportLog.ReportReceivedAdapter(configurationMessages))
            {
                XmlHierarchyConfigurator config = new XmlHierarchyConfigurator(this);
                config.Configure(element);
            }
            Configured = true;
            ConfigurationMessages = configurationMessages;
            // Notify listeners
            OnConfigurationChanged(new ConfigurationChangedEventArgs(configurationMessages));
        }
        #endregion

        #region  公共方法
        //从缓存中根据reporterName获取reporter,如果reporter不存在，则创建
        public Reporter GetReporter(ReporterKey key, IReporterFactory factory)
        {
            if (key == null)
            {
                throw new ArgumentNullException("reportkey");
            }
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }
            lock (this.m_key2ReporterMap)
            {
                Reporter reporter = null;
                object node = this.m_key2ReporterMap[key];
                if (node == null)
                {
                    reporter = factory.CreateReporter(this, key);
                    reporter.Hierarchy = this;
                    this.m_key2ReporterMap[key] = reporter;
                    OnReporter(reporter);
                    return reporter;
                }
                Reporter nodereporter = node as Reporter;
                if (nodereporter != null)
                {
                    return nodereporter;
                }
                return null;
            }
        }
        #endregion
    }
}
