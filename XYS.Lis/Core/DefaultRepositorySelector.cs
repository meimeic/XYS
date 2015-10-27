using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

using XYS.Lis.Util;
using XYS.Lis.Repository;
using XYS.Lis.Config;
namespace XYS.Lis.Core
{
    public class DefaultRepositorySelector:IRepositorySelector
    {
        #region
        private static readonly string DefaultRepositoryName = "reporter-default-repository";
        private static readonly Type declaringType = typeof(DefaultRepositorySelector);
        #endregion

        #region
        private readonly Type m_defaultRepositoryType;
        private readonly Hashtable m_assembly2repositoryMap = new Hashtable();
        private readonly Hashtable m_name2repositoryMap = new Hashtable();
        #endregion

        #region
        private event ReporterRepositoryCreationEventHandler m_reporterRepositoryCreatedEvent;
        #endregion

        #region 构造函数
        public DefaultRepositorySelector(Type defaultRepositoryType)
        {
            if (defaultRepositoryType == null)
            {
                throw new ArgumentNullException("defaultRepositoryType");
            }
            // Check that the type is a repository
            if (!(typeof(IReporterRepository).IsAssignableFrom(defaultRepositoryType)))
            {
                throw SystemInfo.CreateArgumentOutOfRangeException("defaultRepositoryType", defaultRepositoryType, "Parameter: defaultRepositoryType, Value: [" + defaultRepositoryType + "] out of range. Argument must implement the ILoggerRepository interface");
            }
            m_defaultRepositoryType = defaultRepositoryType;
            ReportReport.Debug(declaringType, "defaultRepositoryType [" + m_defaultRepositoryType + "]");
        }
        #endregion
        #region  实现IRepositorySelector接口
        public IReporterRepository GetRepository(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("repositoryAssembly");
            }
            return CreateRepository(assembly, this.m_defaultRepositoryType);
        }

        public IReporterRepository GetRepository(string repositoryName)
        {
            if (repositoryName == null)
            {
                throw new ArgumentNullException("repositoryName");
            }
            lock (this)
            {
                // Lookup in map
                IReporterRepository rep = m_name2repositoryMap[repositoryName] as IReporterRepository;
                if (rep == null)
                {
                    //抛出异常
                   throw new ReportException("Repository [" + repositoryName + "] is NOT defined.");
                }
                return rep;
            }
        }

        public IReporterRepository CreateRepository(Assembly assembly, Type repositoryType)
        {
            return CreateRepository(assembly, repositoryType, DefaultRepositoryName,true);
        }

        public IReporterRepository CreateRepository(string repositoryName, Type repositoryType)
        {
            if (repositoryName == null)
            {
                throw new ArgumentNullException("repositoryName");
            }
            // If the type is not set then use the default type
            if (repositoryType == null)
            {
                repositoryType = m_defaultRepositoryType;
            }
            lock (this)
            {
                IReporterRepository rep = null;

                // First check that the repository does not exist
                rep = m_name2repositoryMap[repositoryName] as IReporterRepository;
                if (rep != null)
                {
                    //抛出异常
                    throw new ReportException("Repository [" + repositoryName + "] is already defined. Repositories cannot be redefined.");
                }
                //else
                //{
                //    // Lookup an alias before trying to create the new repository
                //    IReporterRepository aliasedRepository = m_alias2repositoryMap[repositoryName] as ILoggerRepository;
                //    if (aliasedRepository != null)
                //    {
                //        // Found an alias

                //        // Check repository type
                //        if (aliasedRepository.GetType() == repositoryType)
                //        {
                //            // Repository type is compatible
                //            LogLog.Debug(declaringType, "Aliasing repository [" + repositoryName + "] to existing repository [" + aliasedRepository.Name + "]");
                //            rep = aliasedRepository;

                //            // Store in map
                //            m_name2repositoryMap[repositoryName] = rep;
                //        }
                //        else
                //        {
                //            // Invalid repository type for alias
                //            LogLog.Error(declaringType, "Failed to alias repository [" + repositoryName + "] to existing repository [" + aliasedRepository.Name + "]. Requested repository type [" + repositoryType.FullName + "] is not compatible with existing type [" + aliasedRepository.GetType().FullName + "]");

                //            // We now drop through to create the repository without aliasing
                //        }
                //    }

                //    // If we could not find an alias
                //    if (rep == null)
                //    {
                ReportReport.Debug(declaringType, "Creating repository [" + repositoryName + "] using type [" + repositoryType + "]");
                // Call the no arg constructor for the repositoryType
                rep = (IReporterRepository)Activator.CreateInstance(repositoryType);
                // Set the name of the repository
                rep.RepositoryName = repositoryName;
                // Store in map
                m_name2repositoryMap[repositoryName] = rep;
                // Notify listeners that the repository has been created
                OnReporterRepositoryCreatedEvent(rep);
                return rep;
            }
        }
        
