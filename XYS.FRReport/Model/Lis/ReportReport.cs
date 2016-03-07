using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Common;
using XYS.FRReport.Model;
namespace XYS.FRReport.Model.Lis
{
    [FRExport()]
    public class ReportReport : IFRExportElement
    {
        private DateTime m_receiveDateTime;
        private DateTime m_collectDateTime;
        private DateTime m_inceptDateTime;
        private DateTime m_testDateTime;
        private DateTime m_checkDateTime;
        private DateTime m_secondCheckDateTime;

        private int m_sectionNo;
        private int m_orderNo;
        private int m_printModelNo;

        private string m_remark;
        private string m_reportTitle;
        private string m_parItemName;

        private byte[] m_checkerImage;
        private byte[] m_technicianImage;

        private readonly Hashtable m_reportItemTable;

        private readonly ReportExam m_reportExam;
        private readonly ReportPatient m_reportPatient;
        //private IExportElement m_reportImage;
        private readonly List<int> m_parItemList;

        public ReportReport()
        {
            this.m_reportExam = new ReportExam();
            this.m_reportPatient = new ReportPatient();
            this.m_reportItemTable = new Hashtable(2);
            this.m_parItemList = new List<int>(3);
        }

        #region 属性
        [FRExport()]
        public DateTime ReceiveDateTime
        {
            get { return m_receiveDateTime; }
            set { m_receiveDateTime = value; }
        }
        [FRExport()]
        public DateTime CollectDateTime
        {
            get { return m_collectDateTime; }
            set { m_collectDateTime = value; }
        }
        [FRExport()]
        public DateTime InceptDateTime
        {
            get { return m_inceptDateTime; }
            set { m_inceptDateTime = value; }
        }
        [FRExport()]
        public DateTime TestDateTime
        {
            get { return m_testDateTime; }
            set { m_testDateTime = value; }
        }
        [FRExport()]
        public DateTime CheckDateTime
        {
            get { return m_checkDateTime; }
            set { m_checkDateTime = value; }
        }
        [FRExport()]
        public DateTime SecondeCheckDateTime
        {
            get { return m_secondCheckDateTime; }
            set { m_secondCheckDateTime = value; }
        }

        public int SectionNo
        {
            get { return this.m_sectionNo; }
            set { this.m_sectionNo = value; }
        }
        public int OrderNo
        {
            get { return this.m_orderNo; }
            set { this.m_orderNo = value; }
        }
        public int PrintModelNo
        {
            get { return this.m_printModelNo; }
            set { this.m_printModelNo = value; }
        }
        [FRExport()]
        public string Remark
        {
            get { return this.m_remark; }
            set { this.m_remark = value; }
        }
        [FRExport()]
        public string ReportTitle
        {
            get { return this.m_reportTitle; }
            set { this.m_reportTitle = value; }
        }
        [FRExport()]
        public string ParItemName
        {
            get { return this.m_parItemName; }
            set { this.m_parItemName = value; }
        }

        [FRExport()]
        public byte[] TechnicianImage
        {
            get { return this.m_technicianImage; }
            set { this.m_technicianImage = value; }
        }
        [FRExport()]
        public byte[] CheckerImage
        {
            get { return this.m_checkerImage; }
            set { this.m_checkerImage = value; }
        }

        public ReportExam ReportExam
        {
            get { return this.m_reportExam; }
        }
        public ReportPatient ReportPatient
        {
            get { return this.m_reportPatient; }
        }
        //public IExportElement ReportImage
        //{
        //    get { return this.m_reportImage; }
        //    set { this.m_reportImage = value; }
        //}
        public Hashtable ReportItemTable
        {
            get { return this.m_reportItemTable; }
        }
        public List<int> ParItemList
        {
            get { return this.m_parItemList; }
        }
        #endregion

        #region
        public void Clear()
        {

        }
        public List<IFRExportElement> GetReportItem(string typeName)
        {
            if (!string.IsNullOrEmpty(typeName))
            {
                List<IFRExportElement> result = this.m_reportItemTable[typeName] as List<IFRExportElement>;
                if (result == null)
                {
                    result = new List<IFRExportElement>(10);
                    lock (this.m_reportItemTable)
                    {
                        this.m_reportItemTable[typeName] = result;
                    }
                }
                return result;
            }
            return null;
        }
        #endregion
    }
}
