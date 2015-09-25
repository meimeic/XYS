using System;
using XYS.Model;
using XYS.Lis.Util;
namespace XYS.Lis.Model
{
    public class ReportExamElement:AbstractReportElement
    {
        #region 私有常量字段
        private const ReportElementType m_defaultElementType = ReportElementType.ExamElement;
        #endregion

        #region 私有字段
        private string m_serialNo;
        private string m_sampleNo;
        private int m_sectionNo;
        private int m_sampleTypeNo;
        private string m_sampleTypeName;
        //采集、签收、审核时,二次审核,核收时间
        private DateTime m_collectDateTime;
        private DateTime m_inceptDateTime;
        private DateTime m_checkDateTime;
        private DateTime m_secondCheckDateTime;
        private DateTime m_receiveDateTime;
        //报告附注信息
        private DateTime m_testDateTime;
        private string m_parItemName;
        //备注、结论、解释等
        private string m_remark;
        private string m_formMemo;
        private string m_formComment;
        private string m_formComment2;
        //检验者，审核者
        private string m_technician;
        private string m_checker;
        private byte[] m_technicianImage;
        private byte[] m_checkerImage;
        #endregion

        #region 构造函数
        internal ReportExamElement()
            : base(m_defaultElementType)
        {
        }
        internal ReportExamElement(ReportElementType elementType)
            : base(elementType)
        {
        }
        #endregion

        #region 公共属性
         public string SerialNo
        {
            get { return m_serialNo; }
            set { m_serialNo = value; }
        }
        public string SampleNo
        {
            get { return m_sampleNo; }
            set { m_sampleNo = value; }
        } 

        public string SampleTypeName
        {
            get { return m_sampleTypeName; }
            set { m_sampleTypeName = value; }
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
        public DateTime SecondCheckDateTime
        {
            get { return m_secondCheckDateTime; }
            set { m_secondCheckDateTime = value; }
        }
        public string ParItemName
        {
            get { return m_parItemName; }
            set { m_parItemName = value; }
        }

        public int SectionNo
        {
            get { return m_sectionNo; }
            set { m_sectionNo = value; }
        }
        public int SampleTypeNo
        {
            get { return m_sampleTypeNo; }
            set { m_sampleTypeNo = value; }
        }
        public string Remark
        {
            get { return m_remark; }
            set { m_remark = value; }
        }
        public string FormMemo
        {
            get { return m_formMemo; }
            set { m_formMemo = value; }
        }

        public string FormComment
        {
            get { return m_formComment; }
            set { m_formComment = value; }
        }

        public string FormComment2
        {
            get { return m_formComment2; }
            set { m_formComment2 = value; }
        }
        public string Technician
        {
            get { return m_technician; }
            set { m_technician = value; }
        }

        public string Checker
        {
            get { return m_checker; }
            set { m_checker = value; }
        }
        public DateTime ReceiveDateTime
        {
            get { return m_receiveDateTime; }
            set { m_receiveDateTime = value; }
        }
        public byte[] TechnicianImage
        {
            get { return this.m_technicianImage; }
        }
        public byte[] CheckerImage
        {
            get { return this.m_checkerImage; }
        }
        #endregion

        #region 父类重写虚方法
        protected override void Afterward()
        {
            this.SetSignImage();
        }
        #endregion
        #region 受保护的虚方法
        protected virtual void SetSignImage()
        {
            if (this.m_checker != null && !this.m_checker.Equals(""))
            {
                this.m_checkerImage = LisTechnician.GetSignImage(this.m_checker);
            }
            if (this.m_technician != null && !this.m_technician.Equals(""))
            {
                this.m_technicianImage = LisTechnician.GetSignImage(this.m_technician);
            }
        }
        #endregion
    }
}