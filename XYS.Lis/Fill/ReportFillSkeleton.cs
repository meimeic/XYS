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
        private readonly Hashtable m_section2InsideElementMap;
        private readonly Hashtable m_section2ExtendElementMap;
        #endregion

        #region 构造函数
        protected ReportFillSkeleton(string name)
        {
            this.m_fillerName = name;
            this.m_section2InsideElementMap = new Hashtable(20);
        }
        #endregion

        #region 实现IReportFiller接口
        public virtual string FillerName
        {
            get { return this.m_fillerName.ToLower(); }
        }
        public virtual void Fill(IReportElement reportElement, ReportKey RK)
        {
            if (reportElement.ElementTag == ReportElementTag.Report)
            {
                ReportReportElement rre = reportElement as ReportReportElement;
                if (rre != null)
                {
                    FillReport(rre, RK);
                }
            }
            else
            {
                if (IsTable(reportElement))
                {
                    //填充元素
                    FillElement(reportElement, RK);
                }
            }
        }
        public virtual void Fill(List<IReportElement> reportElementList, ReportKey RK, string elementName)
        {
            if (!string.IsNullOrEmpty(elementName))
            {
                ElementTypeMap map = GetAvailableInsideElements(RK);
                if (map != null && map.Count > 0)
                {
                    ElementType elementType = map[elementName.ToLower()];
                    if (elementType != null && IsTable(elementType.EType))
                    {
                        FillElements(reportElementList, RK, elementType);
                    }
                }
            }
        }
        #endregion

        #region 内部处理逻辑
        protected virtual void FillReport(ReportReportElement rre, ReportKey RK)
        {
            ElementTypeMap availableElementMap = this.GetAvailableInsideElements(RK);
            if (availableElementMap != null && availableElementMap.Count > 0)
            {
                List<IReportElement> tempList = null;
                foreach (ElementType elementType in availableElementMap.AllElementTypes)
                {
                    if (IsTable(elementType.EType))
                    {
                        tempList = rre.GetReportItem(elementType.Name);
                        FillElements(tempList, RK, elementType);
                    }
                }
            }
        }
        #endregion

        #region 抽象方法
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
        protected virtual ElementTypeMap GetAvailableInsideElements(int sectionNo)
        {
            if (this.m_section2InsideElementMap.Count == 0)
            {
                InitInsideElementTable(this.m_section2InsideElementMap);
            }
            ElementTypeMap result = this.m_section2InsideElementMap[sectionNo] as ElementTypeMap;
            return result;
        }
        protected virtual ElementTypeMap GetAvailableInsideElements(ReportKey key)
        {
            int sectionNo = GetSectionNo(key);
            return GetAvailableInsideElements(sectionNo);
        }
        protected virtual ElementTypeMap GetAvailableExtendElements(int sectionNo)
        {
            if (this.m_section2InsideElementMap.Count == 0)
            {
                InitExtendElementTable(this.m_section2InsideElementMap);
            }
            ElementTypeMap result = this.m_section2InsideElementMap[sectionNo] as ElementTypeMap;
            return result;
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
        protected virtual void InitExtendElementTable(Hashtable table)
        {

        }
        protected virtual void InitInsideElementTable(Hashtable table)
        {

        }
        #endregion

        #region 私有方法
        private bool IsTable(IReportElement element)
        {
            Type type = element.GetType();
            return IsTable(type);
        }
        private bool IsTable(Type elementType)
        {
            object[] attrs = elementType.GetCustomAttributes(typeof(TableAttribute), true);
            if (attrs != null && attrs.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
