using XYS.Model;
namespace XYS.Lis.Model
{
    public class ReportGraphItemElement:AbstractReportElement
    {
        #region 私有静态字段
        private const ReportElementType m_defaultElementType = ReportElementType.GraphElement;
        #endregion

        #region 私有字段
        private string m_itemName;
        private byte[] m_itemImage;
        #endregion
      
        #region 公共构造函数
        public ReportGraphItemElement()
            : base(m_defaultElementType)
        { }
        public ReportGraphItemElement(ReportElementType elementType)
            : base(elementType)
        { }
        #endregion

        #region 公共属性
        public string ItemName
        {
            get { return this.m_itemName; }
            set { this.m_itemName = value; }
        }
        public byte[] ItemImage
        {
            get { return this.m_itemImage; }
            set { this.m_itemImage = value; }
        }
        #endregion

        #region
        #endregion

    }
}
