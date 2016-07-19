using XYS.Report;
using XYS.Common;

namespace XYS.Lis.Report.Model
{
    public class CustomElement : IFillElement
    {
        #region 私有字段
        private string m_reportID;
        private string m_c0;
        private string m_c1;
        private string m_c2;
        private string m_c3;
        private string m_c4;
        private string m_c5;
        private string m_c6;
        private string m_c7;
        private string m_c8;
        private string m_c9;
        #endregion

        #region 公共构造函数
        public CustomElement()
        {
        }
        #endregion

        #region 公共属性
        public string ReportID
        {
            get { return this.m_reportID; }
            set { this.m_reportID = value; }
        }
        public string C0
        {
            get { return this.m_c0; }
            set { this.m_c0 = value; }
        }

        public string C1
        {
            get { return this.m_c1; }
            set { this.m_c1 = value; }
        }

        public string C2
        {
            get { return this.m_c2; }
            set { this.m_c2 = value; }
        }

        public string C3
        {
            get { return this.m_c3; }
            set { this.m_c3 = value; }
        }

        public string C4
        {
            get { return this.m_c4; }
            set { this.m_c4 = value; }
        }

        public string C5
        {
            get { return this.m_c5; }
            set { this.m_c5 = value; }
        }

        public string C6
        {
            get { return this.m_c6; }
            set { this.m_c6 = value; }
        }

        public string C7
        {
            get { return this.m_c7; }
            set { this.m_c7 = value; }
        }

        public string C8
        {
            get { return this.m_c8; }
            set { this.m_c8 = value; }
        }

        public string C9
        {
            get { return this.m_c9; }
            set { this.m_c9 = value; }
        }
        #endregion
    }
}