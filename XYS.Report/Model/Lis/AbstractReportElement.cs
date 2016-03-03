using XYS.Report.Core;
using XYS.Util;
namespace XYS.Report.Model.Lis
{
    public abstract class AbstractReportElement : ILisReportElement
    {
        #region 私有字段
        private string m_searchSQL;
        private readonly ReportElementTag m_elementTag;
        #endregion

        #region 受保护的构造函数
        protected AbstractReportElement(ReportElementTag elementTag)
            : this(elementTag, null)
        {
        }
        protected AbstractReportElement(ReportElementTag elementTag, string sql)
        {
            this.m_searchSQL = sql;
            this.m_elementTag = elementTag;
        }
        #endregion

        #region 属性
        public string SearchSQL
        {
            get { return this.m_searchSQL; }
            set { this.m_searchSQL = value; }
        }
        #endregion

        #region IReportElement接口属性实现
        public ReportElementTag ElementTag
        {
            get { return this.m_elementTag; }
        }
        #endregion

        #region 实例虚方法
        public abstract void AfterFill();
        #endregion
    }
}
