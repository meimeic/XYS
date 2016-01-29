using XYS.Common;
using XYS.Lis.Core;
namespace XYS.Lis.Model
{
    public class ReportGraphElement : AbstractReportElement
    {
        #region 私有静态字段
        private const ReportElementTag m_defaultElementTag = ReportElementTag.GraphElement;
        private static readonly string m_defaultGraphSQL = "select graphname,Graphjpg as graphimage from RFGraphData";
        #endregion

        #region 私有字段
        private string m_graphName;
        private byte[] m_graphImage;
        #endregion

        #region 公共构造函数
        public ReportGraphElement()
            : base(m_defaultElementTag, m_defaultGraphSQL)
        { }
        public ReportGraphElement(ReportElementTag elementTag, string sql)
            : base(elementTag, sql)
        {
        }
        #endregion

        #region 公共属性
        [Export()]
        [TableColumn(true)]
        public string GraphName
        {
            get { return this.m_graphName; }
            set { this.m_graphName = value; }
        }

        [Export()]
        [TableColumn(true)]
        public byte[] GraphImage
        {
            get { return this.m_graphImage; }
            set { this.m_graphImage = value; }
        }
        #endregion

        #region 实现父类抽象方法
        protected override void Afterward()
        {
        }
        #endregion
    }
}
