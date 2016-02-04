using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Common;
using XYS.Lis.Core;
using XYS.Lis.Model;
using XYS.Lis.Util;
namespace XYS.Lis.Fill
{
    public abstract class ReportFillSkeleton : IReportFiller
    {
        #region 字段
        private readonly string m_fillerName;
        private readonly Hashtable m_section2ElementTypesMap;
        #endregion

        #region 构造函数
        protected ReportFillSkeleton(string name)
        {
            this.m_fillerName = name;
            this.m_section2ElementTypesMap = new Hashtable(20);
        }
        #endregion

        #region 属性
        public Hashtable Section2ElementsMap
        {
            get { return this.m_section2ElementTypesMap; }
        }
        #endregion

        #region 实现IReportFiller接口
        public virtual string FillerName
        {
            get { return this.m_fillerName.ToLower(); }
        }
        public virtual void Fill(IReportElement reportElement, ReportKey RK)
        {
            switch (reportElement.ElementTag)
            {
                case ReportElementTag.Report:
                    ReportReportElement rre = reportElement as ReportReportElement;
                    if (rre != null)
                    {
                        FillReport(rre, RK);
                    }
                    break;
                case ReportElementTag.Filler:
                    if (reportElement.ElementTag == ReportElementTag.Filler)
                    {
                        FillElement(reportElement, RK);
                    }
                    break;
                case ReportElementTag.Inner:
                    break;
            }
        }
        public virtual void Fill(List<IReportElement> reportElementList, ReportKey RK, string elementName)
        {
            ElementTypeMap map = GetAvailableElements(RK);
            string name = elementName.ToLower();
            ElementType elementType = map[name];
            if (elementType != null)
            {
                FillElements(reportElementList, RK, elementType);
            }
        }
        #endregion

        #region 内部处理逻辑
        protected virtual void FillReport(ReportReportElement rre, ReportKey RK)
        {
            ElementTypeMap availableElementMap = this.GetAvailableElements(RK);
            if (availableElementMap != null && availableElementMap.Count > 0)
            {
                List<IReportElement> tempList;
                foreach (ElementType elementType in availableElementMap.AllElementTypes)
                {
                    tempList = rre.GetReportItem(elementType.TypeName);
                    FillElements(tempList, RK, elementType);
                }
            }
        }
        //protected virtual void FillReport(ReportReportElement rre, ReportKey RK)
        //{
        //    //填充
        //    ElementTypeCollection availableElements = this.GetAvailableElements(RK);
        //    if (availableElements != null && availableElements.Count > 0)
        //    {
        //        Type tempType;
        //        foreach (ElementType elementType in availableElements)
        //        {
        //            tempType = GetElementType(elementType.TypeName);
        //            if (tempType != null)
        //            {
        //                InnerFillElements(rre.GetReportItem(elementType.TypeName), RK, tempType);
        //            }
        //        }
        //    }
        //}
        //protected virtual void FillElement(IReportElement reportElement, ReportKey RK)
        //{
        //    //不进行填充的报告元素
        //    if (reportElement.ElementTag == ReportElementTag.Inner)
        //    {
        //        return;
        //    }
        //    InnerFillElement(reportElement, RK);
        //}
        #endregion
        
        #region 抽象方法
        protected abstract void InitElementTypesTable(Hashtable table);
        protected abstract void FillElement(IReportElement reportElement, ReportKey RK);
        protected abstract void FillElements(List<IReportElement> reportElementList, ReportKey RK, ElementType elementType);
        #endregion

        #region 辅助方法
        protected Type GetElementType(string typeName)
        {
            Type result;
            try
            {
                result = SystemInfo.GetTypeFromString(typeName, true, true);
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }
        protected virtual ElementTypeMap GetAvailableElements(int sectionNo)
        {
            if (this.Section2ElementsMap.Count == 0)
            {
                InitElementTypesTable(this.Section2ElementsMap);
            }
            ElementTypeMap result = this.Section2ElementsMap[sectionNo] as ElementTypeMap;
            return result;
        }
        protected virtual ElementTypeMap GetAvailableElements(ReportKey key)
        {
            int sectionNo = GetSectionNo(key);
            return GetAvailableElements(sectionNo);
        }
        protected virtual int GetSectionNo(ReportKey key)
        {
            int sectionNo = 0;
            foreach (KeyColumn c in key.KeySet)
            {
                if (c.Name.ToLower().Equals("sectionno") || c.Name.ToLower().Equals("r.sectionno"))
                {
                    try
                    {
                        sectionNo = Convert.ToInt32(c.Value);
                    }
                    catch (Exception ex)
                    {
                        return -1;
                    }
                    break;
                }
            }
            return sectionNo;
        }
        #endregion
    }
}
