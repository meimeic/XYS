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
        #endregion

        #region
        private ConfigManager()
        { }
        static ConfigManager()
        {
        }
        #endregion

        #region
        public static void InitParItem2NormalImageTable(Hashtable table)
        {
            table.Clear();
            byte[] imageArray = null;
            string imageFullName = null;
            LisParItem parItem = null;
            LisParItemMap parItemMap = new LisParItemMap();
            ConfigureParItemMap(parItemMap);
            foreach (object item in parItemMap.AllParItem)
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
            LisSectionMap sectionMap = new LisSectionMap();
            ConfigureSectionMap(sectionMap);
            LisElementMap elementMap = new LisElementMap();
            ConfigureElementMap(elementMap);

            LisSection section = null;
            LisElement element = null;
            List<Type> tempList = null;
            foreach (object rs in sectionMap.AllReporterSection)
            {
                section = rs as LisSection;
                if (section != null)
                {
                    if (section.FillElementList.Count > 0)
                    {
                        tempList = new List<Type>(2);
                        foreach (string name in section.FillElementList)
                        {
                            element = elementMap[name];
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
            LisElementMap elementMap = new LisElementMap();
            ConfigureElementMap(elementMap);
            LisElement element = null;
            foreach (object item in elementMap.AllElements)
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
        private static void ConfigureElementMap(LisElementMap elementMap)
        {
            ConsoleInfo.Debug(declaringType, "configuring LisElementMap");
            XmlLisParamConfigurator.ConfigElementMap(elementMap);
            ConsoleInfo.Debug(declaringType, "configured LisElementMap");
        }
        private static void ConfigureSectionMap(LisSectionMap sectionMap)
        {
            ConsoleInfo.Debug(declaringType, "configuring ReportSectionMap");
            XmlLisParamConfigurator.ConfigSectionMap(sectionMap);
            ConsoleInfo.Debug(declaringType, "configured ReportSectionMap");
        }
        private static void ConfigureParItemMap(LisParItemMap parItemMap)
        {
            ConsoleInfo.Debug(declaringType, "configuring ParItemMap");
            XmlLisParamConfigurator.ConfigParItemMap(parItemMap);
            ConsoleInfo.Debug(declaringType, "configured ParItemMap");
        }
        #endregion
    }
}
