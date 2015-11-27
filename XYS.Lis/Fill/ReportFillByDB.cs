using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Common;
using XYS.Model;
using XYS.Lis.Core;
using XYS.Lis.Model;
using XYS.Lis.DAL;

namespace XYS.Lis.Fill
{
    public class ReportFillByDB : ReportFillSkeleton
    {
        #region 变量
        private static readonly string m_FillerName = "DBFiller";
        private LisReportCommonDAL m_reportDAL;
        #endregion

        #region 构造函数
        public ReportFillByDB()
            : this(m_FillerName)
        {
        }
        public ReportFillByDB(string name)
            : base(name)
        {
            this.m_reportDAL = new LisReportCommonDAL();
        }
        #endregion

        #region 实例属性
        public virtual LisReportCommonDAL ReportDAL
        {
            get
            {
                if (this.m_reportDAL == null)
                {
                    this.m_reportDAL = new LisReportCommonDAL();
                }
                return this.m_reportDAL;
            }
            set { this.m_reportDAL = value; }
        }
        #endregion

        #region 实现父类抽象方法
        protected override void FillReport(ReportReportElement rre, ReportKey key)
        {
            int sectionNo = GetSectionNo(key);
            //获取需要填充的报告元素
            ReportElementTypeCollection availableElements = this.GetAvailableElements(sectionNo);
            Hashtable keyTable = ReportKey2Table(key);
            this.FillReport(rre, keyTable, availableElements);
        }
        protected override void FillElement(ILisReportElement reportElement, ReportKey key)
        {
            //不进行填充的报告元素
            if (reportElement.ElementTag == ReportElementTag.ReportElement || reportElement.ElementTag == ReportElementTag.KVElement)
            {
                return;
            }
            Hashtable keyTable = ReportKey2Table(key);
            FillElement(reportElement, keyTable);
        }
        protected override void FillElements(List<ILisReportElement> reportElementList, ReportKey key, ReportElementTag elementTag)
        {
            //不进行填充的报告元素
            if (elementTag == ReportElementTag.ReportElement || elementTag == ReportElementTag.KVElement)
            {
                return;
            }
            int sectionNo = GetSectionNo(key);
            Type type = this.GetElementType(sectionNo, elementTag);
            Hashtable keyTable = ReportKey2Table(key);
            if (type != null)
            {
                this.FillElements(reportElementList, type, keyTable);
            }
        }
        #endregion

        #region 内部逻辑处理
        protected virtual void FillReport(ReportReportElement rre, Hashtable keyTable, ReportElementTypeCollection availableElements)
        {
            Type tempType;
            ReportElementTag tempTag;
            foreach (ReportElementType elementType in availableElements)
            {
                tempTag = elementType.ElementTag;
                tempType = elementType.ElementType;
                if (tempTag == ReportElementTag.ReportElement || tempTag == ReportElementTag.CustomElement || tempTag == ReportElementTag.NoneElement)
                {
                    //过滤不需要填充的元素
                    continue;
                }
                this.FillElements(rre.GetReportItem(tempTag), tempType, keyTable);
            }
            rre.AfterFill();
        }
        #endregion

        #region DAL层代码访问
        protected virtual void FillElement(ILisReportElement reportElement, Hashtable keyTable)
        {
            this.ReportDAL.Fill(reportElement, keyTable);
        }
        protected virtual void FillElements(List<ILisReportElement> reportElementList, Type elementType, Hashtable keyTable)
        {
            this.ReportDAL.FillList(reportElementList, elementType, keyTable);
        }
        #endregion 

        #region 辅助方法
        protected virtual int GetSectionNo(ReportKey key)
        {
            int sectionNo = 0;
            foreach (KeyColumn c in key.KeySet)
            {
                if (c.Name.ToLower() == "sectionno")
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
        protected virtual Hashtable ReportKey2Table(ReportKey keys)
        {
            Hashtable keyTable = new Hashtable();
            foreach (KeyColumn key in keys.KeySet)
            {
                keyTable.Add(key.Name, key.PK);
            }
            return keyTable;
        }
        #endregion

        #region 未调用方法
        //protected override void FillElements(Hashtable reportElementTable, ReportKey key, ReportElementTag elementTag)
        //{
        //    if (elementTag == ReportElementTag.ReportElement || elementTag == ReportElementTag.NoneElement)
        //    {
        //        //错误
        //        return;
        //    }
        //    int sectionNo = GetSectionNo(key);
        //    Type type = this.GetElementType(sectionNo, elementTag);
        //    Hashtable keyTable = ReportKey2Table(key);
        //    if (type != null)
        //    {
        //        this.FillElements(reportElementTable, type, keyTable);
        //    }
        //}
        //protected virtual void FillReportItem(ReportReportElement rre, ReportElementTag elementTag, Type elementType, Hashtable keyTable)
        //{
        //    ILisReportElement element = (ILisReportElement)elementType.Assembly.CreateInstance(elementType.FullName);
        //    FillElement(element, keyTable);
        //    rre.AddItem(elementTag, element);
        //}
        //protected virtual void FillReportItemTable(ReportReportElement rre, ReportElementTag elementTag, Type elementType, Hashtable keyTable)
        //{
        //    Hashtable table = new Hashtable(16);
        //    FillElements(table, elementType, keyTable);
        //    rre.AddTableItem(elementTag, table);
        //}
        //protected virtual void FillElements(Hashtable reportElementTable, Type elementType, Hashtable keyTable)
        //{
        //    this.ReportDAL.FillTable(reportElementTable, elementType, keyTable);
        //}
        #endregion
    }
}
