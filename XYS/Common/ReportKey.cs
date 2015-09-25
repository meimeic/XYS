using System.Collections.Generic;
using XYS.Model;

namespace XYS.Common
{
    public abstract class ReportKey
    {
        #region 私有只读字段
        private readonly HashSet<KeyColumn> m_KeySet;
        #endregion

        #region 私有实例字段
        private IReportElement m_reportElement;
        private string m_tableName;
        #endregion

        #region 公共构造函数
        public ReportKey()
        {
            this.m_KeySet = new HashSet<KeyColumn>();
        }
        public ReportKey(HashSet<KeyColumn> ketSet)
        {
            this.m_KeySet = ketSet;
        }
        #endregion
        public ReportKey AddKey(KeyColumn key)
        {
            this.m_KeySet.Add(key);
            return this;
        }
        #region 实例属性
        public IReportElement ReportElement
        {
            get { return this.m_reportElement; }
            set { this.m_reportElement = value; }
        }
        public string TableName
        {
            get { return this.m_tableName; }
            set { this.m_tableName = value; }
        }
        #endregion
    }
}
