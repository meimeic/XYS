using System;
using System.Collections;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Net;
using System.Threading;

using XYS.Lis.Repository;
using XYS.Lis.Util;
using XYS.Lis;
using XYS.Lis.Core;
using XYS.Lis.Model;
namespace XYS.Lis.Config
{
    public sealed class XmlConfigurator
    {

        #region
        private static readonly Type declaringType = typeof(XmlConfigurator);
        private static readonly Hashtable m_repositoryName2ConfigAndWatchHandler = new Hashtable();
        #endregion
        
        #region 私有构造函数
        private XmlConfigurator()
        {
        }
        #endregion

        #region
        public static ICollection Configure()
        {
            return Configure(ReportManager.GetRepository(Assembly.GetCallingAssembly()));
        }
        public static ICollection Configure(IReporterRepository repository)
        {
            ArrayList configurationMessages = new ArrayList();
            using (new ReportReport.ReportReceivedAdapter(configurationMessages))
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
            using (new ReportReport.ReportReceivedAdapter(configurationMessages))
            {
                InternalConfigureFromXml(repository, element);
            }
            repository.ConfigurationMessages = configurationMessages;
            return configurationMessages;
        }
        public static ICollection Configure(IReporterRepository repository, XmlElement element)
        {
            ArrayList configurationMessages = new ArrayList();
            using (new ReportReport.ReportReceivedAdapter(configurationMessages))
            {
                ReportReport.Debug(declaringType, "XmlConfigurator:configuring repository [" + repository.RepositoryName + "] using XML element");
                InternalConfigureFromXml(repository, element);
            }
            repository.ConfigurationMessages = configurationMessages;
            return configurationMessages;
        }
        public static ICollection Configure(FileInfo configFile)
        {
            ArrayList configurationMessages = new ArrayList();
            using (new ReportReport.ReportReceivedAdapter(configurationMessages))
            {
                InternalConfigure(ReportManager.GetRepository(Assembly.GetCallingAssembly()), configFile);
            }
            return configurationMessages;
        }
        public static ICollection Configure(IReporterRepository repository, FileInfo configFile)
        {
            ArrayList configurationMessages = new ArrayList();
            using (new ReportReport.ReportReceivedAdapter(configurationMessages))
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
            using (new ReportReport.ReportReceivedAdapter(configurationMessages))
            {
                InternalConfigure(repository, configUri);
            }
            repository.ConfigurationMessages = configurationMessages;
            return configurationMessages;
        }
        public static ICollection Configure(IReporterRepository repository, Uri configUri)
        {
            ArrayList configurationMessages = new ArrayList();
            using (new ReportReport.ReportReceivedAdapter(configurationMessages))
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
            using (new ReportReport.ReportReceivedAdapter(configurationMessages))
            {
                InternalConfigure(repository, configStream);
            }
            repository.ConfigurationMessages = configurationMessages;
            return configurationMessages;
        }
        public static ICollection Configure(IReporterRepository repository, Stream configStream)
        {
            ArrayList configurationMessages = new ArrayList();
            using (new ReportReport.ReportReceivedAdapter(configurationMessages))
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
            using (new ReportReport.ReportReceivedAdapter(configurationMessages))
            {
                InternalConfigureAndWatch(repository, configFile);
            }
            repository.ConfigurationMessages = configurationMessages;
            return configurationMessages;
        }
        public static ICollection ConfigureAndWatch(IReporterRepository repository, FileInfo configFile)
        {
            ArrayList configurationMessages = new ArrayList();

            using (new ReportReport.ReportReceivedAdapter(configurationMessages))
            {
                InternalConfigureAndWatch(repository, configFile);
            }
            repository.ConfigurationMessages = configurationMessages;
            return configurationMessages;
        }
        #endregion