        public bool ExistsRepository(string repositoryName)
        {
            lock (this)
            {
                return m_name2repositoryMap.ContainsKey(repositoryName);
            }
        }

        public IReporterRepository[] GetAllRepositories()
        {
            lock (this)
            {
                ICollection reps = m_name2repositoryMap.Values;
                IReporterRepository[] all = new IReporterRepository[reps.Count];
                reps.CopyTo(all, 0);
                return all;
            }
        }

        public event ReporterRepositoryCreationEventHandler ReporterRepositoryCreationEvent
        {
            add { this.m_reporterRepositoryCreatedEvent += value; }
            remove { this.m_reporterRepositoryCreatedEvent -= value; }
        }
        #endregion

        #region
        public IReporterRepository CreateRepository(Assembly repositoryAssembly, Type repositoryType, string repositoryName, bool readAssemblyAttributes)
        {
            if (repositoryAssembly == null)
            {
                throw new ArgumentNullException("repositoryAssembly");
            }
            // If the type is not set then use the default type
            if (repositoryType == null)
            {
                repositoryType = m_defaultRepositoryType;
            }
            lock (this)
            {
                // Lookup in map
                IReporterRepository rep = m_assembly2repositoryMap[repositoryAssembly] as IReporterRepository;
                if (rep == null)
                {
                    // Not found, therefore create
                    ReportReport.Debug(declaringType, "Creating repository for assembly [" + repositoryAssembly + "]");

                    // Must specify defaults
                    string actualRepositoryName = repositoryName;
                    Type actualRepositoryType = repositoryType;

                    if (readAssemblyAttributes)
                    {
                        // Get the repository and type from the assembly attributes
                        GetInfoForAssembly(repositoryAssembly, ref actualRepositoryName, ref actualRepositoryType);
                    }

                    ReportReport.Debug(declaringType, "Assembly [" + repositoryAssembly + "] using repository [" + actualRepositoryName + "] and repository type [" + actualRepositoryType + "]");

                    // Lookup the repository in the map (as this may already be defined)
                    rep = m_name2repositoryMap[actualRepositoryName] as IReporterRepository;
                    if (rep == null)
                    {
                        // Create the repository
                        rep = CreateRepository(actualRepositoryName, actualRepositoryType);
                        if (readAssemblyAttributes)
                        {
                            try
                            {
                                // Look for aliasing attributes
                              //-----  LoadAliases(repositoryAssembly, rep);

                                // Look for plugins defined on the assembly
                               //---- LoadPlugins(repositoryAssembly, rep);

                                // Configure the repository using the assembly attributes
                                ConfigureRepository(repositoryAssembly, rep);
                            }
                            catch (Exception ex)
                            {
                                ReportReport.Error(declaringType, "Failed to configure repository [" + actualRepositoryName + "] from assembly attributes.", ex);
                            }
                        }
                    }
                    else
                    {
                        ReportReport.Debug(declaringType, "repository [" + actualRepositoryName + "] already exists, using repository type [" + rep.GetType().FullName + "]");
                        if (readAssemblyAttributes)
                        {
                            try
                            {
                                // Look for plugins defined on the assembly
                               //--- LoadPlugins(repositoryAssembly, rep);
                            }
                            catch (Exception ex)
                            {
                                ReportReport.Error(declaringType, "Failed to configure repository [" + actualRepositoryName + "] from assembly attributes.", ex);
                            }
                        }
                    }
                    m_assembly2repositoryMap[repositoryAssembly] = rep;
                    this.OnReporterRepositoryCreatedEvent(rep);
                }
                return rep;
            }
        }
        #endregion
        
