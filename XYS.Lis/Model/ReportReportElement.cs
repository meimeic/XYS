﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using XYS.Common;
using XYS.Lis.Core;
using XYS.Lis.Model;
using XYS.Lis.Util;

namespace XYS.Lis.Model
{
    public class ReportReportElement : AbstractReportElement
    {

        #region 私有常量字段
        private static readonly ReportElementTag m_defaultElementTag = ReportElementTag.ReportElement;
        #endregion

        #region 私有实例字段
        private int m_orderNo;
        private int m_printModelNo;
        private string m_reportTitle;
        private string m_remark;
        private bool m_remarkFlag;
        private string m_parItemName;

        private byte[] m_technicianImage;
        private byte[] m_checkerImage;

        private DateTime m_receiveDateTime;
        private DateTime m_collectDateTime;
        private DateTime m_inceptDateTime;
        private DateTime m_testDateTime;
        private DateTime m_checkDateTime;
        private DateTime m_secondCheckDateTime;

        private int m_sectionNo;
        private ClinicType m_clinicType;

        private readonly List<int> m_parItemList;
        private readonly Hashtable m_reportItemTable;
        #endregion

        #region 公共构造函数
        public ReportReportElement()
            : base(m_defaultElementTag, "")
        {
            this.m_parItemList = new List<int>(5);
            this.m_reportItemTable = new Hashtable(5);
            this.m_remarkFlag = false;
        }
        #endregion

        #region 实例属性
        [Export()]
        public int OrderNo
        {
            get { return this.m_orderNo; }
            set { this.m_orderNo = value; }
        }
        [Export()]
        public int PrintModelNo
        {
            get { return this.m_printModelNo; }
            set { this.m_printModelNo = value; }
        }
        public bool RemarkFlag
        {
            get { return this.m_remarkFlag; }
            set { this.m_remarkFlag = value; }
        }

        [Export()]
        public DateTime ReceiveDateTime
        {
            get { return this.m_receiveDateTime; }
            set { this.m_receiveDateTime = value; }
        }
        [Export()]
        public DateTime CollectDateTime
        {
            get { return this.m_collectDateTime; }
            set { this.m_collectDateTime = value; }
        }
        [Export()]
        public DateTime InceptDateTime
        {
            get { return m_inceptDateTime; }
            set { m_inceptDateTime = value; }
        }
        [Export()]
        public DateTime TestDateTime
        {
            get { return m_testDateTime; }
            set { m_testDateTime = value; }
        }
        [Export()]
        public DateTime CheckDateTime
        {
            get { return m_checkDateTime; }
            set { m_checkDateTime = value; }
        }
        [Export()]
        public DateTime SecondeCheckDateTime
        {
            get { return m_secondCheckDateTime; }
            set { m_secondCheckDateTime = value; }
        }

        [Export()]
        public string ReportTitle
        {
            get { return this.m_reportTitle; }
            set { this.m_reportTitle = value; }
        }

        [Export()]
        public string Remark
        {
            get { return this.m_remark; }
            set { this.m_remark = value; }
        }

        [Export()]
        public string ParItemName
        {
            get { return this.m_parItemName; }
            set { this.m_parItemName = value; }
        }
        [Export()]
        public byte[] TechnicianImage
        {
            get { return this.m_technicianImage; }
            set { this.m_technicianImage = value; }
        }
        [Export()]
        public byte[] CheckerImage
        {
            get { return this.m_checkerImage; }
            set { this.m_checkerImage = value; }
        }

        public int SectionNo
        {
            get { return this.m_sectionNo; }
            set { this.m_sectionNo = value; }
        }
        public ClinicType ClinicType
        {
            get { return this.m_clinicType; }
            set { this.m_clinicType = value; }
        }
        public List<int> ParItemList
        {
            get { return this.m_parItemList; }
        }
        public Hashtable ReportItemTable
        {
            get { return this.m_reportItemTable; }
        }
        #endregion

        #region 实现父类抽象方法
        protected override void Afterward()
        {
        }
        #endregion

        #region 公共方法
        public void Init()
        {

        }
        public void ReportClear()
        {
            this.ParItemList.Clear();
            this.ReportItemTable.Clear();
            this.OrderNo = 0;
            this.PrintModelNo = -1;
            this.ReportTitle = "";
            this.RemarkFlag = false;
            this.Remark = "";
            this.ClinicType = ClinicType.none;
            this.ParItemName = null;
            this.TechnicianImage = null;
            this.CheckerImage = null;
        }
        public List<IReportElement> GetReportItem(ReportElementTag elementTag)
        {
            List<IReportElement> result = this.m_reportItemTable[elementTag] as List<IReportElement>;
            if (result == null)
            {
                result = new List<IReportElement>(10);
                lock (this.m_reportItemTable)
                {
                    this.m_reportItemTable[elementTag] = result;
                }
            }
            return result;
        }
        #endregion
    }
}
