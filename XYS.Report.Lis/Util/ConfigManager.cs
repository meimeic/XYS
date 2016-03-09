using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;

using XYS.Report.Lis.Core;
using XYS.Report.Lis.Config;
namespace XYS.Report.Lis.Util
{
    public class ConfigManager
    {
        #region
        private static readonly Type declaringType = typeof(ConfigManager);
        private static readonly LisSectionMap SECTION_MAP = new LisSectionMap();
        private static readonly LisElementMap ELEMENT_MAP = new LisElementMap();
        private static readonly LisParItemMap PARITEM_MAP = new LisParItemMap();
        #endregion

        #region
        private ConfigManager()
        { }
        static ConfigManager()
        {
        }
        #endregion

        #region
        public static void InitParItem2NormalImageDictionary(Dictionary<int, byte[]> map)
        {
            map.Clear();
            byte[] imageArray = null;
            string imageFullName = null;
            LisParItem parItem = null;
            if (PARITEM_MAP.Count == 0)
            {
                ConfigureParItemMap();
            }
            foreach (object item in PARITEM_MAP.AllParItem)
            {
                parItem = item as LisParItem;
                if (parItem != null)
                {
                    if (parItem.ImageFlag)
                    {
                        imageFullName = SystemInfo.GetFileFullName(LisImage.GetImageFilePath(), parItem.ImageName);
                        imageArray = LisImage.ReadImageFile(imageFullName);
                        map[parItem.ParItemNo] = imageArray;
                    }
                }
            }
        }
        public static void InitParItem2NormalImageTable(Hashtable table)
        {
            table.Clear();
            byte[] imageArray=null;
            string imageFullName=null;
            LisParItem parItem = null;
            if (PARITEM_MAP.Count == 0)
            {
                ConfigureParItemMap();
            }
            foreach (object item in PARITEM_MAP.AllParItem)
            {
                parItem = item as LisParItem;
                if (parItem != null)
                {
                    if (parItem.ImageFlag)
                    {
                        imageFullName = SystemInfo.GetFileFullName(LisImage.GetImageFilePath(), parItem.ImageName);
                        imageArray = LisImage.ReadImageFile(imageFullName);
                        table[parItem.ParItemNo] = imageArray;
                    }
                }
            }
        }
        public static void InitSection2FillElementTable(Hashtable table)
        {
            table.Clear();
            if (SECTION_MAP.Count == 0)
            {
                ConfigureSectionMap();
            }
            if (ELEMENT_MAP.Count == 0)
            {
                ConfigureElementMap();
            }
            LisSection section = null;
            LisElement element = null;
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
        public static void InitAllElementList(List<Type> elementTypeList)
        {
            elementTypeList.Clear();
            if (ELEMENT_MAP.Count == 0)
            {
                ConfigureElementMap();
            }
            LisElement element = null;
            foreach (object item in ELEMENT_MAP.AllElements)
            {
                element = item as LisElement;
                if (element != null)
                {
                    elementTypeList.Add(element.EType);
                }
            }
        }
        #endregion

        #region
        private static void ConfigureElementMap()
        {
            ConfigureElementMap(ELEMENT_MAP);
        }
        public static void ConfigureElementMap(LisElementMap elementMap)
        {
            ConsoleInfo.Debug(declaringType, "configuring LisElementMap");
            XmlLisParamConfigurator.ConfigElementMap(elementMap);
            ConsoleInfo.Debug(declaringType, "configured LisElementMap");
        }

        private static void ConfigureSectionMap()
        {
            ConfigureSectionMap(SECTION_MAP);
        }
        private static void ConfigureSectionMap(LisSectionMap sectionMap)
        {
            ConsoleInfo.Debug(declaringType, "configuring ReportSectionMap");
            XmlLisParamConfigurator.ConfigSectionMap(sectionMap);
            ConsoleInfo.Debug(declaringType, "configured ReportSectionMap");
        }

        public static void ConfigureParItemMap()
        {
            ConfigureParItemMap(PARITEM_MAP);
        }
        public static void ConfigureParItemMap(LisParItemMap parItemMap)
        {
            ConsoleInfo.Debug(declaringType, "configuring ParItemMap");
            XmlLisParamConfigurator.ConfigParItemMap(parItemMap);
            ConsoleInfo.Debug(declaringType, "configured ParItemMap");
        }
        #endregion
    }
}
