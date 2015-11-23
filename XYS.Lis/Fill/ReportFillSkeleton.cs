using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Common;
using XYS.Model;
using XYS.Lis.Core;
using XYS.Lis.Model;
using XYS.Lis.Util;

namespace XYS.Lis.Fill
{
    public abstract class ReportFillSkeleton : IReportFiller
    {
        #region 变量
        private readonly string m_fillerName;
        private readonly Hashtable m_section2ElementTypesMap;
        private ReportElementTypeCollection m_defaultElementTypeCollection;
        #endregion

        #region 构造函数
        protected ReportFillSkeleton(string name)
        {
            this.m_fillerName = name;
            this.m_section2ElementTypesMap = new Hashtable(20);
            this.m_defaultElementTypeCollection = new ReportElementTypeCollection(2);
        }
        #endregion

        #region 实例属性
        public virtual ReportElementTypeCollection DefaultElementTypeCollection
        {
            get
            {
                if (this.m_defaultElementTypeCollection == null || this.m_defaultElementTypeCollection.Count == 0)
                {
                    this.InitDefaultElements();
                }
                return this.m_defaultElementTypeCollection;
            }
        }
        #endregion

        #region 实现接口
        public virtual string FillerName
        {
            get { return this.m_fillerName.ToLower(); }
        }

        public virtual void Fill(ILisReportElement reportElement, ReportKey key)
        {
            if (reportElement.ElementTag == ReportElementTag.ReportElement)
            {
                ReportReportElement rre = reportElement as ReportReportElement;
                if (rre != null)
                {
                    FillReport(rre, key);
                }
            }
            else
            {
                FillElement(reportElement, key);
            }
        }
        public virtual void Fill(List<ILisReportElement> reportElementList, ReportKey key, ReportElementTag elementTag)
        {
            //不可填充的报告元素
            if (elementTag == ReportElementTag.ReportElement || elementTag == ReportElementTag.CustomElement || elementTag == ReportElementTag.NoneElement)
            {
                return;
            }
            else
            {
                FillElements(reportElementList, key, elementTag);
            }
        }
        #endregion

        #region 抽象方法
        protected abstract void FillReport(ReportReportElement rre, ReportKey key);
        protected abstract void FillElements(List<ILisReportElement> reportElementList, ReportKey key, ReportElementTag elementTag);
        protected abstract void FillElement(ILisReportElement reportElement, ReportKey key);
        #endregion

        #region 辅助方法
        protected Type GetElementType(int sectionNo, ReportElementTag elementTag)
        {
            ReportElementTypeCollection availableElements = this.GetAvailableElements(sectionNo);
            Type type = null;
            foreach (ReportElementType elementType in availableElements)
            {
                if (elementType.ElementTag == elementTag)
                {
                    type = elementType.ElementType;
                    break;
                }
            }
            return type;
        }
        protected ReportElementTypeCollection GetAvailableElements(int sectionNo)
        {
            ReportElementTypeCollection retc = this.m_section2ElementTypesMap[sectionNo] as ReportElementTypeCollection;
            if (retc == null)
            {
                retc = LisMap.GetSection2ElementTypeCollcetion(sectionNo);
                if (retc != null)
                {
                    lock (this.m_section2ElementTypesMap)
                    {
                        this.m_section2ElementTypesMap[sectionNo] = retc;
                    }
                }
            }
            if (retc == null)
            {
                //
                retc = this.DefaultElementTypeCollection;
            }
            return retc;
        }
        private void InitDefaultElements()
        {
            if (this.m_defaultElementTypeCollection == null)
            {
                this.m_defaultElementTypeCollection = new ReportElementTypeCollection(2);
            }
            this.m_defaultElementTypeCollection.Add(ReportElementType.DEFAULTINFO);
            this.m_defaultElementTypeCollection.Add(ReportElementType.DEFAULTITEM);
        }
        #endregion

        #region 未调用方法
        //public void ClearSection2ElementTypesMap()
        //{
        //    this.m_section2ElementTypesMap.Clear();
        //} 
        //public void AddSection2ElementTypes(int sectionNo,ReportElementTypeCollection elementTypeCollection)
        //{
        //    if (elementTypeCollection != null && elementTypeCollection.Count > 0)
        //    {
        //        lock (this.m_section2ElementTypesMap)
        //        {
        //            this.m_section2ElementTypesMap[sectionNo] = elementTypeCollection;
        //        }
        //    }
        //}
        //public virtual ILisReportElement Fill(ReportKey key, ReportElementTag elementTag)
        //{
        //    ILisReportElement reportElement = CreateReportElement(elementTag);
        //    if (reportElement != null)
        //    {
        //        Fill(reportElement, key);
        //    }
        //    return reportElement;
        //}

        //public virtual List<ILisReportElement> FillList(ReportKey key, ReportElementTag elementTag)
        //{
        //    List<ILisReportElement> result = new List<ILisReportElement>();
        //    if (elementTag != ReportElementTag.ReportElement)
        //    {
        //        FillList(result, key, elementTag);
        //    }
        //    return result;
        //}
        //protected ILisReportElement CreateReportElement(ReportElementTag elementTag)
        //{
        //    Type type = GetElementType(elementTag);
        //    if (type != null)
        //    {
        //        try
        //        {
        //            ILisReportElement element = (ILisReportElement)Activator.CreateInstance(type);
        //            return element;
        //        }
        //        catch (Exception ex)
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //public virtual void Fill(Hashtable reportElementTable, ReportKey key, ReportElementTag elementTag)
        //{
        //    if (elementTag == ReportElementTag.ReportElement)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        FillElements(reportElementTable, key, elementTag);
        //    }
        //}
        //protected abstract void FillElements(Hashtable reportElementTable, ReportKey key, ReportElementTag elementTag);
        #endregion
    }
}
