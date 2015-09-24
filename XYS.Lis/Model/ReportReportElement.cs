using System;
using XYS.Model;

namespace XYS.Lis.Model
{
    public class ReportReportElement:AbstractReportElement
    {
        #region 私有常量字段
        private const ReportElementType m_defaultElementType = ReportElementType.ReportElement;
        #endregion

        #region 公共构造函数
        public ReportReportElement()
            : base(m_defaultElementType)
        { }
        public ReportReportElement(ReportElementType elementType)
            : base(elementType)
        {
        }
        #endregion



    }
}
