using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;
using XYS.Report.Core;
namespace XYS.Report.Util
{
    public class LisConfiguration
    {
         #region
        private static readonly Type declaringType = typeof(LisConfiguration);
        private static readonly LisSectionMap SECTION_MAP = new LisSectionMap();
        private static readonly LisElementMap ELEMENT_MAP=new LisElementMap();
        private static readonly LisParItemMap PARITEM_MAP = new LisParItemMap();
        #endregion

        #region
        private LisMap()
        { }
        static LisMap()
        {
        }
        #endregion

        #region
        public static void InitParItem2NormalImageTable(Hashtable table)
        {
            byte[] imageArray;
            string imageFileName;
            table.Clear();
            ParItemMap parItemMap = new ParItemMap();
            ConfigureParItemMap(parItemMap);
            foreach (ParItem item in parItemMap.AllParItem)
            {
                if (item.ImageFlag)
                {
                    imageFileName = SystemInfo.GetFileFullName(SystemInfo.GetNormalImageFilePath(), item.ImagePath);
                    imageArray = SystemInfo.ReadImageFile(imageFileName);
                    table[item.ParItemNo] = imageArray;
                }
            }
        }
        public static void InitSection2FillElementTable(Hashtable table)
        {
            table.Clear();
            ElementTypeMap elementMap = new ElementTypeMap();
            ConfigureReportElementMap(elementMap);
            ReportSectionMap sectionMap = new ReportSectionMap();
            ConfigureReportSectionMap(sectionMap);

            ElementType eType = null;
            List<Type> tempList = null;
            foreach (ReportSection rs in sectionMap.AllReporterSection)
            {
                if (rs.InnerElementList.Count > 0)
                {
                    tempList = new List<Type>(2);
                    foreach (string name in rs.InnerElementList)
                    {
                        eType = elementMap[name];
                        if (eType != null)
                        {
                            tempList.Add(eType.EType);
                        }
                    }
                    table[rs.SectionNo] = tempList;
                }
            }
        }
        #endregion

        #region
        private static void ConfigureReportElementMap()
        {
        }
        public static void ConfigureReportElementMap(LisElementMap elementMap)
        {
            ConsoleInfo.Debug(declaringType, "configuring ReportElementTypeMap");
            XmlParamConfigurator.ConfigElementTypes(elementMap);
            ConsoleInfo.Debug(declaringType, "configured ReportElementTypeMap");
        }
        
        private static void ConfigureReportSectionMap()
        {
            ReportLog.Debug(declaringType, "LisMap:configuring ReportSectionMap");
            XmlParamConfigurator.ConfigSectionMap(SECTION_MAP);
        }
        private static void ConfigureReportSectionMap(LisSectionMap sectionMap)
        {
            ConsoleInfo.Debug(declaringType, "configuring ReportSectionMap");
            XmlParamConfigurator.ConfigSectionMap(sectionMap);
            ConsoleInfo.Debug(declaringType, "configured ReportSectionMap");
        }

        public static void ConfigureParItemMap(LisParItemMap parItemMap)
        {
            ConsoleInfo.Debug(declaringType, "configuring ParItemMap");
            XmlParamConfigurator.ConfigParItemMap(parItemMap);
            ConsoleInfo.Debug(declaringType, "configured ParItemMap");
        }
        #endregion
    }
}
