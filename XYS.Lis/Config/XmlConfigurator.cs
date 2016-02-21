using System;
using System.Collections;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Net;
using System.Threading;

using XYS.Lis.Util;
using XYS.Lis.Core;
using XYS.Lis.Repository;
namespace XYS.Lis.Config
{
    public sealed class XmlConfigurator
    {

        #region 私有常量
        private static readonly Type declaringType = typeof(XmlConfigurator);
        private static readonly Hashtable m_repositoryName2ConfigAndWatchHandler = new Hashtable(2);
        #endregion
        
        #region 私有构造函数
        private XmlConfigurator()
        {
        }
        #endregion

        #region 静态方法
        public static ICollection Configure()
        {
            return Configure(ReportManager.GetRepository(Assembly.GetCallingAssembly()));
        }
        public static ICollection Configure(IReporterRepository repository)
        {
            ArrayList configurationMessages = new ArrayList();
            using (new ReportLog.ReportReceivedAdapter(configurationMessages))
            {
                InternalConfigure(repository);
            }
            repository.ConfigurationMessages = configurationMessages;
            return configurationMessages;
        }
        
        public static ICollection Configure(XmlElement element)
        {
            ArrayList configurationMessages = new ArrayList();
            IReporterRepository repository = ReportManager.GetRepository(Assembly.GetCallingAssembly());
            using (new ReportLog.ReportReceivedAdapter(configurationMessages))
            {
                InternalConfigureFromXml(repository, element);
            }
            repository.ConfigurationMessages = configurationMessages;
            return configurationMessages;
        }
        public static ICollection Configure(IReporterRepository repository, XmlElement element)
        {
            ArrayList configurationMessages = new ArrayList();
            using (new ReportLog.ReportReceivedAdapter(configurationMessages))
            {
                ReportLog.Debug(declaringType, "XmlConfigurator:configuring repository [" + repository.RepositoryName + "] using XML element");
                InternalConfigureFromXml(repository, element);
            }
            repository.ConfigurationMessages = configurationMessages;
            return configurationMessages;
        }
        
        public static ICollection Configure(FileInfo configFile)
        {
            ArrayList configurationMessages = new ArrayList();
            using (new ReportLog.ReportReceivedAdapter(configurationMessages))
            {
                InternalConfigure(ReportManager.GetRepository(Assembly.GetCallingAssembly()), configFile);
            }
            return configurationMessages;
        }
        public static ICollection Configure(IReporterRepository repository, FileInfo configFile)
        {
            ArrayList configurationMessages = new ArrayList();
            using (new ReportLog.ReportReceivedAdapter(configurationMessages))
            {
                InternalConfigure(repository, configFile);
            }
            repository.ConfigurationMessages = configurationMessages;
            return configurationMessages;
        }
       
        public static ICollection Configure(Uri configUri)
        {
            ArrayList configurationMessages = new ArrayList();
            IReporterRepository repository = ReportManager.GetRepository(Assembly.GetCallingAssembly());
            using (new ReportLog.ReportReceivedAdapter(configurationMessages))
            {
                InternalConfigure(repository, configUri);
            }
            repository.ConfigurationMessages = configurationMessages;
            return configurationMessages;
        }
        public static ICollection Configure(IReporterRepository repository, Uri configUri)
        {
            ArrayList configurationMessages = new ArrayList();
            using (new ReportLog.ReportReceivedAdapter(configurationMessages))
            {
                InternalConfigure(repository, configUri);
            }
            repository.ConfigurationMessages = configurationMessages;
            return configurationMessages;
        }
        
        public static ICollection Configure(Stream configStream)
        {
            ArrayList configurationMessages = new ArrayList();
            IReporterRepository repository = ReportManager.GetRepository(Assembly.GetCallingAssembly());
            using (new ReportLog.ReportReceivedAdapter(configurationMessages))
            {
                InternalConfigure(repository, configStream);
            }
            repository.ConfigurationMessages = configurationMessages;
            return configurationMessages;
        }
        public static ICollection Configure(IReporterRepository repository, Stream configStream)
        {
            ArrayList configurationMessages = new ArrayList();
            using (new ReportLog.ReportReceivedAdapter(configurationMessages))
            {
                InternalConfigure(repository, configStream);
            }
            repository.ConfigurationMessages = configurationMessages;
            return configurationMessages;
        }
        
