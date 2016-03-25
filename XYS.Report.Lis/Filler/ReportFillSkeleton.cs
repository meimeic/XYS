using System;
using System.Collections;
using System.Collections.Generic;

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
        public virtual void Fill(ReportReportElement report, LisReportPK RK)
        {
            if (report != null)
            {
                FillReport(report, RK);
            }
        }
        #endregion

        #region 内部报告处理逻辑
        protected virtual void FillReport(ReportReportElement rre, LisReportPK RK)
        {
            //默认的报告项
            FillElement(rre, RK);
            //可选的报告项
            List<Type> availableElementList = this.GetAvailableInsideElements(RK);
            if (availableElementList != null && availableElementList.Count > 0)
            {
                List<ILisReportElement> tempList = null;
                foreach (Type type in availableElementList)
                {
                    tempList = rre.GetReportItem(type);
                    FillElements(tempList, RK, type);
                }
            }
        }
        #endregion

        #region 抽象方法
        protected abstract void FillElement(ILisReportElement reportElement, LisReportPK RK);
        protected abstract void FillElements(List<ILisReportElement> reportElementList, LisReportPK RK, Type type);
        #endregion

        #region 辅助方法
        protected virtual List<Type> GetAvailableInsideElements(LisReportPK RK)
        {
            if (this.m_section2FillTypeMap.Count == 0)
            {
                InitFillElementTable();
            }
            return this.m_section2FillTypeMap[RK.SectionNo] as List<Type>;
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
        protected bool IsReport(Type type)
        {
            return typeof(ReportReportElement).Equals(type);
        }
        protected bool IsReport(ILisReportElement reportElement)
        {
            return reportElement is ReportReportElement;
        }
        #endregion
    }
}