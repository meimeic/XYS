using System.Collections.Generic;
using XYS.Model;

namespace XYS.Common
{
    public abstract class ReportPK
    {
        #region 私有只读字段
        private readonly HashSet<PKColumn> m_KeySet;
        #endregion

        #region 私有实例字段
        private IReportElement m_reportElement;
        #endregion

        #region 公共构造函数
        public ReportPK()
        {
            this.m_KeySet = new HashSet<PKColumn>();
        }
        public ReportPK(HashSet<PKColumn> ketSet)
        {
            this.m_KeySet = ketSet;
        }
        #endregion
        public ReportPK AddKey(PKColumn key)
        {
            this.m_KeySet.Add(key);
            return this;
        }
        public IReportElement ReportElement
        {
            get { return this.m_reportElement; }
            set { this.m_reportElement = value; }
        }
    }
}
