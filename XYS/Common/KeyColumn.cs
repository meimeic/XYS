using System;
namespace XYS.Common
{
    public class KeyColumn
    {
        #region 私有字段
        private object m_pk;
        private Type m_declareType;
        #endregion

        #region 公共构造方法

        #endregion

        #region 公共属性
        public Type DeclaredType
        {
            get { return this.m_declareType; }
            set { this.m_declareType = value; }
        }
        public object PK
        {
            get { return this.m_pk; }
            set { this.m_pk = value; }
        }
        public Type PKType
        {
            get
            {
                return this.m_pk.GetType();
            }
        }
        #endregion
    }
}
