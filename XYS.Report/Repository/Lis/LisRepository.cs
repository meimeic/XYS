using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;
using XYS.Report.Fill;
using XYS.Report.Fill.Lis;
namespace XYS.Report.Repository.Lis
{
    public delegate void ReporterCreationEventHandler(object sender, ReporterCreationEventArgs e);
    public class ReporterCreationEventArgs : EventArgs
    {
        private LisReporter m_reporter;
        public ReporterCreationEventArgs(LisReporter r)
        {
            this.m_reporter = r;
        }
        public LisReporter Reporter
        {
            get { return this.m_reporter; }
        }
    }
    public class LisRepository : ReporterRepositorySkeleton, IXmlRepositoryConfigurator
    {
        #region
        private LisReporter m_defaultReporter;
        private Hashtable m_key2ReporterMap;
        private IReportFiller m_defaultFiller;
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
        public LisRepository()
            : this(new DefaultReporterFactory())
        { }
        public LisRepository(IReporterFactory reporterFactory)
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
        //获取默认reporter
        public LisReporter DefaultReporter
        {
            get
            {
                if (this.m_defaultReporter == null)
                {
                    lock (this)
                    {
                        if (this.m_defaultReporter == null)
                        {
                            LisReporter def = this.m_defaultFactory.CreateReporter(this, null);
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

        #region
        protected void XmlRepositoryConfigure(XmlElement element)
        {
            ArrayList configurationMessages = new ArrayList();
            using (new ConsoleInfo.InfoReceivedAdapter(configurationMessages))
            {
                XmlLisConfigurator config = new XmlLisConfigurator(this);
                config.Configure(element);
            }
            Configured = true;
            ConfigurationMessages = configurationMessages;
            // Notify listeners
            OnConfigurationChanged(new ConfigurationChangedEventArgs(configurationMessages));
        }
        #endregion
    }
}
