using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;
namespace XYS.Lis.Report.Util
{
    public class ConfigManager
    {
        #region 私有常量
        private static readonly Type declaringType = typeof(ConfigManager);
        private static readonly FillElementMap ELEMENT_MAP = new FillElementMap(4);
        private static readonly LisSectionMap SECTION_MAP = new LisSectionMap(20);
        #endregion

        #region 构造函数
        private ConfigManager()
        { }
        static ConfigManager()
        {
            InternalConfigure();
        }
        #endregion

        #region
        public static void AddSection(LisSection section)
        {
            SECTION_MAP.Add(section);
        }
        public static void AddSubElement(FillElement element)
        {
            ELEMENT_MAP.Add(element);
        }
        #endregion

        #region
        public static void InitSection2FillElementTable(Hashtable table)
        {
            table.Clear();
            if (SECTION_MAP.Count == 0 || ELEMENT_MAP.Count == 0)
            {
                InternalConfigure();
            }
            LisSection section = null;
            FillElement element = null;
            List<Type> tempList = null;
            foreach (object rs in SECTION_MAP.AllReporterSection)
            {
                section = rs as LisSection;
                if (section != null)
                {
                    if (section.FillElementList.Count > 0)
                    {
                        tempList = new List<Type>(2);
                        foreach (string name in section.FillElementList)
                        {
                            element = ELEMENT_MAP[name];
                            if (element != null)
                            {
                                tempList.Add(element.EType);
                            }
                        }
                        table[section.SectionNo] = tempList;
                    }
                }
            }
        }
        public static void InitType2HandleTable(Hashtable table)
        {
            FillElement fill = null;
            IHandle handle = null;
            table.Clear();
            foreach (object element in ELEMENT_MAP.AllElements)
            {
                fill = element as FillElement;
                if (fill != null)
                {
                    try
                    {
                        handle = (IHandle)Activator.CreateInstance(fill.HandleType);
                        if (handle != null)
                        {
                            table.Add(fill.EType, handle);
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }
        }
        #endregion

        #region
        private static void InternalConfigure()
        {
            FileInfo fi = null;
            string fullPath2ConfigFile = null;
            string applicationBaseDirectory = null;
            try
            {
                applicationBaseDirectory = SystemInfo.ApplicationBaseDirectory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //获取文件全名
            fullPath2ConfigFile = Path.Combine(applicationBaseDirectory, "XYS.Lis.Report.xml");
            try
            {
                fi = new FileInfo(fullPath2ConfigFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            InternalConfigure(fi);
        }
        private static void InternalConfigure(FileInfo configFile)
        {
            ConsoleInfo.Debug(declaringType, "configure using file [" + configFile + "]");
            if (configFile == null)
            {
                ConsoleInfo.Error(declaringType, "Configure called with null 'configFile' parameter");
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
                                ConsoleInfo.Error(declaringType, "Failed to open XML config file [" + configFile.Name + "]", ex);
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
                            InternalConfigure(fs);
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
                    ConsoleInfo.Error(declaringType, "config file [" + configFile.FullName + "] not found.");
                }
            }
        }
        private static void InternalConfigure(Stream configStream)
        {
            ConsoleInfo.Debug(declaringType, "configure using stream");
            if (configStream == null)
            {
                ConsoleInfo.Error(declaringType, "Configure called with null 'configStream' parameter");
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                try
                {
                    // Create a validating reader around a text reader for the file stream
                    XmlReaderSettings xmlSettings = new XmlReaderSettings();
                    xmlSettings.ValidationType = ValidationType.None;
                    XmlReader xmlReader = XmlReader.Create(new XmlTextReader(configStream), xmlSettings);
                    //将文件流加载到xml文档
                    doc.Load(xmlReader);
                }
                catch (Exception ex)
                {
                    ConsoleInfo.Error(declaringType, "Error while loading XML configuration", ex);
                    doc = null;
                }
                if (doc != null)
                {
                    ConsoleInfo.Debug(declaringType, "loading XML configuration");
                    //获取节点集合
                    XmlNodeList configNodeList = doc.GetElementsByTagName("lis-param");
                    //节点必须有且只有一个
                    if (configNodeList.Count == 0)
                    {
                        ConsoleInfo.Error(declaringType, "XML configuration does not contain a <lis-param> element. Configuration Aborted.");
                    }
                    else if (configNodeList.Count > 1)
                    {
                        ConsoleInfo.Error(declaringType, "XML configuration contains [" + configNodeList.Count + "] <lis-param> elements. Only one is allowed. Configuration Aborted.");
                    }
                    else
                    {
                        //根据节点信息，配置repository
                        InternalConfigureFromXml(configNodeList[0] as XmlElement);
                    }
                }
            }
        }
        private static void InternalConfigureFromXml(XmlElement element)
        {
            if (element == null)
            {
                ConsoleInfo.Error(declaringType, "ConfigureFromXml called with null 'element' parameter");
            }
            else
            {
                ConsoleInfo.Debug(declaringType, "Configure using XML element");

                XmlDocument newDoc = new XmlDocument();
                XmlElement targetElement = (XmlElement)newDoc.AppendChild(newDoc.ImportNode(element, true));
                XmlConfigurator.Configure(targetElement);
            }
        }
        #endregion
    }
}