using XYS.Model;
namespace XYS.Report.Model
{
    public abstract class LisAbstractReportElement : IReportElement
    {
        #region 私有字段
        private string m_searchSQL;
        #endregion

        #region 受保护的构造函数
        protected LisAbstractReportElement()
            : this(null)
        {
        }
        protected LisAbstractReportElement(string sql)
        {
            this.m_searchSQL = sql;
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
        #endregion

        #region 方法
        #endregion
    }
}