        #region
        public static XmlElement GetParamConfigurationElement()
        {
            ReportReport.Debug(declaringType, "XmlConfigurator:getting lis-param section");
            try
            {
                ReportReport.Debug(declaringType, "XmlConfigurator:Application config file is [" + SystemInfo.ConfigurationFileLocation + "]");
            }
            catch
            {
                // ignore error
                ReportReport.Debug(declaringType, "XmlConfigurator:Application config file location unknown");
            }
            try
            {
                XmlElement configElement = null;
                configElement = System.Configuration.ConfigurationManager.GetSection("lis-param") as XmlElement;
                if (configElement == null)
                {
                    ReportReport.Error(declaringType, "XmlConfigurator:Failed to find configuration section 'lis-param' in the application's .config file. Check your .config file for the <lis-param> and <configSections> elements. The configuration section should look like: <section name=\"lis-param\" type=\"XYS.Lis.Config.ParamSectionHandler,XYS.Lis\" />");
                    return null;
                }
                else
                {
                    ReportReport.Debug(declaringType, "XmlConfigurator:getted lis-param section");
                    XmlDocument newDoc = new XmlDocument();
                    XmlElement newElement = (XmlElement)newDoc.AppendChild(newDoc.ImportNode(configElement, true));
                    return newElement;
                }
            }
            catch (System.Configuration.ConfigurationException confEx)
            {
                if (confEx.BareMessage.IndexOf("Unrecognized element") >= 0)
                {
                    // Looks like the XML file is not valid
                    ReportReport.Error(declaringType, "XmlConfigurator:Failed to parse config file. Check your .config file is well formed XML.", confEx);
                }
                else
                {
                    // This exception is typically due to the assembly name not being correctly specified in the section type.
                    string configSectionStr = "<section name=\"lis-param\" type=\"XYS.Lis.Config.ParamSectionHandler," + Assembly.GetExecutingAssembly().FullName + "\" />";
                    ReportReport.Error(declaringType, "XmlConfigurator:Failed to parse config file. Is the <configSections> specified as: " + configSectionStr, confEx);
                }
                return null;
            }
        }
        #endregion

