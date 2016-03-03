using System;
using System.IO;
using System.Reflection;
using System.Collections;

using XYS.Util;
using XYS.Report.Repository;
namespace XYS.Report.Config
{
    [AttributeUsage(AttributeTargets.Assembly)]
    [Serializable]
    public class XmlConfiguratorAttribute : ConfiguratorAttribute
    {
        private string m_configFile = null;
        private string m_configFileExtension = null;
        private bool m_configureAndWatch = false;
        private readonly static Type declaringType = typeof(XmlConfiguratorAttribute);

        #region 构造函数
        public XmlConfiguratorAttribute()
            : base(0) /* configurator priority 0 */
        {
        }
        #endregion

        #region 特性的属性
        public string ConfigFile
        {
            get { return m_configFile; }
            set { m_configFile = value; }
        }
        public string ConfigFileExtension
        {
            get { return m_configFileExtension; }
            set { m_configFileExtension = value; }
        }
        public bool Watch
        {
            get { return m_configureAndWatch; }
            set { m_configureAndWatch = value; }
        }
        #endregion

        #region  实例方法
        public override void Configure(Assembly sourceAssembly, IReporterRepository targetRepository)
        {
            IList configurationMessages = new ArrayList();
            using (new ConsoleInfo.InfoReceivedAdapter(configurationMessages))
            {
                string applicationBaseDirectory = null;
                try
                {
                    applicationBaseDirectory = SystemInfo.ApplicationBaseDirectory;
                }
                catch (Exception ex)
                {
                }

                if (applicationBaseDirectory == null || (new Uri(applicationBaseDirectory)).IsFile)
                {
                    //资源路径为系统路径
                    ConfigureFromFile(sourceAssembly, targetRepository);
                }
                else
                {
                    //资源路径问网络路径
                    ConfigureFromUri(sourceAssembly, targetRepository);
                }
            }
            targetRepository.ConfigurationMessages = configurationMessages;
        }
        #endregion

