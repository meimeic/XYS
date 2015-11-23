using XYS.Model;
using XYS.Common;
namespace XYS.Lis.Model
{
    /// <summary>
    /// 自定义报告元素类，不应用于数据库数据填充
    /// </summary>
    public class ReportCustomElement : AbstractReportElement
    {
        #region 私有常量字段
        private static readonly ReportElementTag m_defaultElementTag = ReportElementTag.CustomElement;
        public static readonly int COLUMN_COUNT = 20;
        #endregion

        #region
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
        private string m_column10;
        private string m_column11;
        private string m_column12;
        private string m_column13;
        private string m_column14;
        private string m_column15;
        private string m_column16;
        private string m_column17;
        private string m_column18;
        private string m_column19;
        #endregion

        #region 公共构造函数
        public ReportCustomElement()
            : this(m_defaultElementTag, "")
        {
        }
        public ReportCustomElement(ReportElementTag elementTag, string sql)
            : base(elementTag, sql)
        {
        }
        #endregion

        #region 公共属性
        [Export()]
        public string Column0
        {
            get { return this.m_column0; }
            set { this.m_column0 = value; }
        }
        [Export()]
        public string Column1
        {
            get { return this.m_column1; }
            set { this.m_column1 = value; }
        }
        [Export()]
        public string Column2
        {
            get { return this.m_column2; }
            set { this.m_column2 = value; }
        }
        [Export()]
        public string Column3
        {
            get { return this.m_column3; }
            set { this.m_column3 = value; }
        }
        [Export()]
        public string Column4
        {
            get { return this.m_column4; }
            set { this.m_column4 = value; }
        }
        [Export()]
        public string Column5
        {
            get { return this.m_column5; }
            set { this.m_column5 = value; }
        }
        [Export()]
        public string Column6
        {
            get { return this.m_column6; }
            set { this.m_column6 = value; }
        }
        [Export()]
        public string Column7
        {
            get { return this.m_column7; }
            set { this.m_column7 = value; }
        }
        [Export()]
        public string Column8
        {
            get { return this.m_column8; }
            set { this.m_column8 = value; }
        }
        [Export()]
        public string Column9
        {
            get { return this.m_column9; }
            set { this.m_column9 = value; }
        }
        [Export()]
        public string Column10
        {
            get { return this.m_column10; }
            set { this.m_column10 = value; }
        }
        [Export()]
        public string Column11
        {
            get { return this.m_column11; }
            set { this.m_column11 = value; }
        }
        [Export()]
        public string Column12
        {
            get { return this.m_column12; }
            set { this.m_column12 = value; }
        }
        [Export()]
        public string Column13
        {
            get { return this.m_column13; }
            set { this.m_column13 = value; }
        }
        [Export()]
        public string Column14
        {
            get { return this.m_column14; }
            set { this.m_column14 = value; }
        }
        [Export()]
        public string Column15
        {
            get { return this.m_column15; }
            set { this.m_column15 = value; }
        }
        [Export()]
        public string Column16
        {
            get { return this.m_column16; }
            set { this.m_column16 = value; }
        }
        [Export()]
        public string Column17
        {
            get { return this.m_column17; }
            set { this.m_column17 = value; }
        }
        [Export()]
        public string Column18
        {
            get { return this.m_column18; }
            set { this.m_column18 = value; }
        }
        [Export()]
        public string Column19
        {
            get { return this.m_column19; }
            set { this.m_column19 = value; }
        }
        #endregion

        #region 实现父类抽象方法
        protected override void Afterward()
        {
        }
        #endregion
    }
}
