using System;

namespace XYS.FR.Lis.Util
{
    public class LisGroup
    {
        #region 字段
        private readonly int m_sectionNo;
        private string m_sectionName;
        private int m_printModelNo;
        private int m_orderNo;
        #endregion

        #region 构造函数
        public LisGroup(int sectionNo)
            : this(sectionNo, null)
        {
        }
        public LisGroup(int sectionNo, string sectionName)
        {
            this.m_sectionNo = sectionNo;
            this.m_sectionName = sectionName;
        }
        #endregion

        #region 属性
        public int SectionNo
        {
            get { return this.m_sectionNo; }
        }
        public string SectionName
        {
            get { return this.m_sectionName; }
            set { this.m_sectionName = value; }
        }
        public int PrintModelNo
        {
            get { return this.m_printModelNo; }
            set { this.m_printModelNo = value; }
        }
        public int OrderNo
        {
            get { return this.m_orderNo; }
            set { this.m_orderNo = value; }
        }
        #endregion
    }
}
