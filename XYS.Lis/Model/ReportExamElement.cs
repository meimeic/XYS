﻿using System;

using XYS.Common;
using XYS.Lis.Core;
using XYS.Lis.Util;
namespace XYS.Lis.Model
{
    public class ReportExamElement : AbstractReportElement
    {
        #region 私有常量字段
        private const ReportElementTag m_defaultElementTag = ReportElementTag.InfoElement;
        private static readonly string m_defaultEaxmSQL = @"select serialno,sampleno,s.CName as sampletypename,CAST(CONVERT(varchar(10), collectdate, 121) + ' ' + CONVERT(varchar(8), collecttime, 114) AS datetime) as collectdatetime,CAST(CONVERT(varchar(10), inceptdate, 121) + ' ' + CONVERT(varchar(8), incepttime, 114) AS datetime) as inceptdatetime,
                                                                             CAST(CONVERT(varchar(10), testdate, 121) + ' ' + CONVERT(varchar(8), testtime, 114) AS datetime) as testdatetime,CAST(CONVERT(varchar(10), checkdate, 121) + ' ' + CONVERT(varchar(8), checktime, 114) AS datetime) as checkdatetime,
                                                                             CAST(CONVERT(varchar(10), receivedate, 121) + ' ' + CONVERT(varchar(8), receivetime, 114) AS datetime) as receivedatetime,sendertime2 as secondecheckdatetime,paritemname,sectionno,r.sampletypeno,formmemo,formcomment,formcomment2,technician,checker
                                                                             from ReportForm as r left outer join SampleType as s on r.SampleTypeNo=s.SampleTypeNo";
        #endregion

        #region 私有字段
        private string m_serialNo;
        private string m_sampleNo;
        private int m_sectionNo;
        private int m_sampleTypeNo;
        private string m_sampleTypeName;
        //采集、签收、审核时,二次审核,核收时间
        //private DateTime m_collectDate;
        //private DateTime m_collectTime;
        private DateTime m_collectDateTime;
        //private DateTime m_inceptDate;
        //private DateTime m_inceptTime;
        private DateTime m_inceptDateTime;
        //private DateTime m_checkDate;
        //private DateTime m_checkTime;
        private DateTime m_checkDateTime;
        private DateTime m_secondCheckDateTime;
        //private DateTime m_receiveDate;
        //private DateTime m_receiveTime;
        private DateTime m_receiveDateTime;
        //报告附注信息
        //private DateTime m_testDate;
        //private DateTime m_testTime;
        private DateTime m_testDateTime;
        private string m_parItemName;
        //备注、结论、解释等
        private string m_formMemo;
        private string m_formComment;
        private string m_formComment2;
        //检验者，审核者
        private string m_technician;
        private string m_checker;
        #endregion

        #region 构造函数
        public ReportExamElement()
            : base(m_defaultElementTag, m_defaultEaxmSQL)
        {
        }
        public ReportExamElement(ReportElementTag elementTag, string sql)
            : base(elementTag, sql)
        {
        }
        #endregion

        #region 公共属性
        [Export()]
        [TableColumn(true)]
        public string SerialNo
        {
            get { return m_serialNo; }
            set { m_serialNo = value; }
        }

        [Export()]
        [TableColumn(true)]
        public string SampleNo
        {
            get { return m_sampleNo; }
            set { m_sampleNo = value; }
        }

        [Export()]
        [TableColumn(true)]
        public string SampleTypeName
        {
            get { return m_sampleTypeName; }
            set { m_sampleTypeName = value; }
        }

        //[Convert2Xml()]
        //public string ReceiveDateTimeStr
        //{
        //    get { return SystemInfo.FormatDateTime(this.ReceiveDateTime); }
        //}
        [TableColumn(true)]
        public DateTime ReceiveDateTime
        {
            get { return m_receiveDateTime; }
            set { m_receiveDateTime = value; }
        }

        //[Convert2Xml()]
        //public string CollectDateTimeStr
        //{
        //    get { return SystemInfo.FormatDateTime(this.CollectDateTime); }
        //}
        [TableColumn(true)]
        public DateTime CollectDateTime
        {
            get { return m_collectDateTime; }
            set { m_collectDateTime = value; }
        }

        //[Convert2Xml()]
        //public string InceptDateTimeStr
        //{
        //    get { return SystemInfo.FormatDateTime(this.InceptDateTime); }
        //}
        [TableColumn(true)]
        public DateTime InceptDateTime
        {
            get { return m_inceptDateTime; }
            set { m_inceptDateTime = value; }
        }

        //[Convert2Xml()]
        //public string TestDateTimeStr
        //{
        //    get { return SystemInfo.FormatDateTime(this.TestDateTime); }
        //}
        [TableColumn(true)]
        public DateTime TestDateTime
        {
            get { return m_testDateTime; }
            set { m_testDateTime = value; }
        }

        //[Convert2Xml()]
        //public string CheckDateTimeStr
        //{
        //    get { return SystemInfo.FormatDateTime(this.CheckDateTime); }
        //}
        [TableColumn(true)]
        public DateTime CheckDateTime
        {
            get { return m_checkDateTime; }
            set { m_checkDateTime = value; }
        }

        //[Convert2Xml()]
        //public string SecondCheckDateTimeStr
        //{
        //    get { return SystemInfo.FormatDateTime(this.SecondeCheckDateTime); }
        //}
        [TableColumn(true)]
        public DateTime SecondeCheckDateTime
        {
            get { return m_secondCheckDateTime; }
            set { m_secondCheckDateTime = value; }
        }

        [Export()]
        [TableColumn(true)]
        public string ParItemName
        {
            get { return m_parItemName; }
            set { m_parItemName = value; }
        }

        [TableColumn(true)]
        public int SectionNo
        {
            get { return m_sectionNo; }
            set { m_sectionNo = value; }
        }
        [TableColumn(true)]
        public int SampleTypeNo
        {
            get { return m_sampleTypeNo; }
            set { m_sampleTypeNo = value; }
        }

        [Export()]
        [TableColumn(true)]
        public string FormMemo
        {
            get { return m_formMemo; }
            set { m_formMemo = value; }
        }
        [Export()]
        [TableColumn(true)]
        public string FormComment
        {
            get { return m_formComment; }
            set { m_formComment = value; }
        }
        [Export()]
        [TableColumn(true)]
        public string FormComment2
        {
            get { return m_formComment2; }
            set { m_formComment2 = value; }
        }

        [Export()]
        [TableColumn(true)]
        public string Technician
        {
            get { return m_technician; }
            set { m_technician = value; }
        }
        [Export()]
        [TableColumn(true)]
        public string Checker
        {
            get { return m_checker; }
            set { m_checker = value; }
        }
        #endregion

        #region 实现父类抽象方法
        protected override void Afterward()
        {
        }
        #endregion

        #region 受保护的虚方法
        //protected virtual void SetSignImage()
        //{
        //    //if (this.Checker != null && !this.Checker.Equals(""))
        //    //{
        //    //    this.CheckerImage = LisPUser.GetSignImage(this.Checker);
        //    //}
        //    //if (this.Technician != null && !this.Technician.Equals(""))
        //    //{
        //    //    this.TechnicianImage = LisPUser.GetSignImage(this.Technician);
        //    //}
        //}
        #endregion
    }
}