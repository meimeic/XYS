using XYS.Common;
using XYS.Lis.Core;
namespace XYS.Lis.Model
{
    [Export()]
    public class ReportGraphElement : AbstractReportElement
    {
        #region 私有静态字段
        private static readonly string m_defaultGraphSQL = "select graphname,Graphjpg as graphimage from RFGraphData";
        private static readonly ReportElementTag m_defaultElementTag = ReportElementTag.Graph;
        #endregion

        #region 私有字段
        private string m_graphName;
        private byte[] m_graphImage;
        #endregion

        #region 公共构造函数
        public ReportGraphElement()
            : base(m_defaultElementTag)
        { }
        public ReportGraphElement(string sql)
            : base(m_defaultElementTag, sql)
        { }
        #endregion

        #region 公共属性
        [Export()]
        [Column(true)]
        public string GraphName
        {
            get { return this.m_graphName; }
            set { this.m_graphName = value; }
        }

        [Export()]
        [Column(true)]
        public byte[] GraphImage
        {
            get { return this.m_graphImage; }
            set { this.m_graphImage = value; }
        }
        #endregion

        #region 实现父类抽象方法
        public override void AfterFill()
        {
        }
        #endregion
    }
}
