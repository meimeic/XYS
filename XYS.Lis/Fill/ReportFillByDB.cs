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
        #region
        private static readonly string m_FillerName = "DBFiller";
        private LisReportCommonDAL m_reportDAL;
        #endregion

        #region
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

        #region
        public virtual LisReportCommonDAL ReportDAL
        {
            get
            {
                if (this.m_reportDAL == null)
                {
                    return new LisReportCommonDAL();
                }
                else
                {
                    return this.m_reportDAL;
                }
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
            //填充报告
            Hashtable keyTable = ReportKey2Table(key);
            this.FillReport(rre, keyTable, availableElements);
        }

        protected override void FillElements(Hashtable reportElementTable, ReportKey key, ReportElementTag elementTag)
        {
            if (elementTag == ReportElementTag.ReportElement || elementTag == ReportElementTag.NoneElement)
            {
                //错误
                return;
            }
            int sectionNo = GetSectionNo(key);
            Type type = this.GetElementType(sectionNo, elementTag);
            Hashtable keyTable = ReportKey2Table(key);
            if (type != null)
            {
                this.FillElements(reportElementTable, type, keyTable);
            }
        }

        protected override void FillElement(ILisReportElement reportElement, ReportKey key)
        {
            if (reportElement.ElementTag == ReportElementTag.ReportElement)
            {
                //填充报告
                return;
            }
            Hashtable keyTable = ReportKey2Table(key);
            FillElement(reportElement, keyTable);
        }

        #endregion

        #region
        protected virtual void FillReport(ReportReportElement rre, Hashtable keyTable, ReportElementTypeCollection availableElements)
        {
            foreach (ReportElementType elementtype in availableElements)
            {
                switch (elementtype.ElementTag)
                {
                    //报告元素---检验信息项
                    //报告元素---患者信息项
                    case ReportElementTag.ExamElement:
                    case ReportElementTag.PatientElement:
                        FillReportItem(rre,elementtype.ElementTag, elementtype.ElementType, keyTable);                    
                        break;
                    //报告元素---检验项
                    //报告元素---图片项
                    case ReportElementTag.ItemElement:
                    case ReportElementTag.GraphElement:
                        lock (this)
                        {
                            FillReportItemTable(rre, elementtype.ElementTag, elementtype.ElementType, keyTable);
                        }
                        break;
                    //报告元素---自定义项
                    case ReportElementTag.CustomElement:
                        //FillElements(rrElement.CustomItemList, elementtype.ElementType, keyTable);
                        break;
                    //报告元素----报告
                    case ReportElementTag.ReportElement:
                        break;
                    default:
                        break;
                }
            }
            rre.AfterFill();
        }
        protected virtual void FillReportItem(ReportReportElement rre,ReportElementTag elementTag,Type elementType,Hashtable keyTable)
        {
            ILisReportElement element = (ILisReportElement)elementType.Assembly.CreateInstance(elementType.FullName);
            FillElement(element, keyTable);
            rre.AddItem(elementTag, element);
        }
        protected virtual void FillReportItemTable(ReportReportElement rre, ReportElementTag elementTag, Type elementType, Hashtable keyTable)
        {
            Hashtable table = new Hashtable(16);
            FillElements(table,elementType, keyTable);
            rre.AddTableItem(elementTag, table);
        }
        
        protected virtual void FillElement(ILisReportElement reportElement, Hashtable keyTable)
        {
            this.ReportDAL.Fill(reportElement, keyTable);
        }
        protected virtual void FillElements(Hashtable reportElementTable,Type elementType, Hashtable keyTable)
        {
            this.ReportDAL.FillTable(reportElementTable, elementType, keyTable);
        }
        
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
    }
}
