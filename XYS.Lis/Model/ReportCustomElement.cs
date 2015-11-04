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
            : this(m_defaultElementTag, "")
        {
        }
        public ReportCustomElement(ReportElementTag elementTag,string sql)
            : base(elementTag, sql)
        {
        }
        #endregion

    }
}
