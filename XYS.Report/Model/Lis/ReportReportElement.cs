using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using XYS.Util;
using XYS.Util;
using XYS.Report.Core;
using XYS.Report.Util;
namespace XYS.Report.Model.Lis
{
    public class ReportReportElement : ILisReportElement
    {

        #region 私有只读字段
        private readonly ReportElementTag m_elementTag;
        #endregion

        #region 私有实例字段
        private int m_sectionNo;
        private string m_reportTitle;
        private int m_remarkFlag;
        private string m_remark;
        private string m_parItemName;

        private byte[] m_technicianImage;
        private byte[] m_checkerImage;

        private DateTime m_receiveDateTime;
        private DateTime m_collectDateTime;
        private DateTime m_inceptDateTime;
        private DateTime m_testDateTime;
        private DateTime m_checkDateTime;
        private DateTime m_secondCheckDateTime;

        private readonly List<int> m_parItemList;
        private readonly Hashtable m_reportItemTable;

        private readonly ReportExamElement m_reportExam;
        private readonly ReportPatientElement m_reportPatient;
        #endregion

        #region 公共构造函数
        public ReportReportElement()
        {
            this.m_remarkFlag = 0;
            this.m_parItemList = new List<int>(5);
            this.m_elementTag = ReportElementTag.Report;
            this.m_reportExam = new ReportExamElement();
            this.m_reportPatient = new ReportPatientElement();
            this.m_reportItemTable = SystemInfo.CreateCaseInsensitiveHashtable(3);
        }
        #endregion

        #region 实现IReportElement接口属性
        public ReportElementTag ElementTag
        {
            get { return this.m_elementTag; }
        }
        #endregion

        #region 实例属性
        [FRConvert()]
        public int SectionNo
        {
            get { return this.m_sectionNo; }
            set { this.m_sectionNo = value; }
        }
        [FRConvert()]
        public string ReportTitle
        {
            get { return this.m_reportTitle; }
            set { this.m_reportTitle = value; }
        }
        //备注标识 0表示没有备注 1 表示备注已设置  2 表示备注未设置
        public int RemarkFlag
        {
            get { return this.m_remarkFlag; }
            set { this.m_remarkFlag = value; }
        }
        [FRConvert()]
        public string Remark
        {
            get { return this.m_remark; }
            set { this.m_remark = value; }
        }
         [FRConvert()]
        public string ParItemName
        {
            get { return this.m_parItemName; }
            set { this.m_parItemName = value; }
        }

        [FRConvert()]
        public DateTime ReceiveDateTime
        {
            get { return this.m_receiveDateTime; }
            set { this.m_receiveDateTime = value; }
        }
        [FRConvert()]
        public DateTime CollectDateTime
        {
            get { return this.m_collectDateTime; }
            set { this.m_collectDateTime = value; }
        }
        [FRConvert()]
        public DateTime InceptDateTime
        {
            get { return m_inceptDateTime; }
            set { m_inceptDateTime = value; }
        }
        [FRConvert()]
        public DateTime TestDateTime
        {
            get { return m_testDateTime; }
            set { m_testDateTime = value; }
        }
        [FRConvert()]
        public DateTime CheckDateTime
        {
            get { return m_checkDateTime; }
            set { m_checkDateTime = value; }
        }
        [FRConvert()]
        public DateTime SecondeCheckDateTime
        {
            get { return m_secondCheckDateTime; }
            set { m_secondCheckDateTime = value; }
        }

        [FRConvert()]
        public byte[] TechnicianImage
        {
            get { return this.m_technicianImage; }
            set { this.m_technicianImage = value; }
        }
        [FRConvert()]
        public byte[] CheckerImage
        {
            get { return this.m_checkerImage; }
            set { this.m_checkerImage = value; }
        }

        public List<int> ParItemList
        {
            get { return this.m_parItemList; }
        }
        public Hashtable ReportItemTable
        {
            get { return this.m_reportItemTable; }
        }
        public ReportExamElement ReportExam
        {
            get { return this.m_reportExam; }
        }
        public ReportPatientElement ReportPatient
        {
            get { return this.m_reportPatient; }
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
            this.ReportTitle = "";
            this.RemarkFlag = 0;
            this.Remark = "";
            this.TechnicianImage = null;
            this.CheckerImage = null;
        }
        public List<ILisReportElement> GetReportItem(string typeName)
        {
            if (!string.IsNullOrEmpty(typeName))
            {
                List<ILisReportElement> result = this.m_reportItemTable[typeName] as List<ILisReportElement>;
                if (result == null)
                {
                    result = new List<ILisReportElement>(10);
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
