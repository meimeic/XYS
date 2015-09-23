using System;
namespace XYS.Common
{
    public class CommonPKElement : IPKElement
    {
        #region 私有静态字段
        private static readonly string ELEMENTNAME = "COMMON_PK_ELEMENT";
        private static readonly long ELEMENTVALUE = 100000L;
        #endregion

        #region 私有字段
        private object m_pk;
        private Type m_declareType;
        #endregion

        #region 实现IPKElement接口
        public string PKElementName
        {
            get { return ELEMENTNAME; }
        }
        public long PKElementValue
        {
            get { return ELEMENTVALUE; }
        }
        public Type PKType
        {
            get { return this.m_pk.GetType(); }
        }
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
        #endregion
    }
}
