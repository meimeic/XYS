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
        private ReportElementType m_defaultReportElementType;
        private readonly Hashtable m_section2ElementTypesMap;
        private readonly Hashtable m_defaultTag2ElementTypeMap;
        #endregion

        #region 构造函数
        protected ReportFillSkeleton(string name)
        {
            this.m_fillerName = name;
            this.m_section2ElementTypesMap = new Hashtable(20);
            this.m_defaultTag2ElementTypeMap = new Hashtable(5);
            this.m_defaultReportElementType = ReportElementType.DEFAULTINFO;
        }
        #endregion

        #region 实例属性
        public ReportElementType DefaultReportElementType
        {
            get
            {
                return this.m_defaultReportElementType;
            }
            set { this.m_defaultReportElementType = value; }
        }
        #endregion

        #region 实现IReportFiller接口
        public virtual string FillerName
        {
            get { return this.m_fillerName.ToLower(); }
        }
        public virtual void Fill(IReportElement reportElement, ReportKey PK)
        {
            //填充报告
            if (reportElement.ElementTag == ReportElementTag.ReportElement)
            {
                ReportReportElement rre = reportElement as ReportReportElement;
                if (rre != null)
                {
                    FillReport(rre, PK);
                }
            }
            //填充元素
            else if (reportElement.ElementTag != ReportElementTag.NoneElement)
            {
                FillElement(reportElement, PK);
            }
            else
            {
                return;
            }
        }
        public virtual void Fill(List<IReportElement> reportElementList, ReportKey PK, ReportElementTag elementTag)
        {
            //不可填充的报告元素
            if (elementTag == ReportElementTag.NoneElement)
            {
                return;
            }
            else
            {
                FillElements(reportElementList, PK, elementTag);
            }
        }
        #endregion

        #region 内部处理逻辑
        protected virtual void FillReport(ReportReportElement rre, ReportKey PK)
        {
            Hashtable keyTable = ReportKey2Table(PK);
            //默认项填充
            List<IReportElement> defaultElementList = rre.GetReportItem(DefaultReportElementType.ElementTag);
            InnerFillElements(defaultElementList, keyTable, DefaultReportElementType.ElementType);
            //可选项填充
            ReportElementTypeCollection availableElements = this.GetAvailableElements(PK);
            if (availableElements != null && availableElements.Count > 0)
            {
                foreach (ReportElementType elementType in availableElements)
                {
                    InnerFillElements(rre.GetReportItem(elementType.ElementTag), keyTable, elementType.ElementType);
                }
            }
            //数据处理
            rre.AfterFill();
        }
        protected virtual void FillElement(IReportElement reportElement, ReportKey PK)
        {
            //不进行填充的报告元素
            if (reportElement.ElementTag == ReportElementTag.KVElement)
            {
                return;
            }
            Hashtable keyTable = ReportKey2Table(PK);
            InnerFillElement(reportElement, keyTable);
        }
        protected virtual void FillElements(List<IReportElement> reportElementList, ReportKey PK, ReportElementTag elementTag)
        {
            //不填充处理
            if (elementTag == ReportElementTag.ReportElement||elementTag==ReportElementTag.KVElement)
            {
                return;
            }
            else
            {
                //元素集合
                Hashtable keyTable = ReportKey2Table(PK);
                Type elementType = GetElementType(PK, elementTag);
                InnerFillElements(reportElementList, keyTable, elementType);
            }
        }
        #endregion
        
        #region 抽象方法
        protected abstract void InnerFillElement(IReportElement reportElement, Hashtable keyTable);
        protected abstract void InnerFillElements(List<IReportElement> reportElementList,Hashtable keyTable,Type elementType);
        #endregion

        #region 辅助方法
        //获取每个小组特定的元素类型
        protected virtual ReportElementTypeCollection GetAvailableElements(int sectionNo)
        {
            if (this.m_section2ElementTypesMap.Count == 0)
            {
                InitElementTypesTable();
            }
            ReportElementTypeCollection retc = this.m_section2ElementTypesMap[sectionNo] as ReportElementTypeCollection;
            return retc;
        }
        protected virtual ReportElementTypeCollection GetAvailableElements(ReportKey key)
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
                        sectionNo = Convert.ToInt32(c.PK);
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
        
        //将键对象转换成table对象
        protected virtual Hashtable ReportKey2Table(ReportKey key)
        {
            Hashtable keyTable = new Hashtable(5);
            foreach (KeyColumn k in key.KeySet)
            {
                keyTable.Add(k.Name, k.PK);
            }
            return keyTable;
        }
        protected Type GetElementType(ReportKey key, ReportElementTag elementTag)
        {
            int sectionNo = GetSectionNo(key);
            return GetElementType(sectionNo, elementTag);
        }
        protected Type GetElementType(int sectionNo, ReportElementTag elementTag)
        {
            Type type = null;
            ReportElementTypeCollection availableElements = this.GetAvailableElements(sectionNo);
            foreach (ReportElementType elementType in availableElements)
            {
                if (elementType.ElementTag == elementTag)
                {
                    type = elementType.ElementType;
                    break;
                }
            }
            if (type == null)
            {
                type = GetDefaultType(elementTag);
            }
            return type;
        }
        protected Type GetDefaultType(ReportElementTag elementTag)
        {
            if (this.m_defaultTag2ElementTypeMap.Count == 0)
            {
                InitTag2TypeTable();
            }
            return this.m_defaultTag2ElementTypeMap[elementTag] as Type;
        }
        private void InitElementTypesTable()
        {
            lock (this.m_section2ElementTypesMap)
            {
                LisMap.InitSection2ElementTypeTable(this.m_section2ElementTypesMap);
            }
        }
        private void InitTag2TypeTable()
        {

        }
        #endregion
    }
}
