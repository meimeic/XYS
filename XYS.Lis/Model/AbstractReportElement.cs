using XYS.Lis.Core;
using XYS.Model;
using XYS.Common;
namespace XYS.Lis.Model
{
    public abstract class AbstractReportElement:ILisReportElement
    {
        #region 私有字段
        private readonly ReportElementType m_elementType;
        //private ReportKey m_reportKey;
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
        //public ReportKey ReporterKey
        //{
        //    get { return this.m_reportKey; }
        //    set { this.m_reportKey = value; }
        //}
        #endregion

        #region ILisReportElement实现
        public void AfterFill()
        {
            //后续操作
            this.Afterward();
        }
        #endregion

        #region 实例虚方法
        protected virtual void Afterward()
        {
        }
        #endregion

    }
}
