using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Model;
using XYS.Lis.Core;
namespace XYS.Lis.Model.Export
{
    public class ReportReport : ILisExportElement
    {
        private static readonly ReportElementTag Default_Tag = ReportElementTag.ReportElement;

        private DateTime m_receiveDateTime;
        private DateTime m_collectDateTime;
        private DateTime m_inceptDateTime;
        private DateTime m_testDateTime;
        private DateTime m_checkDateTime;
        private DateTime m_secondCheckDateTime;

        private int m_orderNo;
        private string m_remark;
        private string m_reportTitle;
        private int m_printModelNo;
        private string m_parItemName;

        private byte[] m_checkerImage;
        private byte[] m_technicianImage;

        private ReportInfo m_reportInfo;
        private Dictionary<int, ReportItem> m_no2Item;
        private Dictionary<string, ReportGraph> m_name2Graph;
        private Dictionary<string, ReportCustom> m_name2Custom;

        public ReportReport()
        {
            this.m_reportInfo = new ReportInfo();
            this.m_no2Item = new Dictionary<int, ReportItem>(16);
            this.m_name2Graph = new Dictionary<string, ReportGraph>(2);
            this.m_name2Custom = new Dictionary<string, ReportCustom>(1);
        }
        #region 实现接口方法
        public ReportElementTag ElementTag
        {
            get { return Default_Tag; }
        }
        #endregion

        #region
        public DateTime ReceiveDateTime
        {
            get { return m_receiveDateTime; }
            set { m_receiveDateTime = value; }
        }
        public DateTime CollectDateTime
        {
            get { return m_collectDateTime; }
            set { m_collectDateTime = value; }
        }
        public DateTime InceptDateTime
        {
            get { return m_inceptDateTime; }
            set { m_inceptDateTime = value; }
        }
        public DateTime TestDateTime
        {
            get { return m_testDateTime; }
            set { m_testDateTime = value; }
        }
        public DateTime CheckDateTime
        {
            get { return m_checkDateTime; }
            set { m_checkDateTime = value; }
        }
        public DateTime SecondeCheckDateTime
        {
            get { return m_secondCheckDateTime; }
            set { m_secondCheckDateTime = value; }
        }

        public int OrderNo
        {
            get { return this.m_orderNo; }
            set { this.m_orderNo = value; }
        }
        public string Remark
        {
            get { return this.m_remark; }
            set { this.m_remark = value; }
        }
        public string ReportTitle
        {
            get { return this.m_reportTitle; }
            set { this.m_reportTitle = value; }
        }
        public int PrintModelNo
        {
            get { return this.m_printModelNo; }
            set { this.m_printModelNo = value; }
        }
        public string ParItemName
        {
            get { return this.m_parItemName; }
            set { this.m_parItemName = value; }
        }

        public byte[] TechnicianImage
        {
            get { return this.m_technicianImage; }
            set { this.m_technicianImage = value; }
        }
        public byte[] CheckerImage
        {
            get { return this.m_checkerImage; }
            set { this.m_checkerImage = value; }
        }

        public ReportInfo ReportInfo
        {
            get { return this.m_reportInfo; }
        }
        public Dictionary<int, ReportItem> ItemTable
        {
            get { return this.m_no2Item; }
        }
        public Dictionary<string, ReportGraph> GraphTable
        {
            get { return this.m_name2Graph; }
        }
        public Dictionary<string, ReportCustom> CustomTable
        {
            get { return this.m_name2Custom; }
        }
        #endregion
    }
}