        #region
        private static void InternalConfigure(IReporterRepository repository)
        {
            ReportReport.Debug(declaringType, "XmlConfigurator:configuring repository [" + repository.RepositoryName + "] using .config file section");
            try
            {
                ReportReport.Debug(declaringType, "XmlConfigurator:Application config file is [" + SystemInfo.ConfigurationFileLocation + "]");
            }
            catch
            {
                // ignore error
                ReportReport.Debug(declaringType, "XmlConfigurator:Application config file location unknown");
            }
#if NETCF
			// No config file reading stuff. Just go straight for the file
			Configure(repository, new FileInfo(SystemInfo.ConfigurationFileLocation));
#else
            try
            {
                XmlElement configElement = null;
#if NET_2_0
				configElement = System.Configuration.ConfigurationManager.GetSection("log4net") as XmlElement;
#else
                //configElement = System.Configuration.ConfigurationSettings.GetConfig("log4net") as XmlElement;
                configElement = System.Configuration.ConfigurationManager.GetSection("lis-report") as XmlElement;
#endif
                if (configElement == null)
                {
                    // Failed to load the xml config using configuration settings handler
                    ReportReport.Error(declaringType, "XmlConfigurator:Failed to find configuration section 'lis-report' in the application's .config file. Check your .config file for the <lis-report> and <configSections> elements. The configuration section should look like: <section name=\"lis-report\" type=\"XYS.Lis.Config.ReportSectionHandler,XYS.Lis\" />");
                }
                else
                {
                    //Configure using the xml loaded from the config file
                    InternalConfigureFromXml(repository, configElement);
                }
            }
            catch (System.Configuration.ConfigurationException confEx)
            {
                if (confEx.BareMessage.IndexOf("Unrecognized element") >= 0)
                {
                    // Looks like the XML file is not valid
                    ReportReport.Error(declaringType, "XmlConfigurator:Failed to parse config file. Check your .config file is well formed XML.", confEx);
                }
                else
                {
                    // This exception is typically due to the assembly name not being correctly specified in the section type.
                    string configSectionStr = "<section name=\"lis-report\" type=\"XYS.Lis.Config.ConfigurationSectionHandler," + Assembly.GetExecutingAssembly().FullName + "\" />";
                    ReportReport.Error(declaringType, "XmlConfigurator:Failed to parse config file. Is the <configSections> specified as: " + configSectionStr, confEx);
                }
            }
#endif
        }
        private static void InternalConfigure(IReporterRepository repository, Stream configStream)
        {
            ReportReport.Debug(declaringType, "XmlConfigurator:configuring repository [" + repository.RepositoryName + "] using stream");
            if (configStream == null)
            {
                ReportReport.Error(declaringType, "XmlConfigurator:Configure called with null 'configStream' parameter");
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
                    //XmlValidatingReader xmlReader = new XmlValidatingReader(new XmlTextReader(configStream));
                    // Specify that the reader should not perform validation, but that it should
                    // expand entity refs.
                    //xmlReader.ValidationType = ValidationType.None;
                   // xmlReader.EntityHandling = EntityHandling.ExpandEntities;
#endif
                    // load the data into the document
                    doc.Load(xmlReader);
                }
                catch (Exception ex)
                {
                    ReportReport.Error(declaringType, "XmlConfigurator:Error while loading XML configuration", ex);
                    // The document is invalid
                    doc = null;
                }

                if (doc != null)
                {
                    ReportReport.Debug(declaringType, "XmlConfigurator:loading XML configuration");

                    // Configure using the 'lis-report' element
                    XmlNodeList configNodeList = doc.GetElementsByTagName("lis-report");
                    if (configNodeList.Count == 0)
                    {
                        ReportReport.Debug(declaringType, "XmlConfigurator:XML configuration does not contain a <lis-report> element. Configuration Aborted.");
                    }
                    else if (configNodeList.Count > 1)
                    {
                        ReportReport.Error(declaringType, "XmlConfigurator:XML configuration contains [" + configNodeList.Count + "] <lis-report> elements. Only one is allowed. Configuration Aborted.");
                    }
                    else
                    {
                        InternalConfigureFromXml(repository, configNodeList[0] as XmlElement);
                    }
                }
            }
        }
        private static void InternalConfigureFromXml(IReporterRepository repository, XmlElement element)
        {
            if (element == null)
            {
                ReportReport.Error(declaringType, "XmlConfigurator:ConfigureFromXml called with null 'element' parameter");
            }
            else if (repository == null)
            {
                ReportReport.Error(declaringType, "XmlConfigurator:ConfigureFromXml called with null 'repository' parameter");
            }
            else
            {
                ReportReport.Debug(declaringType, "XmlConfigurator:Configuring Repository [" + repository.RepositoryName + "] using XML element");
                IXmlRepositoryConfigurator configurableRepository = repository as IXmlRepositoryConfigurator;
                if (configurableRepository == null)
                {
                    //不可配置
                    ReportReport.Warn(declaringType, "XmlConfigurator:Repository [" + repository + "] does not support the XmlConfigurator");
                }
                else
                {
                    XmlDocument newDoc = new XmlDocument();
                    XmlElement newElement = (XmlElement)newDoc.AppendChild(newDoc.ImportNode(element, true));
                    configurableRepository.Configure(newElement);
                }
            }
        }
        private static void InternalConfigure(IReporterRepository repository, FileInfo configFile)
        {
            ReportReport.Debug(declaringType, "XmlConfigurator:configuring repository [" + repository.RepositoryName + "] using file [" + configFile + "]");

            if (configFile == null)
            {
                ReportReport.Error(declaringType, "XmlConfigurator:Configure called with null 'configFile' parameter");
            }
            else
            {
                // Have to use File.Exists() rather than configFile.Exists()
                // because configFile.Exists() caches the value, not what we want.
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
                                ReportReport.Error(declaringType, "XmlConfigurator:Failed to open XML config file [" + configFile.Name + "]", ex);
                                // The stream cannot be valid
                                fs = null;
                            }
                            System.Threading.Thread.Sleep(250);
                        }
                    }
                    if (fs != null)
                    {
                        try
                        {
                            // Load the configuration from the stream
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
                    ReportReport.Debug(declaringType, "XmlConfigurator:config file [" + configFile.FullName + "] not found. Configuration unchanged.");
                }
            }
        }
        private static void InternalConfigure(IReporterRepository repository, Uri configUri)
        {
            ReportReport.Debug(declaringType, "XmlConfigurator:configuring repository [" + repository.RepositoryName + "] using URI [" + configUri + "]");
            if (configUri == null)
            {
                ReportReport.Error(declaringType, "XmlConfigurator:Configure called with null 'configUri' parameter");
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
                        ReportReport.Error(declaringType, "XmlConfigurator:Failed to create WebRequest for URI [" + configUri + "]", ex);
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
                            ReportReport.Error(declaringType, "XmlConfigurator:Failed to request config from URI [" + configUri + "]", ex);
                        }
                    }
                }
            }
        }
        private static void InternalConfigureAndWatch(IReporterRepository repository, FileInfo configFile)
        {
            ReportReport.Debug(declaringType, "XmlConfigurator:configuring repository [" + repository.RepositoryName + "] using file [" + configFile + "] watching for file updates");

            if (configFile == null)
            {
                ReportReport.Error(declaringType, "XmlConfigurator:ConfigureAndWatch called with null 'configFile' parameter");
            }
            else
            {
                // Configure log4net now
                InternalConfigure(repository, configFile);
                try
                {
                    lock (m_repositoryName2ConfigAndWatchHandler)
                    {
                        // support multiple repositories each having their own watcher
                        ConfigureAndWatchHandler handler =(ConfigureAndWatchHandler)m_repositoryName2ConfigAndWatchHandler[configFile.FullName];
                        if (handler != null)
                        {
                            m_repositoryName2ConfigAndWatchHandler.Remove(configFile.FullName);
                            handler.Dispose();
                        }
                        // Create and start a watch handler that will reload the
                        // configuration whenever the config file is modified.
                        handler = new ConfigureAndWatchHandler(repository, configFile);
                        m_repositoryName2ConfigAndWatchHandler[configFile.FullName] = handler;
                    }
                }
                catch (Exception ex)
                {
                    ReportReport.Error(declaringType, "Failed to initialize configuration file watcher for file [" + configFile.FullName + "]", ex);
                }
            }
        }
        #endregion

        #region ConfigureAndWatchHandler
        private sealed class ConfigureAndWatchHandler : IDisposable
        {
            private FileInfo m_configFile;
            private IReporterRepository m_repository;
            private Timer m_timer;
            private const int TimeoutMillis = 500;
            private FileSystemWatcher m_watcher;

            [System.Security.SecuritySafeCritical]
            public ConfigureAndWatchHandler(IReporterRepository repository, FileInfo configFile)
            {
                m_repository = repository;
                m_configFile = configFile;

                // Create a new FileSystemWatcher and set its properties.
                m_watcher = new FileSystemWatcher();
                
                m_watcher.Path = m_configFile.DirectoryName;
                m_watcher.Filter = m_configFile.Name;

                // Set the notification filters
                m_watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName;
                // Add event handlers. OnChanged will do for all event handlers that fire a FileSystemEventArgs
                m_watcher.Changed += new FileSystemEventHandler(ConfigureAndWatchHandler_OnChanged);
                m_watcher.Created += new FileSystemEventHandler(ConfigureAndWatchHandler_OnChanged);
                m_watcher.Deleted += new FileSystemEventHandler(ConfigureAndWatchHandler_OnChanged);
                m_watcher.Renamed += new RenamedEventHandler(ConfigureAndWatchHandler_OnRenamed);

                // Begin watching.
                m_watcher.EnableRaisingEvents = true;

                // Create the timer that will be used to deliver events. Set as disabled
                m_timer = new Timer(new TimerCallback(OnWatchedFileChange), null, Timeout.Infinite, Timeout.Infinite);
            }

            /// <summary>
            /// Event handler used by <see cref="ConfigureAndWatchHandler"/>.
            /// </summary>
            /// <param name="source">The <see cref="FileSystemWatcher"/> firing the event.</param>
            /// <param name="e">The argument indicates the file that caused the event to be fired.</param>
            /// <remarks>
            /// <para>
            /// This handler reloads the configuration from the file when the event is fired.
            /// </para>
            /// </remarks>
            private void ConfigureAndWatchHandler_OnChanged(object source, FileSystemEventArgs e)
            {
                ReportReport.Debug(declaringType, "ConfigureAndWatchHandler: " + e.ChangeType + " [" + m_configFile.FullName + "]");

                // Deliver the event in TimeoutMillis time
                // timer will fire only once
                m_timer.Change(TimeoutMillis, Timeout.Infinite);
            }

            /// <summary>
            /// Event handler used by <see cref="ConfigureAndWatchHandler"/>.
            /// </summary>
            /// <param name="source">The <see cref="FileSystemWatcher"/> firing the event.</param>
            /// <param name="e">The argument indicates the file that caused the event to be fired.</param>
            /// <remarks>
            /// <para>
            /// This handler reloads the configuration from the file when the event is fired.
            /// </para>
            /// </remarks>
            private void ConfigureAndWatchHandler_OnRenamed(object source, RenamedEventArgs e)
            {
                ReportReport.Debug(declaringType, "ConfigureAndWatchHandler: " + e.ChangeType + " [" + m_configFile.FullName + "]");
                // Deliver the event in TimeoutMillis time
                // timer will fire only once
                m_timer.Change(TimeoutMillis, Timeout.Infinite);
            }

            /// <summary>
            /// Called by the timer when the configuration has been updated.
            /// </summary>
            /// <param name="state">null</param>
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
