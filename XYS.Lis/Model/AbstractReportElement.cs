using XYS.Lis.Core;
using XYS.Model;
namespace XYS.Lis.Model
{
    public abstract class AbstractReportElement:ILisReportElement
    {
        #region 私有字段
        private readonly ReportElementType m_elementType;
        #endregion

        #region 受保护的构造函数
        protected AbstractReportElement(ReportElementType elementType)
        {
            this.m_elementType = elementType;
        }
        #endregion
        
        #region IReportElement实现
        public ReportElementType ElementType
        {
            get { return this.m_elementType; }
        }
        #endregion
    }
}
