using XYS.Lis.Core;
using XYS.Model;
using XYS.Common;
namespace XYS.Lis.Model
{
    public abstract class AbstractReportElement:ILisReportElement
    {
        #region 私有字段
        private readonly ReportElementTag m_elementTag;
        private readonly string m_searchSQL;
        //private ReportKey m_reportKey;
        #endregion

        #region 受保护的构造函数
        protected AbstractReportElement(ReportElementTag elementTag, string sql)
        {
            this.m_elementTag = elementTag;
            this.m_searchSQL = sql;
        }
        #endregion

        #region IReportElement实现
        public ReportElementTag ElementTag
        {
            get { return this.m_elementTag; }
        }
        //public ReportKey ReporterKey
        //{
        //    get { return this.m_reportKey; }
        //    set { this.m_reportKey = value; }
        //}
        #endregion

        #region ILisReportElement实现
        public string SearchSQL
        {
            get { return this.m_searchSQL; }
        }
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
