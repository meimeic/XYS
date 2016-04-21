using System;
using System.Text;

using XYS.Report;
using XYS.Common;
namespace XYS.Report.Lis
{
    public class LisReportPK : IReportKey
    {
        #region 字段
        private bool m_config;
        private int m_sectionNo;
        private int m_testTypeNo;
        private string m_sampleNo;
        private DateTime m_receiveDate;
        private readonly KeyColumn[] m_KeySet;
        private static readonly int Capacity = 4;
        #endregion

        #region 构造函数
        public LisReportPK()
        {
            this.m_config = false;
            this.m_KeySet = new KeyColumn[Capacity];
        }
        #endregion

        #region 属性
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
        public KeyColumn[] KeySet
        {
            get { return this.m_KeySet; }
        }
        #endregion

        #region 属性
        public string ID
        {
            get { return this.m_receiveDate.ToString("yyyyMMdd") + "-" + this.m_sectionNo + "-" + this.m_testTypeNo + "-" + this.m_sampleNo; }
        }
        public bool Configured
        {
            get { return this.m_config; }
            set { this.m_config = value; }
        }
        #endregion

        #region
        public string GetID()
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyColumn key in m_KeySet)
            {
                if (key.Value.GetType().Equals(typeof(DateTime)))
                {
                    DateTime dt = (DateTime)key.Value;
                    sb.Append(dt.ToString("yyyyMMdd"));
                }
                else
                {
                    sb.Append(key.Value.ToString());
                }
                sb.Append('-');
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
        #endregion
    }
}