        #region
        private void GetInfoForAssembly(Assembly assembly, ref string repositoryName, ref Type repositoryType)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            try
            {
                ReportReport.Debug(declaringType, "Assembly [" + assembly.FullName + "] Loaded From [" + SystemInfo.AssemblyLocationInfo(assembly) + "]");
            }
            catch
            {
                // Ignore exception from debug call
            }

            try
            {
                // Look for the RepositoryAttribute on the assembly 
                object[] repositoryAttributes = Attribute.GetCustomAttributes(assembly, typeof(RepositoryAttribute), false);
                if (repositoryAttributes == null || repositoryAttributes.Length == 0)
                {
                    // This is not a problem, but its nice to know what is going on.
                    ReportReport.Debug(declaringType, "Assembly [" + assembly + "] does not have a RepositoryAttribute specified.");
                }
                else
                {
                    if (repositoryAttributes.Length > 1)
                    {
                        ReportReport.Error(declaringType, "Assembly [" + assembly + "] has multiple XYS.Lis.Config.RepositoryAttribute assembly attributes. Only using first occurrence.");
                    }
                    RepositoryAttribute domAttr = repositoryAttributes[0] as RepositoryAttribute;
                    if (domAttr == null)
                    {
                        ReportReport.Error(declaringType, "Assembly [" + assembly + "] has a RepositoryAttribute but it does not!.");
                    }
                    else
                    {
                        // If the Name property is set then override the default
                        if (domAttr.Name != null)
                        {
                            repositoryName = domAttr.Name;
                        }

                        // If the RepositoryType property is set then override the default
                        if (domAttr.RepositoryType != null)
                        {
                            // Check that the type is a repository
                            if (typeof(IReporterRepository).IsAssignableFrom(domAttr.RepositoryType))
                            {
                                repositoryType = domAttr.RepositoryType;
                            }
                            else
                            {
                                ReportReport.Error(declaringType, "DefaultRepositorySelector: Repository Type [" + domAttr.RepositoryType + "] must implement the ILoggerRepository interface.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ReportReport.Error(declaringType, "Unhandled exception in GetInfoForAssembly", ex);
            }
        }
        private void ConfigureRepository(Assembly assembly, IReporterRepository repository)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            //通过特性配置
            // Look for the Configurator attributes (e.g. XmlConfiguratorAttribute) on the assembly
            object[] configAttributes = Attribute.GetCustomAttributes(assembly, typeof(ConfiguratorAttribute), false);
            if (configAttributes != null && configAttributes.Length > 0)
            {
                // Sort the ConfiguratorAttributes in priority order
                Array.Sort(configAttributes);

                // Delegate to the attribute the job of configuring the repository
                foreach (ConfiguratorAttribute configAttr in configAttributes)
                {
                    if (configAttr != null)
                    {
                        try
                        {
                            configAttr.Configure(assembly, repository);
                        }
                        catch (Exception ex)
                        {
                            ReportReport.Error(declaringType, "Exception calling [" + configAttr.GetType().FullName + "] .Configure method.", ex);
                        }
                    }
                }
            }
            //通过.config 配置文件进行配置
            if (repository.RepositoryName == DefaultRepositoryName)
            {
                // Try to configure the default repository using an AppSettings specified config file
                // Do this even if the repository has been configured (or claims to be), this allows overriding
                // of the default config files etc, if that is required.

                string repositoryConfigFile = SystemInfo.GetAppSetting("lis-report.Config");
                if (repositoryConfigFile != null && repositoryConfigFile.Length > 0)
                {
                    string applicationBaseDirectory = null;
                    try
                    {
                        applicationBaseDirectory = SystemInfo.ApplicationBaseDirectory;
                    }
                    catch (Exception ex)
                    {
                        ReportReport.Warn(declaringType, "Exception getting ApplicationBaseDirectory. appSettings log4net.Config path [" + repositoryConfigFile + "] will be treated as an absolute URI", ex);
                    }
                    string repositoryConfigFilePath = repositoryConfigFile;
                    if (applicationBaseDirectory != null)
                    {
                        repositoryConfigFilePath = Path.Combine(applicationBaseDirectory, repositoryConfigFile);
                    }
                    // Determine whether to watch the file or not based on an app setting value:
                    //是否观测文件变化
                    bool watchRepositoryConfigFile = false;
                    {
                        string watch = SystemInfo.GetAppSetting("lis-report.Config.Watch");
                        if (watch != null && watch.Length > 0)
                        {
                            try
                            {
                                watchRepositoryConfigFile = Boolean.Parse(watch);
                            }
                            catch (FormatException ex)
                            {
                                // simply not a Boolean
                            }
                        }
                    }
                    if (watchRepositoryConfigFile)
                    {
                        // As we are going to watch the config file it is required to resolve it as a 
                        // physical file system path pass that in a FileInfo object to the Configurator
                        FileInfo repositoryConfigFileInfo = null;
                        try
                        {
                            repositoryConfigFileInfo = new FileInfo(repositoryConfigFilePath);
                        }
                        catch (Exception ex)
                        {
                            ReportReport.Error(declaringType, "DefaultRepositorySelector: Exception while parsing lis-report.Config file physical path [" + repositoryConfigFilePath + "]", ex);
                        }
                        try
                        {
                            ReportReport.Debug(declaringType, "Loading and watching configuration for default repository from AppSettings specified Config path [" + repositoryConfigFilePath + "]");

                            XmlConfigurator.ConfigureAndWatch(repository, repositoryConfigFileInfo);
                        }
                        catch (Exception ex)
                        {
                            ReportReport.Error(declaringType, "DefaultRepositorySelector: Exception calling XmlConfigurator.ConfigureAndWatch method with ConfigFilePath [" + repositoryConfigFilePath + "]", ex);
                        }
                    }
                    else
                    {
                        // As we are not going to watch the config file it is easiest to just resolve it as a 
                        // URI and pass that to the Configurator
                        Uri repositoryConfigUri = null;
                        try
                        {
                            repositoryConfigUri = new Uri(repositoryConfigFilePath);
                        }
                        catch (Exception ex)
                        {
                            ReportReport.Error(declaringType, "Exception while parsing lis-report.Config file path [" + repositoryConfigFile + "]", ex);
                        }

                        if (repositoryConfigUri != null)
                        {
                            ReportReport.Debug(declaringType, "Loading configuration for default repository from AppSettings specified Config URI [" + repositoryConfigUri.ToString() + "]");

                            try
                            {
                                // TODO: Support other types of configurator
                                XmlConfigurator.Configure(repository, repositoryConfigUri);
                            }
                            catch (Exception ex)
                            {
                                ReportReport.Error(declaringType, "Exception calling XmlConfigurator.Configure method with ConfigUri [" + repositoryConfigUri + "]", ex);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region
        protected virtual void OnReporterRepositoryCreatedEvent(IReporterRepository repository)
        {
            ReporterRepositoryCreationEventHandler handler = this.m_reporterRepositoryCreatedEvent;
            if (handler != null)
            {
                handler(this, new ReporterRepositoryCreationEventArgs(repository));
            }
        }
        #endregion
    }
}
