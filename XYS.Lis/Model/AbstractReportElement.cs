using XYS.Lis.Core;
using XYS.Common;
namespace XYS.Lis.Model
{
    public abstract class AbstractReportElement:IReportElement
    {
        #region 私有字段
        private readonly string m_searchSQL;
        private readonly ReportElementTag m_elementTag;
        #endregion

        #region 受保护的构造函数
        protected AbstractReportElement(string sql)
        {
            this.m_searchSQL = sql;
            this.m_elementTag = ReportElementTag.Filler;
        }
        #endregion

        #region 属性
        public string SearchSQL
        {
            get { return this.m_searchSQL; }
        }
        #endregion

        #region IReportElement接口属性实现
        public ReportElementTag ElementTag
        {
            get { return this.m_elementTag; }
        }
        #endregion

        #region 实例虚方法
        public void After()
        {
            //后续操作
            this.Afterward();
        }
        protected abstract void Afterward();
        #endregion
    }
}
