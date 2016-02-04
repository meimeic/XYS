using System;
namespace XYS.Common
{
    public class KeyColumn
    {
        #region 私有字段
        private string m_name;
        private object m_value;
        #endregion

        #region 公共构造方法
        public KeyColumn(string name, object value)
        {
            this.m_name = name;
            this.m_value = value;
        }
        #endregion

        #region 公共属性
        public object Value
        {
            get { return this.m_value; }
            set { this.m_value = value; }
        }
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }
        #endregion
    }
}
