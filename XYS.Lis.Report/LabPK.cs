using System;
using System.Text;

using XYS.Report;
namespace XYS.Lis.Report
{
    public class LabPK : IReportKey
    {
        #region 私有字段
        private bool m_config;
        private int m_sectionNo;
        private int m_testTypeNo;
        private string m_sampleNo;
        private DateTime m_receiveDate;
        #endregion

        #region 构造函数
        public LabPK()
        {
            this.m_config = false;
        }
        #endregion

        #region 实例属性
        public int SectionNo
        {
            get { return this.m_sectionNo; }
            set { this.m_sectionNo = value; }
        }
        public int TestTypeNo
        {
            get { return this.m_testTypeNo; }
            set { this.m_testTypeNo = value; }
        }
        public string SampleNo
        {
            get { return this.m_sampleNo; }
            set { this.m_sampleNo = value; }
        }
        public DateTime ReceiveDate
        {
            get { return this.m_receiveDate; }
            set { this.m_receiveDate = value; }
        }
        #endregion

        #region 实现IReportKey接口
        public bool Configured
        {
            get { return this.m_config; }
            set { this.m_config = value; }
        }
        public string ID
        {
            get { return this.m_receiveDate.ToString("yyyyMMdd") + "-" + this.m_sectionNo + "-" + this.m_testTypeNo + "-" + this.m_sampleNo; }
        }
        public string KeyWhere()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" where ");
            sb.Append("receivedate='");
            sb.Append(this.m_receiveDate.ToString("yyyy-MM-dd"));
            sb.Append("' and sectionno=");
            sb.Append(this.m_sectionNo);
            sb.Append(" and testtypeno=");
            sb.Append(this.m_testTypeNo);
            sb.Append(" and sampleno='");
            sb.Append(this.m_sampleNo);
            sb.Append("'");
            return sb.ToString();
        }
        #endregion
    }
}