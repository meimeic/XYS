using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Common;

using XYS.Report.Lis.Util;
using XYS.Report.Lis.Core;
using XYS.Report.Lis.Model;
namespace XYS.Report.Lis.Filler
{
    public abstract class ReportFillSkeleton : IReportFiller
    {
        #region 字段
        private readonly string m_fillerName;
        private readonly Hashtable m_section2FillTypeMap;
        #endregion

        #region 构造函数
        protected ReportFillSkeleton(string name)
        {
            this.m_fillerName = name;
            this.m_section2FillTypeMap = new Hashtable(20);
        }
        #endregion

        #region 实现IReportFiller接口
        public string FillerName
        {
            get { return this.m_fillerName; }
        }
        public virtual void Fill(ReportReportElement report, ReportPK RK)
        {
            if (report != null)
            {
                FillReport(report, RK);
            }
        }
        #endregion

        #region 内部报告处理逻辑
        protected virtual void FillReport(ReportReportElement rre, ReportPK RK)
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
        protected abstract void FillElement(ILisReportElement reportElement, ReportPK RK);
        protected abstract void FillElements(List<ILisReportElement> reportElementList, ReportPK RK, Type type);
        #endregion

        #region 辅助方法
        protected virtual int GetSectionNo(ReportPK RK)
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
            if (this.m_section2FillTypeMap.Count == 0)
            {
                InitFillElementTable();
            }
            return this.m_section2FillTypeMap[sectionNo] as List<Type>;
        }
        protected virtual List<Type> GetAvailableInsideElements(ReportPK key)
        {
            int sectionNo = GetSectionNo(key);
            return GetAvailableInsideElements(sectionNo);
        }
        #endregion

        #region 私有方法
        private void InitFillElementTable()
        {
            lock (this.m_section2FillTypeMap)
            {
                 ConfigManager.InitSection2FillElementTable(this.m_section2FillTypeMap);
            }
        }
        protected bool IsFill(ILisReportElement element)
        {
            return element is AbstractFillElement;
        }
        protected bool IsFill(Type type)
        {
            if (type != null)
            {
                return typeof(AbstractFillElement).IsAssignableFrom(type);
            }
            return false;
        }
        protected bool IsReport(ILisReportElement reportElement)
        {
            return reportElement is ReportReportElement;
        }
        protected bool IsReport(Type type)
        {
            return typeof(ReportReportElement).Equals(type);
        }
        #endregion
    }
}