using System;

using XYS.Util;
using XYS.Lis.Core;
using XYS.Lis.Util;
namespace XYS.Lis.Model
{
    public class ReportExamElement : AbstractReportElement
    {
        #region 私有常量字段
        //        private static readonly string m_defaultEaxmSQL = @"select serialno,sampleno,s.CName as sampletypename,CAST(CONVERT(varchar(10), collectdate, 121) + ' ' + CONVERT(varchar(8), collecttime, 114) AS datetime) as collectdatetime,CAST(CONVERT(varchar(10), inceptdate, 121) + ' ' + CONVERT(varchar(8), incepttime, 114) AS datetime) as inceptdatetime,
        //                                                                             CAST(CONVERT(varchar(10), testdate, 121) + ' ' + CONVERT(varchar(8), testtime, 114) AS datetime) as testdatetime,CAST(CONVERT(varchar(10), checkdate, 121) + ' ' + CONVERT(varchar(8), checktime, 114) AS datetime) as checkdatetime,
        //                                                                             CAST(CONVERT(varchar(10), receivedate, 121) + ' ' + CONVERT(varchar(8), receivetime, 114) AS datetime) as receivedatetime,sendertime2 as secondecheckdatetime,paritemname,sectionno,r.sampletypeno,formmemo,formcomment,formcomment2,technician,checker
        //                                                                             from ReportForm as r left outer join SampleType as s on r.SampleTypeNo=s.SampleTypeNo";

        private static readonly ReportElementTag m_defaultElementTag = ReportElementTag.Exam;
        #endregion

        #region 私有字段
        private int m_sectionNo;
        private string m_serialNo;
        private string m_sampleNo;
        //
        private int m_sampleTypeNo;
        private string m_sampleTypeName;
        //
        private DateTime m_collectDateTime;
        private DateTime m_inceptDateTime;
        private DateTime m_receiveDateTime;
        private DateTime m_checkDateTime;
        private DateTime m_secondCheckDateTime;
        //报告附注信息
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
            : base(m_defaultElementTag)
        {
        }
        public ReportExamElement(string sql)
            : base(m_defaultElementTag, sql)
        {
        }
        #endregion

        #region 公共属性
        [Column(true)]
        public int SectionNo
        {
            get { return m_sectionNo; }
            set { m_sectionNo = value; }
        }

        [FRConvert()]
        [Column(true)]
        public string SerialNo
        {
            get { return m_serialNo; }
            set { m_serialNo = value; }
        }

        [FRConvert()]
        [Column(true)]
        public string SampleNo
        {
            get { return m_sampleNo; }
            set { m_sampleNo = value; }
        }

        [Column(true)]
        public int SampleTypeNo
        {
            get { return m_sampleTypeNo; }
            set { m_sampleTypeNo = value; }
        }
        [FRConvert()]
        [Column(true)]
        public string SampleTypeName
        {
            get { return m_sampleTypeName; }
            set { m_sampleTypeName = value; }
        }

        [Column(true)]
        public DateTime CollectDateTime
        {
            get { return m_collectDateTime; }
            set { m_collectDateTime = value; }
        }
        [Column(true)]
        public DateTime InceptDateTime
        {
            get { return m_inceptDateTime; }
            set { m_inceptDateTime = value; }
        }
        [Column(true)]
        public DateTime ReceiveDateTime
        {
            get { return m_receiveDateTime; }
            set { m_receiveDateTime = value; }
        }
        [Column(true)]
        public DateTime TestDateTime
        {
            get { return m_testDateTime; }
            set { m_testDateTime = value; }
        }
        [Column(true)]
        public DateTime CheckDateTime
        {
            get { return m_checkDateTime; }
            set { m_checkDateTime = value; }
        }
        [Column(true)]
        public DateTime SecondeCheckDateTime
        {
            get { return m_secondCheckDateTime; }
            set { m_secondCheckDateTime = value; }
        }

        [FRConvert()]
        [Column(true)]
        public string ParItemName
        {
            get { return m_parItemName; }
            set { m_parItemName = value; }
        }

        [FRConvert()]
        [Column(true)]
        public string FormMemo
        {
            get { return m_formMemo; }
            set { m_formMemo = value; }
        }
        [FRConvert()]
        [Column(true)]
        public string FormComment
        {
            get { return m_formComment; }
            set { m_formComment = value; }
        }
        [FRConvert()]
        [Column(true)]
        public string FormComment2
        {
            get { return m_formComment2; }
            set { m_formComment2 = value; }
        }

        [FRConvert()]
        [Column(true)]
        public string Technician
        {
            get { return m_technician; }
            set { m_technician = value; }
        }
        [FRConvert()]
        [Column(true)]
        public string Checker
        {
            get { return m_checker; }
            set { m_checker = value; }
        }
        #endregion

        #region 实现父类抽象方法
        public override void AfterFill()
        {
        }
        #endregion

        #region
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