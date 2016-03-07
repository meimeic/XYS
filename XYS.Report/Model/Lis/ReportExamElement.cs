using System;
using XYS.Common;
namespace XYS.Report.Model.Lis
{
    [Export()]
    public class ReportExamElement : LisAbstractReportElement
    {
        #region 私有常量字段
        #endregion

        #region 私有字段
        private int m_sectionNo;
        private string m_serialNo;
        private string m_sampleNo;

        private int m_sampleTypeNo;
        private string m_sampleTypeName;
        //备注、结论、解释等
        private string m_formMemo;
        private string m_formComment;
        private string m_formComment2;
        //检验者，审核者

        #endregion

        #region 构造函数
        public ReportExamElement()
            : this(null)
        {
        }
        public ReportExamElement(string sql)
            : base(sql)
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

        [Export()]
        [Column(true)]
        public string SerialNo
        {
            get { return m_serialNo; }
            set { m_serialNo = value; }
        }

        [Export()]
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
        [Export()]
        [Column(true)]
        public string SampleTypeName
        {
            get { return m_sampleTypeName; }
            set { m_sampleTypeName = value; }
        }

        [Export()]
        [Column(true)]
        public string FormMemo
        {
            get { return m_formMemo; }
            set { m_formMemo = value; }
        }
        [Export()]
        [Column(true)]
        public string FormComment
        {
            get { return m_formComment; }
            set { m_formComment = value; }
        }
        [Export()]
        [Column(true)]
        public string FormComment2
        {
            get { return m_formComment2; }
            set { m_formComment2 = value; }
        }
        #endregion

        #region 方法
        #endregion

        #region
        #endregion
    }
}