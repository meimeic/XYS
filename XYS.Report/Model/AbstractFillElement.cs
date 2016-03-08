using XYS.Model;
namespace XYS.Report.Model
{
    public abstract class AbstractFillElement : IReportElement
    {
        #region 私有字段
        private string m_searchSQL;
        #endregion

        #region 受保护的构造函数
        protected AbstractFillElement()
            : this(null)
        {
        }
        protected AbstractFillElement(string sql)
        {
            this.m_searchSQL = sql;
        }
        #endregion

        #region 实例属性
        public string SearchSQL
        {
            get { return this.m_searchSQL; }
            set { this.m_searchSQL = value; }
        }
        #endregion

        #region IReportElement接口实现
        #endregion

        #region 内部方法
        #endregion
    }
}