        public static ICollection ConfigureAndWatch(FileInfo configFile)
        {
            ArrayList configurationMessages = new ArrayList();
            IReporterRepository repository = ReportManager.GetRepository(Assembly.GetCallingAssembly());
            using (new ReportLog.ReportReceivedAdapter(configurationMessages))
            {
                InternalConfigureAndWatch(repository, configFile);
            }
            repository.ConfigurationMessages = configurationMessages;
            return configurationMessages;
        }
        public static ICollection ConfigureAndWatch(IReporterRepository repository, FileInfo configFile)
        {
            ArrayList configurationMessages = new ArrayList();

            using (new ReportLog.ReportReceivedAdapter(configurationMessages))
            {
                InternalConfigureAndWatch(repository, configFile);
            }
            repository.ConfigurationMessages = configurationMessages;
            return configurationMessages;
        }
        #endregion

        #region 获取指定元素
        public static XmlElement GetParamConfigurationElement(string tagName)
        {
            XmlElement resultElement = null;
            string configFullFile = SystemInfo.GetFileFullName(SystemInfo.ApplicationBaseDirectory, "Lis.config");
            XmlElement rootElement = GetRootConfigurationElement(new FileInfo(configFullFile));
            if (rootElement != null)
            {
                XmlNodeList configNodeList = rootElement.GetElementsByTagName(tagName);
                if (configNodeList.Count == 0)
                {
                    ReportLog.Debug(declaringType, "XmlConfigurator:XML configuration does not contain a <" + tagName + "> element. Configuration Aborted.");
                }
                else if (configNodeList.Count > 1)
                {
                    resultElement = configNodeList[0] as XmlElement;
                    ReportLog.Error(declaringType, "XmlConfigurator:XML configuration contains [" + configNodeList.Count + "] <" + tagName + "> elements. Only one is allowed. Configuration Aborted.");
                }
                else
                {
                    resultElement = configNodeList[0] as XmlElement;
                }
            }
            return resultElement;
        }
        public static XmlElement GetRootConfigurationElement(FileInfo configFile)
        {
            XmlElement root = null;
            if (configFile == null)
            {
                ReportLog.Error(declaringType, "XmlConfigurator:Configure called with null 'configFile' parameter");
            }
            else
            {
                //文件存在，尝试打开文件
                if (File.Exists(configFile.FullName))
                {
                    FileStream fs = null;
                    for (int retry = 5; --retry >= 0; )
                    {
                        try
                        {
                            fs = configFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                            break;
                        }
                        catch (IOException ex)
                        {
                            if (retry == 0)
                            {
                                ReportLog.Error(declaringType, "XmlConfigurator:Failed to open XML config file [" + configFile.Name + "]", ex);
                                fs = null;
                            }
                            System.Threading.Thread.Sleep(250);
                        }
                    }
                    //处理文件流
                    if (fs != null)
                    {
                        try
                        {
                            root = GetRootConfigurationElement(fs);
                        }
                        finally
                        {
                            fs.Close();
                        }
                    }
                }
                else
                {
                    ReportLog.Debug(declaringType, "XmlConfigurator:config file [" + configFile.FullName + "] not found.");
                }
            }
            return root;
        }
        public static XmlElement GetRootConfigurationElement(Stream configStream)
        {
            XmlElement root = null;
            XmlDocument doc = new XmlDocument();
            try
            {
                XmlReaderSettings xmlSettings = new XmlReaderSettings();
                xmlSettings.ValidationType = ValidationType.None;
                XmlReader xmlReader = XmlReader.Create(new XmlTextReader(configStream), xmlSettings);
                //将文件流加载到xml文档
                doc.Load(xmlReader);
                root = doc.DocumentElement;
            }
            catch (Exception ex)
            {
                ReportLog.Error(declaringType, "XmlConfigurator:Error while loading XML configuration", ex);
            }
            return root;
        }
        #endregion

