using System;
using System.Collections.Generic;
using Newtonsoft.Json;

using XYS.Model; 
namespace XYS.Lis.Service.Models.Report
{
    public class ReportReport:IReportModel
    {
        private static readonly ReportElementTag Default_Element = ReportElementTag.ReportElement; 

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

        private readonly List<int> m_parItemList;
        private ReportInfo m_reportInfo;
        private List<ReportItem> m_itemList;
        private List<ReportGraph> m_graphList;
        private List<ReportCustom> m_customList;
        private List<ReportKV> m_kvList;

        public ReportReport()
        {
            this.m_reportInfo = new ReportInfo();
            this.m_parItemList = new List<int>(3);
            this.m_itemList = null;
            this.m_graphList = null;
            this.m_customList = null;
            this.m_kvList = null;
        }

        #region 实现ireportmodel接口
        [JsonIgnore]
        public ReportElementTag ElementTag
        {
            get { return Default_Element; }
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

        [JsonIgnore]
        public byte[] TechnicianImage
        {
            get { return this.m_technicianImage; }
            set { this.m_technicianImage = value; }
        }
        [JsonIgnore]
        public byte[] CheckerImage
        {
            get { return this.m_checkerImage; }
            set { this.m_checkerImage = value; }
        }
        [JsonIgnore]
        public List<int> ParItemList
        {
            get { return this.m_parItemList; }
        }

        public ReportInfo ReportInfo
        {
            get { return this.m_reportInfo; }
        }
        public List<ReportItem> ItemList
        {
            get { return this.m_itemList; }
            set
            {
                lock (this)
                {
                    this.m_itemList = value;
                }
            }
        }
        public List<ReportGraph> GraphList
        {
            get { return this.m_graphList; }
            set
            {
                lock (this)
                {
                    this.m_graphList = value;
                }
            }
        }
        public List<ReportCustom> CustomList
        {
            get { return this.m_customList; }
            set
            {
                lock (this)
                {
                    this.m_customList = value;
                }
            }
        }
        public List<ReportKV> KVList
        {
            get { return this.m_kvList; }
            set
            {
                lock (this)
                {
                    this.m_kvList = value;
                }
            }
        }
        #endregion
    }
}
