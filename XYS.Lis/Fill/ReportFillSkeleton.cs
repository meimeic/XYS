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
        #region
        private readonly string m_fillerName;
        //private readonly Dictionary<int, ReportElementTypeCollection> m_section2ElementTypesMap;
        private readonly Hashtable m_section2ElementTypesMap;
        private ReportElementTypeCollection m_defaultElementTypeCollection;
        #endregion
        
        #region
        protected ReportFillSkeleton(string name)
        {
            this.m_section2ElementTypesMap = new Hashtable(10);
            this.m_defaultElementTypeCollection = new ReportElementTypeCollection(6);
            this.m_fillerName = name.ToLower();
        }
        #endregion

        #region 属性
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
        public virtual void Fill(Hashtable reportElementTable, ReportKey key, ReportElementTag elementTag)
        {
            if (elementTag == ReportElementTag.ReportElement)
            {
                return;
            }
            else
            {
                FillElements(reportElementTable, key, elementTag);
            }
        }
        #endregion

        #region 抽象方法
        protected abstract void FillReport(ReportReportElement rre, ReportKey key);
        protected abstract void FillElements(Hashtable reportElementTable, ReportKey key, ReportElementTag elementTag);
        protected abstract void FillElement(ILisReportElement reportElement, ReportKey key);
        #endregion

        #region
        protected Type GetElementType(int sectionNo,ReportElementTag elementTag)
        {
            ReportElementTypeCollection availableElements = GetAvailableElements(sectionNo);
            ReportElementType elementType=null;
            foreach (ReportElementType type in availableElements)
            {
                if (type.ElementTag == elementTag)
                {
                    elementType = type;
                    break;
                }
            }
            if (elementType != null)
            {
                return elementType.ElementType;
            }
            else
            {
                return null;
            }
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
            this.m_defaultElementTypeCollection.Add(ReportElementType.DEFAULTEXAM);
            this.m_defaultElementTypeCollection.Add(ReportElementType.DEFAULTREPORT);
            this.m_defaultElementTypeCollection.Add(ReportElementType.DEFAULTPATIENT);
        }
        #endregion

        #region
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
        #endregion

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
    }
}
