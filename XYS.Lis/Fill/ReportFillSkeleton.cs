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
        private readonly Hashtable m_section2InsideTypeMap;
        private readonly Hashtable m_section2ExtendTypeMap;
        #endregion

        #region 构造函数
        protected ReportFillSkeleton(string name)
        {
            this.m_fillerName = name;
            this.m_section2ExtendTypeMap = new Hashtable(2);
            this.m_section2InsideTypeMap = new Hashtable(20);
        }
        #endregion

        #region 实现IReportFiller接口
        public string FillerName
        {
            get { return this.m_fillerName; }
        }
        public virtual void Fill(ILisReportElement reportElement, ReportKey RK)
        {
            if (IsFill(reportElement))
            {
                //报告
                if (IsReport(reportElement))
                {
                    ReportReportElement rre = reportElement as ReportReportElement;
                    if (rre != null)
                    {
                        FillReport(rre, RK);
                    }
                }
                //报告项
                else
                {
                    FillElement(reportElement, RK);
                }
            }
        }
        public virtual void Fill(List<ILisReportElement> reportElementList, ReportKey RK, Type type)
        {
            if (IsFill(type))
            {
                //
                FillElements(reportElementList, RK, type);
            }
        }
        #endregion

        #region 内部报告处理逻辑
        protected virtual void FillReport(ReportReportElement rre, ReportKey RK)
        {
            //默认的报告项
            FillElement(rre.ReportExam, RK);
            FillElement(rre.ReportPatient, RK);
            //可选的报告项
            List<Type> availableElementList = this.GetAvailableInsideElements(RK);
            if (availableElementList != null && availableElementList.Count > 0)
            {
                List<ILisReportElement> tempList = null;
                foreach (Type type in availableElementList)
                {
                    if (IsFill(type))
                    {
                        tempList = rre.GetReportItem(type.Name);
                        FillElements(tempList, RK, type);
                    }
                }
            }
        }
        #endregion

        #region 抽象方法
        protected abstract void FillElement(ILisReportElement reportElement, ReportKey RK);
        protected abstract void FillElements(List<ILisReportElement> reportElementList, ReportKey RK, Type type);
        #endregion

        #region 辅助方法
        protected virtual int GetSectionNo(ReportKey RK)
        {
            int sectionNo = 0;
            foreach (KeyColumn c in RK.KeySet)
            {
                if (c.Name.ToLower().Equals("sectionno"))
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
        protected virtual List<Type> GetAvailableInsideElements(int sectionNo)
        {
            if (this.m_section2InsideTypeMap.Count == 0)
            {
                InitInsideElementTable();
            }
            return this.m_section2InsideTypeMap[sectionNo] as List<Type>;
        }
        protected virtual List<Type> GetAvailableInsideElements(ReportKey key)
        {
            int sectionNo = GetSectionNo(key);
            return GetAvailableInsideElements(sectionNo);
        }
        #endregion

        #region 私有方法
        private void InitExtendElementTable()
        {
        }
        private void InitInsideElementTable()
        {
            lock (this.m_section2InsideTypeMap)
            {
                LisMap.InitSection2InnerElementTable(this.m_section2InsideTypeMap);
            }
        }
        private bool IsFill(ILisReportElement element)
        {
            return element is AbstractReportElement;
        }
        private bool IsFill(Type type)
        {
            if (type != null)
            {
                return typeof(AbstractReportElement).IsAssignableFrom(type);
            }
            return false;
        }
        private bool IsReport(ILisReportElement reportElement)
        {
            return IsReport(reportElement.GetType());
        }
        private bool IsReport(Type type)
        {
            return typeof(ReportReportElement).Equals(type);
        }
        #endregion

        #region 未调用的方法
        #endregion
    }
}
