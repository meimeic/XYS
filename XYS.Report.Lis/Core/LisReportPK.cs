using System;

namespace XYS.Report.Lis.Core
{
    public class LisReportPK
    {
        private int m_sectionNo;
        private int m_testTypeNo;
        private string m_sampleNo;
        private DateTime m_receiveDate;

        public LisReportPK()
        {
        }

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

        public string GUID
        {
            get { return this.m_receiveDate.ToString("yyyyMMdd") + "-" + this.m_sectionNo + "-" + this.m_testTypeNo + "-" + this.m_sampleNo; }
        }
    }
}
