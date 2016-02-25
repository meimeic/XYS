using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Lis.Config;
using XYS.Lis.Core;
namespace XYS.Lis.Util
{
    public class LisMap
    {
        #region
        private static readonly Type declaringType = typeof(LisMap);
        private static readonly ReportSectionMap SECTION_MAP = new ReportSectionMap();
        private static readonly ElementTypeMap ELEMENT_MAP = new ElementTypeMap();
        #endregion

        #region
        private LisMap()
        { }
        #endregion

        #region
        static LisMap()
        {
        }
        #endregion

        #region

        public static void InitModelNo2ModelPathTable(Hashtable table)
        {
            table.Clear();
            string modelFileName;
            PrintModelMap modelMap = new PrintModelMap();
            ConfigureReportModelMap(modelMap);
            foreach (PrintModel rm in modelMap.AllModels)
            {
                modelFileName = SystemInfo.GetFileFullName(SystemInfo.GetPrintModelFilePath(), rm.ModelName);
                table.Add(rm.ModelNo, modelFileName);
            }
        }
        public static void InitParItem2ReportModelTable(Hashtable table)
        {
            table.Clear();
            ParItemMap parItemMap = new ParItemMap();
            ConfigureParItemMap(parItemMap);
            foreach (ParItem item in parItemMap.AllParItem)
            {
                table.Add(item.ParItemNo, item.ModelNo);
            }
        }
        public static void InitParItem2OrderNoTable(Hashtable table)
        {
            table.Clear();
            ParItemMap parItemMap = new ParItemMap();
            ConfigureParItemMap(parItemMap);
            foreach (ParItem item in parItemMap.AllParItem)
            {
                table.Add(item.ParItemNo, item.OrderNo);
            }
        }
        public static void InitSection2PrintModelTable(Hashtable table)
        {
            table.Clear();
            ReportSectionMap sectionMap = new ReportSectionMap();
            ConfigureReportSectionMap(sectionMap);
            foreach (ReportSection section in sectionMap.AllReporterSection)
            {
                table.Add(section.SectionNo, section.ModelNo);
            }
        }
        public static void InitSection2OrderNoTable(Hashtable table)
        {
            table.Clear();
            ReportSectionMap sectionMap = new ReportSectionMap();
            ConfigureReportSectionMap(sectionMap);
            foreach (ReportSection section in sectionMap.AllReporterSection)
            {
                table.Add(section.SectionNo, section.OrderNo);
            }
        }
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
        public static void InitSection2InnerElementTable(Hashtable table)
        {
            table.Clear();
            ElementTypeMap elementMap = new ElementTypeMap();
            ConfigureReportElementMap(elementMap);
            ReportSectionMap sectionMap = new ReportSectionMap();
            ConfigureReportSectionMap(sectionMap);
            ElementType eType = null;
            ElementTypeMap eMap = null;
            foreach (ReportSection rs in sectionMap.AllReporterSection)
            {
                if (rs.InnerElementList.Count > 0)
                {
                    eMap = new ElementTypeMap();
                    foreach (string name in rs.InnerElementList)
                    {
                        eType = elementMap[name];
                        if (eType != null)
                        {
                            eMap.Add(eType);
                        }
                    }
                    table[rs.SectionNo] = eMap;
                }
            }
        }
        #endregion

        #region
        private static void ConfigureReportModelMap(PrintModelMap modelMap)
        {
            ReportLog.Debug(declaringType, "LisMap:configuring ReportModelMap");
            XmlParamConfigurator.ConfigReportModelMap(modelMap);
        }
        
        private static void ConfigureReportElementMap()
        {
            ReportLog.Debug(declaringType, "LisMap:configuring ReportElementTypeMap");
            XmlParamConfigurator.ConfigReportElementMap(ELEMENT_MAP);
        }
        private static void ConfigureReportElementMap(ElementTypeMap elementTypeMap)
        {
            ReportLog.Debug(declaringType, "LisMap:configuring ReportElementTypeMap");
            XmlParamConfigurator.ConfigReportElementMap(elementTypeMap);
        }
        
        private static void ConfigureReportSectionMap()
        {
            ReportLog.Debug(declaringType, "LisMap:configuring ReportSectionMap");
            XmlParamConfigurator.ConfigSectionMap(SECTION_MAP);
        }
        private static void ConfigureReportSectionMap(ReportSectionMap sectionMap)
        {
            ReportLog.Debug(declaringType, "LisMap:configuring ReportSectionMap");
            XmlParamConfigurator.ConfigSectionMap(sectionMap);
        }

        public static void ConfigureParItemMap(ParItemMap parItemMap)
        {
            ReportLog.Debug(declaringType, "LisMap:configuring ParItemMap");
            XmlParamConfigurator.ConfigParItemMap(parItemMap);
        }
        #endregion
    }
}