using XYS.Model;

namespace XYS.Lis.Model
{
    public  class ReportCustomItemElement:AbstractReportElement
    {
        #region 私有常量字段
        private const ReportElementType m_elementType = ReportElementType.CustomElement;
        #endregion

        #region 公共构造函数
        public ReportCustomItemElement()
            : base(m_elementType)
        {
        }
        public ReportCustomItemElement(ReportElementType elementType)
            : base(elementType)
        {
        }
        #endregion

    }
}
