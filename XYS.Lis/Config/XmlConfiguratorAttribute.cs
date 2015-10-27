using System;
using System.Collections;
using System.Reflection;
using System.IO;

using XYS.Lis.Util;
using XYS.Lis.Repository;
using XYS.Lis.Repository.Hierarchy;

namespace XYS.Lis.Config
{
    [AttributeUsage(AttributeTargets.Assembly)]
    [Serializable]
   public class XmlConfiguratorAttribute:ConfiguratorAttribute
    {
        private string m_configFile = null;
        private string m_configFileExtension = null;
        private bool m_configureAndWatch = false;
        private readonly static Type declaringType = typeof(XmlConfiguratorAttribute);

        public XmlConfiguratorAttribute()
            : base(0) /* configurator priority 0 */
        {
        }
        #region 属性
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

        #region
        public override void Configure(Assembly sourceAssembly, IReporterRepository targetRepository)
        {
            IList configurationMessages = new ArrayList();
            using (new ReportReport.ReportReceivedAdapter(configurationMessages))
            {
                string applicationBaseDirectory = null;
                try
                {
                    applicationBaseDirectory = SystemInfo.ApplicationBaseDirectory;
                }
                catch
                {
                    // Ignore this exception because it is only thrown when ApplicationBaseDirectory is a file
                    // and the application does not have PathDiscovery permission
                }

                if (applicationBaseDirectory == null || (new Uri(applicationBaseDirectory)).IsFile)
                {
                    ConfigureFromFile(sourceAssembly, targetRepository);
                }
                else
                {
                    ConfigureFromUri(sourceAssembly, targetRepository);
                }
            }
            targetRepository.ConfigurationMessages = configurationMessages;
        }
        #endregion

        #region
        private void ConfigureFromFile(Assembly sourceAssembly, IReporterRepository targetRepository)
        {
            // Work out the full path to the config file
            string fullPath2ConfigFile = null;

            // Select the config file
            if (m_configFile == null || m_configFile.Length == 0)
            {
                if (m_configFileExtension == null || m_configFileExtension.Length == 0)
                {
                    // Use the default .config file for the AppDomain
                    try
                    {
                        fullPath2ConfigFile = SystemInfo.ConfigurationFileLocation;
                    }
                    catch (Exception ex)
                    {
                        ReportReport.Error(declaringType, "XmlConfiguratorAttribute: Exception getting ConfigurationFileLocation. Must be able to resolve ConfigurationFileLocation when ConfigFile and ConfigFileExtension properties are not set.", ex);
                    }
                }
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
                        ReportReport.Error(declaringType, "Exception getting ApplicationBaseDirectory. Must be able to resolve ApplicationBaseDirectory and AssemblyFileName when ConfigFileExtension property is set.", ex);
                    }

                    if (applicationBaseDirectory != null)
                    {
                        fullPath2ConfigFile = Path.Combine(applicationBaseDirectory, SystemInfo.AssemblyFileName(sourceAssembly) + m_configFileExtension);
                    }
                }
            }
            else
            {
                string applicationBaseDirectory = null;
                try
                {
                    applicationBaseDirectory = SystemInfo.ApplicationBaseDirectory;
                }
                catch (Exception ex)
                {
                    ReportReport.Warn(declaringType, "Exception getting ApplicationBaseDirectory. ConfigFile property path [" + m_configFile + "] will be treated as an absolute path.", ex);
                }
                if (applicationBaseDirectory != null)
                {
                    // Just the base dir + the config file
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
            // Do we configure just once or do we configure and then watch?
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
            // Work out the full path to the config file
            Uri fullPath2ConfigFile = null;

            // Select the config file
            if (m_configFile == null || m_configFile.Length == 0)
            {
                if (m_configFileExtension == null || m_configFileExtension.Length == 0)
                {
                    string systemConfigFilePath = null;
                    try
                    {
                        systemConfigFilePath = SystemInfo.ConfigurationFileLocation;
                    }
                    catch (Exception ex)
                    {
                        ReportReport.Error(declaringType, "XmlConfiguratorAttribute: Exception getting ConfigurationFileLocation. Must be able to resolve ConfigurationFileLocation when ConfigFile and ConfigFileExtension properties are not set.", ex);
                    }

                    if (systemConfigFilePath != null)
                    {
                        Uri systemConfigFileUri = new Uri(systemConfigFilePath);

                        // Use the default .config file for the AppDomain
                        fullPath2ConfigFile = systemConfigFileUri;
                    }
                }
                else
                {
                    // Force the extension to start with a '.'
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
                        ReportReport.Error(declaringType, "XmlConfiguratorAttribute: Exception getting ConfigurationFileLocation. Must be able to resolve ConfigurationFileLocation when the ConfigFile property are not set.", ex);
                    }

                    if (systemConfigFilePath != null)
                    {
                        UriBuilder builder = new UriBuilder(new Uri(systemConfigFilePath));

                        // Remove the current extension from the systemConfigFileUri path
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
            else
            {
                string applicationBaseDirectory = null;
                try
                {
                    applicationBaseDirectory = SystemInfo.ApplicationBaseDirectory;
                }
                catch (Exception ex)
                {
                    ReportReport.Warn(declaringType, "Exception getting ApplicationBaseDirectory. ConfigFile property path [" + m_configFile + "] will be treated as an absolute URI.", ex);
                }

                if (applicationBaseDirectory != null)
                {
                    // Just the base dir + the config file
                    fullPath2ConfigFile = new Uri(new Uri(applicationBaseDirectory), m_configFile);
                }
                else
                {
                    fullPath2ConfigFile = new Uri(m_configFile);
                }
            }

            if (fullPath2ConfigFile != null)
            {
                if (fullPath2ConfigFile.IsFile)
                {
                    // The m_configFile could be an absolute local path, therefore we have to be
                    // prepared to switch back to using FileInfos here
                    ConfigureFromFile(targetRepository, new FileInfo(fullPath2ConfigFile.LocalPath));
                }
                else
                {
                    if (m_configureAndWatch)
                    {
                        ReportReport.Warn(declaringType, "XmlConfiguratorAttribute: Unable to watch config file loaded from a URI");
                    }
                    XmlConfigurator.Configure(targetRepository, fullPath2ConfigFile);
                }
            }
        }
        #endregion
    }
}
