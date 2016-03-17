using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;

using XYS.Report.Lis.Core;
using XYS.Report.Lis.Filler;
namespace XYS.Report.Lis.Repository
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
        #region 私有字段
        private Reporter m_defaultReporter;
        private Hashtable m_key2ReporterMap;
        private IReportFiller m_defaultFiller;
        private IReporterFactory m_defaultFactory;
        #endregion

        #region 构造函数
        public Hierarchy()
            : this(new DefaultReporterFactory())
        {
        }
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
        #endregion

        #region 实现IXmlRepositoryConfigurator接口
        public void Configure(XmlElement element)
        {
            XmlRepositoryConfigure(element);
        }
        #endregion

        #region 通过xml配置Repository
        protected void XmlRepositoryConfigure(XmlElement element)
        {
            ArrayList configurationMessages = new ArrayList();
            using (new ConsoleInfo.InfoReceivedAdapter(configurationMessages))
            {
                XmlHierarchyConfigurator config = new XmlHierarchyConfigurator(this);
                config.Configure(element);
            }
            Configured = true;
            ConfigurationMessages = configurationMessages;
        }
        #endregion

        #region 实现父类抽象方法
        //查看某个名字的reporter是否存在
        public override ILisReporter Exists(ReporterKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            return this.m_key2ReporterMap[key] as Reporter;
        }
        //根据名称获取reporter
        public override ILisReporter GetReporter(ReporterKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            return this.GetReporter(key, this.m_defaultFactory);
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
                    reporter.InitReporter();
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