        #region 私有方法
        private void ConfigureFromFile(Assembly sourceAssembly, IReporterRepository targetRepository)
        {
            // Work out the full path to the config file
            string fullPath2ConfigFile = null;

            // 使用特定的配置文件（若不存在配置文件扩展名， 则使用程序默认的配置文件）
            if (m_configFile == null || m_configFile.Length == 0)
            {
                //配置文件扩展名不存在，使用程序默认配置文件
                if (m_configFileExtension == null || m_configFileExtension.Length == 0)
                {
                    // Use the default .config file for the AppDomain
                    try
                    {
                        fullPath2ConfigFile = SystemInfo.ConfigurationFileLocation;
                    }
                    catch (Exception ex)
                    {
                        ConsoleInfo.Error(declaringType, "Exception getting ConfigurationFileLocation. Must be able to resolve ConfigurationFileLocation when ConfigFile and ConfigFileExtension properties are not set.", ex);
                        throw ex;
                    }
                }
                //使用特定扩展名的配置文件（文件名称按照通用规则生成）
                else
                {
                    // Force the extension to start with a '.'
                    if (m_configFileExtension[0] != '.')
                    {
                        m_configFileExtension = "." + m_configFileExtension;
                    }
                    string applicationBaseDirectory = null;
                    try
                    {
                        applicationBaseDirectory = SystemInfo.ApplicationBaseDirectory;
                    }
                    catch (Exception ex)
                    {
                        ConsoleInfo.Error(declaringType, "Exception getting ApplicationBaseDirectory. Must be able to resolve ApplicationBaseDirectory and AssemblyFileName when ConfigFileExtension property is set.", ex);
                    }
                    if (applicationBaseDirectory != null)
                    {
                        fullPath2ConfigFile = Path.Combine(applicationBaseDirectory, SystemInfo.AssemblyFileName(sourceAssembly) + m_configFileExtension);
                    }
                }
            }
            //使用指定的配置文件
            else
            {
                string applicationBaseDirectory = null;
                try
                {
                    applicationBaseDirectory = SystemInfo.ApplicationBaseDirectory;
                }
                catch (Exception ex)
                {
                    ConsoleInfo.Warn(declaringType, "Exception getting ApplicationBaseDirectory. ConfigFile property path [" + m_configFile + "] will be treated as an absolute path.", ex);
                }
                if (applicationBaseDirectory != null)
                {
                    //获取文件全名
                    fullPath2ConfigFile = Path.Combine(applicationBaseDirectory, m_configFile);
                }
                else
                {
                    fullPath2ConfigFile = m_configFile;
                }
            }
            if (fullPath2ConfigFile != null)
            {
                ConfigureFromFile(targetRepository, new FileInfo(fullPath2ConfigFile));
            }
        }
        private void ConfigureFromFile(IReporterRepository targetRepository, FileInfo configFile)
        {
#if (SSCLI)
			if (m_configureAndWatch)
			{
				LogLog.Warn(declaringType, "XmlConfiguratorAttribute: Unable to watch config file not supported on SSCLI");
			}
			XmlConfigurator.Configure(targetRepository, configFile);
#else
            //是否监视配置文件
            if (m_configureAndWatch)
            {
                XmlConfigurator.ConfigureAndWatch(targetRepository, configFile);
            }
            else
            {
                XmlConfigurator.Configure(targetRepository, configFile);
            }
#endif
        }
        private void ConfigureFromUri(Assembly sourceAssembly, IReporterRepository targetRepository)
        {
            //得到配置文件的全路径
            Uri fullPath2ConfigFile = null;

            // 选择配置文件
            if (m_configFile == null || m_configFile.Length == 0)
            {
                //不存在扩展名，使用默认扩展名(.config)的默认配置文件
                if (m_configFileExtension == null || m_configFileExtension.Length == 0)
                {
                    //文件系统路径
                    string systemConfigFilePath = null;
                    try
                    {
                        //获得本地配置文件
                        systemConfigFilePath = SystemInfo.ConfigurationFileLocation;
                    }
                    catch (Exception ex)
                    {
                        ConsoleInfo.Error(declaringType, "Exception getting ConfigurationFileLocation. Must be able to resolve ConfigurationFileLocation when ConfigFile and ConfigFileExtension properties are not set.", ex);
                    }
                    //本地配置文件存在
                    if (systemConfigFilePath != null)
                    {
                        Uri systemConfigFileUri = new Uri(systemConfigFilePath);
                        //使用默认扩展名(.config)的配置文件
                        fullPath2ConfigFile = systemConfigFileUri;
                    }
                }
                //使用指定扩展名的默认配置文件
                else
                {
                    //加点操作
                    if (m_configFileExtension[0] != '.')
                    {
                        m_configFileExtension = "." + m_configFileExtension;
                    }

                    string systemConfigFilePath = null;
                    try
                    {
                        systemConfigFilePath = SystemInfo.ConfigurationFileLocation;
                    }
                    catch (Exception ex)
                    {
                        ConsoleInfo.Error(declaringType, "Exception getting ConfigurationFileLocation. Must be able to resolve ConfigurationFileLocation when the ConfigFile property are not set.", ex);
                    }

                    if (systemConfigFilePath != null)
                    {
                        UriBuilder builder = new UriBuilder(new Uri(systemConfigFilePath));
                        //将默认配置文件的扩展名改成指定扩展名
                        string path = builder.Path;
                        int startOfExtension = path.LastIndexOf(".");
                        if (startOfExtension >= 0)
                        {
                            path = path.Substring(0, startOfExtension);
                        }
                        path += m_configFileExtension;
                        builder.Path = path;
                        fullPath2ConfigFile = builder.Uri;
                    }
                }
            }
            //使用指定的uri资源路径
            else
            {
                string applicationBaseDirectory = null;
                try
                {
                    applicationBaseDirectory = SystemInfo.ApplicationBaseDirectory;
                }
                catch (Exception ex)
                {
                    ConsoleInfo.Warn(declaringType, "Exception getting ApplicationBaseDirectory. ConfigFile property path [" + m_configFile + "] will be treated as an absolute URI.", ex);
                }

                if (applicationBaseDirectory != null)
                {
                    // 获得全路径uri
                    fullPath2ConfigFile = new Uri(new Uri(applicationBaseDirectory), m_configFile);
                }
                else
                {
                    fullPath2ConfigFile = new Uri(m_configFile);
                }
            }

            //判断uri是否为本地路径
            if (fullPath2ConfigFile != null)
            {
                //本地路径
                if (fullPath2ConfigFile.IsFile)
                {
                    ConfigureFromFile(targetRepository, new FileInfo(fullPath2ConfigFile.LocalPath));
                }
                //网络路径
                else
                {
                    //网络路径不可监控
                    if (m_configureAndWatch)
                    {
                        ConsoleInfo.Warn(declaringType, "XmlConfiguratorAttribute: Unable to watch config file loaded from a URI");
                    }
                    XmlConfigurator.Configure(targetRepository, fullPath2ConfigFile);
                }
            }
        }
        #endregion
    }
}
