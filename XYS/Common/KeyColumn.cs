using System;
namespace XYS.Common
{
    public class KeyColumn
    {
        #region 私有字段
        private string m_name;
        private object m_pk;
        #endregion

        #region 公共构造方法
        public KeyColumn()
        { }
        public KeyColumn(string name, object pk)
        {
            this.m_name = name;
            this.m_pk = pk;
        }
        #endregion

        #region 公共属性
        public object PK
        {
            get { return this.m_pk; }
            set { this.m_pk = value; }
        }
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }
        public Type PKType
        {
            get
            {
                if (this.m_pk != null)
                {
                    return this.m_pk.GetType();
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
    }
}
