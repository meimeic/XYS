using XYS.Model;
using XYS.Common;
namespace XYS.Lis.Model
{
    public class ReportGraphItemElement : AbstractReportElement
    {
        #region 私有静态字段
        private const ReportElementType m_defaultElementType = ReportElementType.GraphElement;
        private const string m_defaultGraphSQL = "select graphname,Graphjpg as graphimage from RFGraphData";
        #endregion

        #region 私有字段
        private string m_graphName;
        private byte[] m_graphImage;
        #endregion

        #region 公共构造函数
        public ReportGraphItemElement()
            : base(m_defaultElementType,m_defaultGraphSQL)
        { }
        public ReportGraphItemElement(ReportElementType elementType,string sql)
            : base(elementType,sql)
        {
        }
        #endregion

        #region 公共属性
        [TableColumn(true)]
        public string GraphName
        {
            get { return this.m_graphName; }
            set { this.m_graphName = value; }
        }
        [TableColumn(true)]
        public byte[] GraphImage
        {
            get { return this.m_graphImage; }
            set { this.m_graphImage = value; }
        }
        #endregion

    }
}
