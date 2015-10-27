using XYS.Model;

namespace XYS.Lis.Model
{
    public class ReportCustomElement:AbstractReportElement
    {
        #region 私有常量字段
        private const ReportElementTag m_defaultElementTag = ReportElementTag.CustomElement;
        #endregion

        #region 公共构造函数
        public ReportCustomElement()
            : base(m_defaultElementTag,"")
        {
        }
        public ReportCustomElement(ReportElementTag elementTag)
            : base(elementTag, "")
        {
        }
        #endregion

    }
}
