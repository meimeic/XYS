using XYS.Model;
namespace XYS.Lis.Model
{
    public class ReportCommonItemElement:AbstractReportElement
    {
        #region 私有常量字段
        private const ReportElementType m_defaultElementType = ReportElementType.ItemElement;
        #endregion

        #region 私有字段
        private int m_itemNo;
        private string m_itemCName;
        private string m_itemEName;
        private string m_itemResult;
        private string m_itemStandard;
        private string m_resultStatus;
        private string m_itemUnit;
        private string m_refRange;
        private int m_dispOrder;
        #endregion

        #region 公共构造方法
        public ReportCommonItemElement()
            : base(m_defaultElementType)
        { }
        public ReportCommonItemElement(ReportElementType elementType)
            : base(elementType)
        { }
        #endregion

        #region 公共属性
        public int ItemNo
        {
            get { return this.m_itemNo; }
            set { this.m_itemNo = value; }
        }
        public string ItemCName
        {
            get { return this.m_itemCName; }
            set { this.m_itemCName = value; }
        }
        public string ItemEName
        {
            get { return this.m_itemEName; }
            set { this.m_itemEName = value; }
        }
        public string ItemResult
        {
            get { return this.m_itemResult; }
            set { this.m_itemResult = value; }
        }
        public string ItemStandard
        {
            get { return this.m_itemStandard; }
            set { this.m_itemStandard = value; }
        }
        public string ResultStatus
        {
            get { return this.m_resultStatus; }
            set { this.m_resultStatus = value; }
        }
        public string ItemUnit
        {
            get { return this.m_itemUnit; }
            set { this.m_itemUnit = value; }
        }
        public string RefRange
        {
            get { return this.m_refRange; }
            set { this.m_refRange = value; }
        }
        public int DispOrder
        {
            get { return this.m_dispOrder; }
            set { this.m_dispOrder = value; }
        }
        #endregion
    }
}
