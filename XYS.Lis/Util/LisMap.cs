using System;
using System.Collections;

using XYS.Lis.Config;
using XYS.Lis.Core;
using XYS.Lis.Model;

namespace XYS.Lis.Util
{
    public class LisMap
    {
        #region
        private static readonly Type declaringType = typeof(LisMap);
        #endregion

        #region
        private LisMap()
        { }
        #endregion

        #region

        public static void InitModelNo2ModelPathTable(Hashtable table)
        {
            table.Clear();
            string modelFileName;
            ReportModelMap modelMap = new ReportModelMap();
            ConfigureReportModelMap(modelMap);
            foreach (ReportModel rm in modelMap.AllModels)
            {
                modelFileName = SystemInfo.GetFileFullName(SystemInfo.GetReportModelFilePath(), rm.ModelPath);
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
            ReporterSectionMap sectionMap = new ReporterSectionMap();
            ConfigureReportSectionMap(sectionMap);
            foreach (ReporterSection section in sectionMap.AllReporterSection)
            {
                table.Add(section.SectionNo, section.ModelNo);
            }
        }
        public static void InitSection2OrderNoTable(Hashtable table)
        {
            table.Clear();
            ReporterSectionMap sectionMap = new ReporterSectionMap();
            ConfigureReportSectionMap(sectionMap);
            foreach (ReporterSection section in sectionMap.AllReporterSection)
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
        public static void InitSection2ElementTypeTable(Hashtable table)
        {
            ReportElementTypeCollection retc;
            ReporterSectionMap sectionMap = new ReporterSectionMap();
            ReportElementTypeMap elementTypeMap = new ReportElementTypeMap();
            ConfigureReportSectionMap(sectionMap);
            ConfigureReportElementMap(elementTypeMap);
            foreach (ReporterSection rs in sectionMap.AllReporterSection)
            {
                retc = new ReportElementTypeCollection(3);
                if (rs.ElementNameList.Count > 0)
                {
                    ReportElementType ret;
                    foreach (string name in rs.ElementNameList)
                    {
                        ret = elementTypeMap[name];
                        if (ret != null)
                        {
                            retc.Add(ret);
                        }
                    }
                }
                table[rs.SectionNo] = retc;
            }
        }
        #endregion

        #region
        private static void ConfigureReportModelMap(ReportModelMap modelMap)
        {
            ReportLog.Debug(declaringType, "LisMap:configuring ReportModelMap");
            XmlParamConfigurator.ConfigReportModelMap(modelMap);
        }
        private static void ConfigureReportElementMap(ReportElementTypeMap elementTypeMap)
        {
            ReportLog.Debug(declaringType, "LisMap:configuring ReportElementTypeMap");
            XmlParamConfigurator.ConfigReportElementMap(elementTypeMap);
        }
        private static void ConfigureReportSectionMap(ReporterSectionMap sectionMap)
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