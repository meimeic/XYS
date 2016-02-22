using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Common;
using XYS.Lis.Core;
namespace XYS.Lis.Export.Model
{
    [Export()]
    public class ReportReport : IExportElement
    {
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

        private object m_checkerImage;
        private object m_technicianImage;

        private ReportExam m_reportExam;
        private ReportPatient m_reportPatient;
        private ReportImage m_reportGraph;
        private List<ReportItem> m_reportItemList;
        private List<ReportCustom> m_reportCustomList;
        private readonly List<int> m_parItemList;

        public ReportReport()
        {
            this.m_parItemList = new List<int>(3);
            this.m_reportExam = null;
            this.m_reportPatient = null;
            this.m_reportGraph = null;

            this.m_reportItemList = null;
            this.m_reportCustomList = null;
        }

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

        public object TechnicianImage
        {
            get { return this.m_technicianImage; }
            set { this.m_technicianImage = value; }
        }
        public object CheckerImage
        {
            get { return this.m_checkerImage; }
            set { this.m_checkerImage = value; }
        }
        public List<int> ParItemList
        {
            get { return this.m_parItemList; }
        }

        public List<ReportItem> ItemList
        {
            get { return this.m_reportItemList; }
            set
            {
                lock (this)
                {
                    this.m_reportItemList = value;
                }
            }
        }
        public List<ReportImage> GraphList
        {
            get { return this.m_reportGraph; }
            set
            {
                lock (this)
                {
                    this.m_reportGraph = value;
                }
            }
        }
        public List<ReportCustom> CustomList
        {
            get { return this.m_reportCustomList; }
            set
            {
                lock (this)
                {
                    this.m_reportCustomList = value;
                }
            }
        }

        #endregion

        #region
        //public Dictionary<int, ReporterItem> ItemTable
        //{
        //    get { return this.m_no2Item; }
        //    set
        //    {
        //        lock (this)
        //        {
        //            this.m_no2Item = value;
        //        }
        //    }
        //}
        //public Dictionary<string, ReporterGraph> GraphTable
        //{
        //    get { return this.m_name2Graph; }
        //    set
        //    {
        //        lock (this)
        //        {
        //            this.m_name2Graph = value;
        //        }
        //    }
        //}
        //public Dictionary<string, ReporterCustom> CustomTable
        //{
        //    get { return this.m_name2Custom; }
        //    set
        //    {
        //        lock (this)
        //        {
        //            this.m_name2Custom = value;
        //        }
        //    }
        //}
        //public Dictionary<string, ReporterKV> KVTable
        //{
        //    get { return this.m_name2KV; }
        //    set
        //    {
        //        lock (this)
        //        {
        //            this.m_name2KV = value;
        //        }
        //    }
        //}
        #endregion
    }
}
