﻿using System;
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
        protected override void InnerFillElement(ILisReportElement reportElement, Hashtable keyTable)
        {
            FillElement(reportElement, keyTable);
        }
        protected override void InnerFillElements(List<ILisReportElement> reportElementList, Hashtable keyTable, Type elementType)
        {
            FillElements(reportElementList, keyTable, elementType);
        }
        #endregion

        #region DAL层代码访问
        protected virtual void FillElement(ILisReportElement reportElement, Hashtable keyTable)
        {
            this.ReportDAL.Fill(reportElement, keyTable);
        }
        protected virtual void FillElements(List<ILisReportElement> reportElementList, Hashtable keyTable, Type elementType)
        {
            this.ReportDAL.FillList(reportElementList, elementType, keyTable);
        }
        #endregion
    }
}