        #region
        private static void InternalConfigure(IReporterRepository repository)
        {
            ReportLog.Debug(declaringType, "XmlConfigurator:configuring repository [" + repository.RepositoryName + "] using .config file section");
            try
            {
                ReportLog.Debug(declaringType, "XmlConfigurator:Application config file is [" + SystemInfo.ConfigurationFileLocation + "]");
            }
            catch
            {
                // ignore error
                ReportLog.Debug(declaringType, "XmlConfigurator:Application config file location unknown");
            }
#if NETCF
			// No config file reading stuff. Just go straight for the file
			Configure(repository, new FileInfo(SystemInfo.ConfigurationFileLocation));
#else
            try
            {
                XmlElement configElement = null;
#if NET_2_0
				configElement = System.Configuration.ConfigurationManager.GetSection("lis-report") as XmlElement;
#else
                configElement = System.Configuration.ConfigurationManager.GetSection("lis-report") as XmlElement;
#endif
                if (configElement == null)
                {
                    // Failed to load the xml config using configuration settings handler
                    ReportLog.Error(declaringType, "XmlConfigurator:Failed to find configuration section 'lis-report' in the application's .config file. Check your .config file for the <lis-report> and <configSections> elements. The configuration section should look like: <section name=\"lis-report\" type=\"XYS.Lis.Config.ReportSectionHandler,XYS.Lis\" />");
                }
                else
                {
                    InternalConfigureFromXml(repository, configElement);
                }
            }
            catch (System.Configuration.ConfigurationException confEx)
            {
                if (confEx.BareMessage.IndexOf("Unrecognized element") >= 0)
                {
                    // Looks like the XML file is not valid
                    ReportLog.Error(declaringType, "XmlConfigurator:Failed to parse config file. Check your .config file is well formed XML.", confEx);
                }
                else
                {
                    // This exception is typically due to the assembly name not being correctly specified in the section type.
                    string configSectionStr = "<section name=\"lis-report\" type=\"XYS.Lis.Config.ConfigurationSectionHandler," + Assembly.GetExecutingAssembly().FullName + "\" />";
                    ReportLog.Error(declaringType, "XmlConfigurator:Failed to parse config file. Is the <configSections> specified as: " + configSectionStr, confEx);
                }
            }
#endif
        }
        private static void InternalConfigure(IReporterRepository repository, Stream configStream)
        {
            ReportLog.Debug(declaringType, "XmlConfigurator:configuring repository [" + repository.RepositoryName + "] using stream");
            if (configStream == null)
            {
                ReportLog.Error(declaringType, "XmlConfigurator:Configure called with null 'configStream' parameter");
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                try
                {
#if (NETCF)
					// Create a text reader for the file stream
					XmlTextReader xmlReader = new XmlTextReader(configStream);
#elif NET_2_0
					// Allow the DTD to specify entity includes
					XmlReaderSettings settings = new XmlReaderSettings();
                                        // .NET 4.0 warning CS0618: 'System.Xml.XmlReaderSettings.ProhibitDtd'
                                        // is obsolete: 'Use XmlReaderSettings.DtdProcessing property instead.'
#if !NET_4_0
					settings.ProhibitDtd = false;
#else
					settings.DtdProcessing = DtdProcessing.Parse;
#endif

					// Create a reader over the input stream
					XmlReader xmlReader = XmlReader.Create(configStream, settings);
#else
                    // Create a validating reader around a text reader for the file stream
                    XmlReaderSettings xmlSettings = new XmlReaderSettings();
                    xmlSettings.ValidationType = ValidationType.None;
                    XmlReader xmlReader = XmlReader.Create(new XmlTextReader(configStream),xmlSettings);
#endif
                    //将文件流加载到xml文档
                    doc.Load(xmlReader);
                }
                catch (Exception ex)
                {
                    ReportLog.Error(declaringType, "XmlConfigurator:Error while loading XML configuration", ex);
                    doc = null;
                }

                if (doc != null)
                {
                    ReportLog.Debug(declaringType, "XmlConfigurator:loading XML configuration");
                    //获取节点集合
                    XmlNodeList configNodeList = doc.GetElementsByTagName("lis-report");
                    //节点必须有且只有一个
                    if (configNodeList.Count == 0)
                    {
                        ReportLog.Debug(declaringType, "XmlConfigurator:XML configuration does not contain a <lis-report> element. Configuration Aborted.");
                    }
                    else if (configNodeList.Count > 1)
                    {
                        ReportLog.Error(declaringType, "XmlConfigurator:XML configuration contains [" + configNodeList.Count + "] <lis-report> elements. Only one is allowed. Configuration Aborted.");
                    }
                    else
                    {
                        //根据节点信息，配置repository
                        InternalConfigureFromXml(repository, configNodeList[0] as XmlElement);
                    }
                }
            }
        }
        private static void InternalConfigureFromXml(IReporterRepository repository, XmlElement element)
        {
            if (element == null)
            {
                ReportLog.Error(declaringType, "XmlConfigurator:ConfigureFromXml called with null 'element' parameter");
            }
            else if (repository == null)
            {
                ReportLog.Error(declaringType, "XmlConfigurator:ConfigureFromXml called with null 'repository' parameter");
            }
            else
            {
                ReportLog.Debug(declaringType, "XmlConfigurator:Configuring Repository [" + repository.RepositoryName + "] using XML element");
                //将repository转换成可配置类型
                IXmlRepositoryConfigurator configurableRepository = repository as IXmlRepositoryConfigurator;
                if (configurableRepository == null)
                {
                    //不可配置
                    ReportLog.Warn(declaringType, "XmlConfigurator:Repository [" + repository + "] does not support the XmlConfigurator");
                }
                else
                {
                    XmlDocument newDoc = new XmlDocument();
                    XmlElement newElement = (XmlElement)newDoc.AppendChild(newDoc.ImportNode(element, true));
                    //配置
                    configurableRepository.Configure(newElement);
                }
            }
        }
        private static void InternalConfigure(IReporterRepository repository, FileInfo configFile)
        {
            ReportLog.Debug(declaringType, "XmlConfigurator:configuring repository [" + repository.RepositoryName + "] using file [" + configFile + "]");

            if (configFile == null)
            {
                ReportLog.Error(declaringType, "XmlConfigurator:Configure called with null 'configFile' parameter");
            }
            else
            {
                //文件存在，尝试打开文件
                if (File.Exists(configFile.FullName))
                {
                    // Open the file for reading
                    FileStream fs = null;
                    // Try hard to open the file
                    for (int retry = 5; --retry >= 0; )
                    {
                        try
                        {
                            fs = configFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                            break;
                        }
                        catch (IOException ex)
                        {
                            if (retry == 0)
                            {
                                ReportLog.Error(declaringType, "XmlConfigurator:Failed to open XML config file [" + configFile.Name + "]", ex);
                                // The stream cannot be valid
                                fs = null;
                            }
                            System.Threading.Thread.Sleep(250);
                        }
                    }
                    //处理文件流
                    if (fs != null)
                    {
                        try
                        {
                            //调用内部处理文件流方法
                            InternalConfigure(repository, fs);
                        }
                        finally
                        {
                            // Force the file closed whatever happens
                            fs.Close();
                        }
                    }
                }
                else
                {
                    ReportLog.Debug(declaringType, "XmlConfigurator:config file [" + configFile.FullName + "] not found. Configuration unchanged.");
                }
            }
        }
        private static void InternalConfigure(IReporterRepository repository, Uri configUri)
        {
            ReportLog.Debug(declaringType, "XmlConfigurator:configuring repository [" + repository.RepositoryName + "] using URI [" + configUri + "]");
            if (configUri == null)
            {
                ReportLog.Error(declaringType, "XmlConfigurator:Configure called with null 'configUri' parameter");
            }
            else
            {
                if (configUri.IsFile)
                {
                    // If URI is local file then call Configure with FileInfo
                    InternalConfigure(repository, new FileInfo(configUri.LocalPath));
                }
                else
                {
                    // NETCF dose not support WebClient
                    WebRequest configRequest = null;
                    try
                    {
                        configRequest = WebRequest.Create(configUri);
                    }
                    catch (Exception ex)
                    {
                        ReportLog.Error(declaringType, "XmlConfigurator:Failed to create WebRequest for URI [" + configUri + "]", ex);
                    }
                    if (configRequest != null)
                    {
#if !NETCF_1_0
                        // authentication may be required, set client to use default credentials
                        try
                        {
                            configRequest.Credentials = CredentialCache.DefaultCredentials;
                        }
                        catch
                        {
                            // ignore security exception
                        }
#endif
                        try
                        {
                            WebResponse response = configRequest.GetResponse();
                            if (response != null)
                            {
                                try
                                {
                                    // Open stream on config URI
                                    using (Stream configStream = response.GetResponseStream())
                                    {
                                        InternalConfigure(repository, configStream);
                                    }
                                }
                                finally
                                {
                                    response.Close();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ReportLog.Error(declaringType, "XmlConfigurator:Failed to request config from URI [" + configUri + "]", ex);
                        }
                    }
                }
            }
        }
        
        private static void InternalConfigureAndWatch(IReporterRepository repository, FileInfo configFile)
        {
            ReportLog.Debug(declaringType, "XmlConfigurator:configuring repository [" + repository.RepositoryName + "] using file [" + configFile + "] watching for file updates");
            if (configFile == null)
            {
                ReportLog.Error(declaringType, "XmlConfigurator:ConfigureAndWatch called with null 'configFile' parameter");
            }
            else
            {
                // 先根据configFile配置数据
                InternalConfigure(repository, configFile);
                try
                {
                    lock (m_repositoryName2ConfigAndWatchHandler)
                    {
                        //根据配置文件名获取handler
                        ConfigureAndWatchHandler handler =(ConfigureAndWatchHandler)m_repositoryName2ConfigAndWatchHandler[configFile.FullName];
                        if (handler != null)
                        {
                            //若存在则移除此handler
                            m_repositoryName2ConfigAndWatchHandler.Remove(configFile.FullName);
                            handler.Dispose();
                        }
                        //加载配置文件时创建一个监控，并启用
                        handler = new ConfigureAndWatchHandler(repository, configFile);
                        m_repositoryName2ConfigAndWatchHandler[configFile.FullName] = handler;
                    }
                }
                catch (Exception ex)
                {
                    ReportLog.Error(declaringType, "Failed to initialize configuration file watcher for file [" + configFile.FullName + "]", ex);
                }
            }
        }
        #endregion

        #region ConfigureAndWatchHandler
        private sealed class ConfigureAndWatchHandler : IDisposable
        {
            private FileInfo m_configFile;
            //处理的信息
            private IReporterRepository m_repository;
            private Timer m_timer;
            //检测时间间隔
            private const int TimeoutMillis = 500;
            //监控
            private FileSystemWatcher m_watcher;

            [System.Security.SecuritySafeCritical]
            public ConfigureAndWatchHandler(IReporterRepository repository, FileInfo configFile)
            {
                m_repository = repository;
                m_configFile = configFile;
                // Create a new FileSystemWatcher and set its properties.
                m_watcher = new FileSystemWatcher();

                //设置监控的文件
                m_watcher.Path = m_configFile.DirectoryName;
                m_watcher.Filter = m_configFile.Name;

                // Set the notification filters
                m_watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName;
                // Add event handlers. OnChanged will do for all event handlers that fire a FileSystemEventArgs
                //
                m_watcher.Changed += new FileSystemEventHandler(ConfigureAndWatchHandler_OnChanged);
                m_watcher.Created += new FileSystemEventHandler(ConfigureAndWatchHandler_OnChanged);
                m_watcher.Deleted += new FileSystemEventHandler(ConfigureAndWatchHandler_OnChanged);
                m_watcher.Renamed += new RenamedEventHandler(ConfigureAndWatchHandler_OnRenamed);

                //开始监控
                m_watcher.EnableRaisingEvents = true;

                m_timer = new Timer(new TimerCallback(OnWatchedFileChange), null, Timeout.Infinite, Timeout.Infinite);
            }
            private void ConfigureAndWatchHandler_OnChanged(object source, FileSystemEventArgs e)
            {
                ReportLog.Debug(declaringType, "ConfigureAndWatchHandler: " + e.ChangeType + " [" + m_configFile.FullName + "]");
                m_timer.Change(TimeoutMillis, Timeout.Infinite);
            }
            private void ConfigureAndWatchHandler_OnRenamed(object source, RenamedEventArgs e)
            {
                ReportLog.Debug(declaringType, "ConfigureAndWatchHandler: " + e.ChangeType + " [" + m_configFile.FullName + "]");
                m_timer.Change(TimeoutMillis, Timeout.Infinite);
            }
            private void OnWatchedFileChange(object state)
            {
                XmlConfigurator.InternalConfigure(m_repository, m_configFile);
            }
            [System.Security.SecuritySafeCritical]
            public void Dispose()
            {
                m_watcher.EnableRaisingEvents = false;
                m_watcher.Dispose();
                m_timer.Dispose();
            }
        }
        #endregion
    }
}
