using XYS.Common;
namespace XYS.Report.Model.Lis
{
    public class ReportGraphElement : LisAbstractReportElement
    {
        #region 私有静态字段
        #endregion

        #region 私有字段
        private string m_graphName;
        private byte[] m_graphImage;
        #endregion

        #region 公共构造函数
        public ReportGraphElement()
            : this(null)
        { }
        public ReportGraphElement(string sql)
            : base(sql)
        { }
        #endregion

        #region 公共属性

        [Column(true)]
        public string GraphName
        {
            get { return this.m_graphName; }
            set { this.m_graphName = value; }
        }

        [Column(true)]
        public byte[] GraphImage
        {
            get { return this.m_graphImage; }
            set { this.m_graphImage = value; }
        }
        #endregion

        #region 方法
        #endregion
    }
}
