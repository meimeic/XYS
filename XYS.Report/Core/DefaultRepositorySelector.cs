using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;
using XYS.Report.Config;
using XYS.Report.Repository;
namespace XYS.Report.Core
{
    public class DefaultRepositorySelector : IRepositorySelector
    {
        #region 静态成员
        private static readonly string DefaultRepositoryName = "default-repository";
        private static readonly Type declaringType = typeof(DefaultRepositorySelector);
        #endregion

        #region 指读实例成员
        //默认的Repository类型
        private readonly Type m_defaultRepositoryType;
        private readonly Hashtable m_assembly2repositoryMap = new Hashtable(3);
        private readonly Hashtable m_name2repositoryMap = new Hashtable(2);
        #endregion

        #region 事件成员
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
            ConsoleInfo.Debug(declaringType, "defaultRepositoryType [" + m_defaultRepositoryType + "]");
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
            return CreateRepository(assembly, repositoryType, DefaultRepositoryName, true);
        }
        public IReporterRepository CreateRepository(string repositoryName, Type repositoryType)
        {
            if (repositoryName == null)
            {
                throw new ArgumentNullException("repositoryName");
            }
            if (repositoryType == null)
            {
                repositoryType = m_defaultRepositoryType;
            }
            lock (this)
            {
                IReporterRepository rep = null;

                //先检查m_name2repositoryMap中是否存在相应记录
                rep = m_name2repositoryMap[repositoryName] as IReporterRepository;
                if (rep != null)
                {
                    //存在，抛出异常
                    throw new ReportException("Repository [" + repositoryName + "] is already defined. Repositories cannot be redefined.");
                }
                ConsoleInfo.Debug(declaringType, "Creating repository [" + repositoryName + "] using type [" + repositoryType + "]");
                //创建repository实例
                rep = (IReporterRepository)Activator.CreateInstance(repositoryType);
                //设置repository的name属性
                rep.RepositoryName = repositoryName;
                //将repository实例加入map
                m_name2repositoryMap[repositoryName] = rep;
                //通知创建repository事件
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
        //创建repository
        public IReporterRepository CreateRepository(Assembly repositoryAssembly, Type repositoryType, string repositoryName, bool readAssemblyAttributes)
        {
            //使用这个repository的程序集不能为null
            if (repositoryAssembly == null)
            {
                throw new ArgumentNullException("repositoryAssembly");
            }
            // repositoryType为null 则将其置为默认值
            if (repositoryType == null)
            {
                repositoryType = m_defaultRepositoryType;
            }
            //锁定该对象
            lock (this)
            {
                // 查看assembly2repositoryMap 是否存在此repository
                IReporterRepository rep = m_assembly2repositoryMap[repositoryAssembly] as IReporterRepository;
                //在m_assembly2repositoryMap中不存在
                if (rep == null)
                {
                    ConsoleInfo.Debug(declaringType, "Creating repository for assembly [" + repositoryAssembly + "]");
                    //设置实际名称的、及实际类型，并初始化
                    string actualRepositoryName = repositoryName;
                    Type actualRepositoryType = repositoryType;
                    //判断是否可以读取程序集特性
                    if (readAssemblyAttributes)
                    {
                        //通过程序集的特性设置RepositoryName、RepositoryType。
                        GetInfoForAssembly(repositoryAssembly, ref actualRepositoryName, ref actualRepositoryType);
                    }
                    ConsoleInfo.Debug(declaringType, "Assembly [" + repositoryAssembly + "] using repository [" + actualRepositoryName + "] and repository type [" + actualRepositoryType + "]");

                    // 在name2repositoryMap中查找是否存在repository
                    rep = m_name2repositoryMap[actualRepositoryName] as IReporterRepository;
                    //在m_name2repositoryMap不存在，则创建
                    if (rep == null)
                    {
                        rep = CreateRepository(actualRepositoryName, actualRepositoryType);
                        //根据程序集特性对Repository进行配置
                        if (readAssemblyAttributes)
                        {
                            try
                            {
                                ConfigureRepository(repositoryAssembly, rep);
                            }
                            catch (Exception ex)
                            {
                                ConsoleInfo.Error(declaringType, "Failed to configure repository [" + actualRepositoryName + "] from assembly attributes.", ex);
                            }
                        }
                    }
                    //在m_name2repositoryMap存在
                    else
                    {
                        ConsoleInfo.Debug(declaringType, "repository [" + actualRepositoryName + "] already exists, using repository type [" + rep.GetType().FullName + "]");
                        if (readAssemblyAttributes)
                        {
                            try
                            {
                                //目前什么都不做
                                // Look for plugins defined on the assembly
                                //--- LoadPlugins(repositoryAssembly, rep);
                            }
                            catch (Exception ex)
                            {
                                ConsoleInfo.Error(declaringType, "Failed to configure repository [" + actualRepositoryName + "] from assembly attributes.", ex);
                            }
                        }
                    }
                    //将repository添加到m_assembly2repositoryMap
                    m_assembly2repositoryMap[repositoryAssembly] = rep;
                }
                //返回
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
                //
                ConsoleInfo.Debug(declaringType, "Assembly [" + assembly.FullName + "] Loaded From [" + SystemInfo.AssemblyLocationInfo(assembly) + "]");
            }
            catch
            {
                // Ignore exception from debug call
            }
            try
            {
                // 查找程序集中的RepositoryAttribute
                object[] repositoryAttributes = Attribute.GetCustomAttributes(assembly, typeof(RepositoryAttribute), false);
                if (repositoryAttributes == null || repositoryAttributes.Length == 0)
                {
                    //不存在此特性（默认值不变）
                    ConsoleInfo.Debug(declaringType, "Assembly [" + assembly + "] does not have a RepositoryAttribute specified.");
                }
                else
                {
                    //存在多个此特性（当然可以通过设置特性来避免这种情况）
                    if (repositoryAttributes.Length > 1)
                    {
                        ConsoleInfo.Error(declaringType, "Assembly [" + assembly + "] has multiple XYS.Report.Config.RepositoryAttribute assembly attributes. Only using first occurrence.");
                    }
                    //获取程序集指定的特性
                    RepositoryAttribute domAttr = repositoryAttributes[0] as RepositoryAttribute;
                    if (domAttr == null)
                    {
                        ConsoleInfo.Error(declaringType, "Assembly [" + assembly + "] has a RepositoryAttribute but it does not!.");
                    }
                    //根据特性设置相关值
                    else
                    {
                        //设置repositoryName
                        if (domAttr.Name != null)
                        {
                            repositoryName = domAttr.Name;
                        }
                        //设置RepositoryType
                        if (domAttr.RepositoryType != null)
                        {
                            //判断type类型是否为合法的type类型
                            if (typeof(IReporterRepository).IsAssignableFrom(domAttr.RepositoryType))
                            {
                                repositoryType = domAttr.RepositoryType;
                            }
                            else
                            {
                                ConsoleInfo.Error(declaringType, " Repository Type [" + domAttr.RepositoryType + "] must implement the ILoggerRepository interface.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleInfo.Error(declaringType, "Unhandled exception in GetInfoForAssembly", ex);
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
                            ConsoleInfo.Debug(declaringType, "Configure Repository using assembly attribute [" + configAttr.GetType().Name + "]");
                            configAttr.Configure(assembly, repository);
                        }
                        catch (Exception ex)
                        {
                            ConsoleInfo.Error(declaringType, "Exception calling [" + configAttr.GetType().FullName + "] .Configure method.", ex);
                        }
                    }
                }
            }
            //对于默认RepositoryName的repository还需要用指定的.config 配置文件进行配置（可能覆盖特性的配置）
            if (repository.RepositoryName == DefaultRepositoryName)
            {
                //获取相关的配置文件名
                string repositoryConfigFile = SystemInfo.GetAppSetting("xys-report.Config");
                if (repositoryConfigFile != null && repositoryConfigFile.Length > 0)
                {
                    //使用AppSettings指定的配置文件进行配置
                    ConsoleInfo.Debug(declaringType, "Try to configure the default repository using an AppSettings specified config file");
                    string applicationBaseDirectory = null;
                    try
                    {
                        applicationBaseDirectory = SystemInfo.ApplicationBaseDirectory;
                    }
                    catch (Exception ex)
                    {
                        ConsoleInfo.Warn(declaringType, "Exception getting ApplicationBaseDirectory. appSettings lis-report.Config path [" + repositoryConfigFile + "] will be treated as an absolute URI", ex);
                    }
                    //获取配置文件全路径
                    string repositoryConfigFilePath = repositoryConfigFile;
                    if (applicationBaseDirectory != null)
                    {
                        repositoryConfigFilePath = Path.Combine(applicationBaseDirectory, repositoryConfigFile);
                    }
                    //是否观测文件变化（根据AppSettings获取监控文件标识）
                    bool watchRepositoryConfigFile = false;
                    {
                        string watch = SystemInfo.GetAppSetting("xys-report.Config.Watch");
                        if (watch != null && watch.Length > 0)
                        {
                            try
                            {
                                watchRepositoryConfigFile = Boolean.Parse(watch);
                            }
                            catch (FormatException ex)
                            {
                                //无法将string转换成bool
                            }
                        }
                    }
                    //监控配置文件
                    if (watchRepositoryConfigFile)
                    {
                        FileInfo repositoryConfigFileInfo = null;
                        try
                        {
                            repositoryConfigFileInfo = new FileInfo(repositoryConfigFilePath);
                        }
                        catch (Exception ex)
                        {
                            ConsoleInfo.Error(declaringType, " Exception while parsing lis-report.Config file physical path [" + repositoryConfigFilePath + "]", ex);
                        }
                        try
                        {
                            ConsoleInfo.Debug(declaringType, "Loading and watching configuration for default repository from AppSettings specified Config path [" + repositoryConfigFilePath + "]");
                            //根据文件配置repository，并监控文件的变化
                            XmlConfigurator.ConfigureAndWatch(repository, repositoryConfigFileInfo);
                        }
                        catch (Exception ex)
                        {
                            ConsoleInfo.Error(declaringType, " Exception calling XmlConfigurator.ConfigureAndWatch method with ConfigFilePath [" + repositoryConfigFilePath + "]", ex);
                        }
                    }
                    //不监控
                    else
                    {
                        //不监控可以将其作为uri文件处理
                        Uri repositoryConfigUri = null;
                        try
                        {
                            repositoryConfigUri = new Uri(repositoryConfigFilePath);
                        }
                        catch (Exception ex)
                        {
                            ConsoleInfo.Error(declaringType, "Exception while parsing lis-report.Config file path [" + repositoryConfigFile + "]", ex);
                        }

                        if (repositoryConfigUri != null)
                        {
                            ConsoleInfo.Debug(declaringType, "Loading configuration for default repository from AppSettings specified Config URI [" + repositoryConfigUri.ToString() + "]");

                            try
                            {
                                //配置uri文件
                                XmlConfigurator.Configure(repository, repositoryConfigUri);
                            }
                            catch (Exception ex)
                            {
                                ConsoleInfo.Error(declaringType, "Exception calling XmlConfigurator.Configure method with ConfigUri [" + repositoryConfigUri + "]", ex);
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
