using XYS.Common;
using XYS.Report.Lis.Core;

namespace XYS.Report.Lis.Model
{
    public class ReportCustomElement : ILisReportElement
    {
        #region 私有字段
        private string m_name;
        private string m_column0;
        private string m_column1;
        private string m_column2;
        private string m_column3;
        private string m_column4;
        private string m_column5;
        private string m_column6;
        private string m_column7;
        private string m_column8;
        private string m_column9;
        #endregion

        #region 公共构造函数
        public ReportCustomElement()
        {
        }
        #endregion

        #region 公共属性
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }
        public string Column0
        {
            get { return this.m_column0; }
            set { this.m_column0 = value; }
        }
        public string Column1
        {
            get { return this.m_column1; }
            set { this.m_column1 = value; }
        }
        public string Column2
        {
            get { return this.m_column2; }
            set { this.m_column2 = value; }
        }
        public string Column3
        {
            get { return this.m_column3; }
            set { this.m_column3 = value; }
        }
        public string Column4
        {
            get { return this.m_column4; }
            set { this.m_column4 = value; }
        }
        public string Column5
        {
            get { return this.m_column5; }
            set { this.m_column5 = value; }
        }
        public string Column6
        {
            get { return this.m_column6; }
            set { this.m_column6 = value; }
        }
        public string Column7
        {
            get { return this.m_column7; }
            set { this.m_column7 = value; }
        }
        public string Column8
        {
            get { return this.m_column8; }
            set { this.m_column8 = value; }
        }
        public string Column9
        {
            get { return this.m_column9; }
            set { this.m_column9 = value; }
        }
        #endregion
    }
}