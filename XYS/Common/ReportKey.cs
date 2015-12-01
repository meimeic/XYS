using System.Collections.Generic;
using XYS.Model;

namespace XYS.Common
{
    public abstract class ReportKey
    {
        #region 私有只读字段
        private readonly HashSet<KeyColumn> m_KeySet;
        #endregion

        //#region 私有实例字段
        //private IReportElement m_reportElement;
        //private string m_tableName;
        //#endregion

        #region 公共构造函数
        protected ReportKey()
        {
            this.m_KeySet = new HashSet<KeyColumn>();
        }
        protected ReportKey(HashSet<KeyColumn> ketSet)
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
        public int Count
        {
            get { return this.m_KeySet.Count; }
        }
        public HashSet<KeyColumn> KeySet
        {
            get { return this.m_KeySet; }
        }
        #endregion
    }
}
