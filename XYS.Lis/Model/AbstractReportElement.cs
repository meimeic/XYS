using XYS.Lis.Core;
using XYS.Model;
using XYS.Common;
namespace XYS.Lis.Model
{
    public abstract class AbstractReportElement:ILisReportElement
    {
        #region 私有字段
        private readonly ReportElementType m_elementType;
        private ReportPK m_reportPK;
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
        public ReportPK ReporterPK
        {
            get { return this.m_reportPK; }
            set { this.m_reportPK = value; }
        }
        #endregion

    }
}
