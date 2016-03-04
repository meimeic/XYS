﻿using System;
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
        private static readonly ElementTypeMap ELEMENT_MAP = new ElementTypeMap();
        #endregion

        #region
        private LisMap()
        { }
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
        public static void InitAllElementTypes(List<Type> typeList)
        {
            typeList.Clear();
            ElementTypeMap typeMap = new ElementTypeMap(7);
            ConfigureReportElementMap(typeMap);
            foreach (ElementType eType in typeMap.AllElementTypes)
            {
                if (!typeList.Contains(eType.ExportType))
                {
                    typeList.Add(eType.ExportType);
                }
            }
        }
        #endregion

        #region
        private static void ConfigureReportModelMap(PrintModelMap modelMap)
        {
            ReportLog.Debug(declaringType, "LisMap:configuring ReportModelMap");
            XmlParamConfigurator.ConfigReportModelMap(modelMap);
            ReportLog.Debug(declaringType, "LisMap:configured ReportModelMap");
        }
        
        private static void ConfigureReportElementMap()
        {
            ReportLog.Debug(declaringType, "LisMap:configuring ReportElementTypeMap");
            XmlParamConfigurator.ConfigElementTypes(ELEMENT_MAP);
        }
        public static void ConfigureReportElementMap(ElementTypeMap elementTypeMap)
        {
            ReportLog.Debug(declaringType, "LisMap:configuring ReportElementTypeMap");
            XmlParamConfigurator.ConfigElementTypes(elementTypeMap);
            ReportLog.Debug(declaringType, "LisMap:configured ReportElementTypeMap");
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
            ReportLog.Debug(declaringType, "LisMap:configured ReportSectionMap");
        }

        public static void ConfigureParItemMap(ParItemMap parItemMap)
        {
            ReportLog.Debug(declaringType, "LisMap:configuring ParItemMap");
            XmlParamConfigurator.ConfigParItemMap(parItemMap);
            ReportLog.Debug(declaringType, "LisMap:configured ParItemMap");
        }
        #endregion
    }
}
