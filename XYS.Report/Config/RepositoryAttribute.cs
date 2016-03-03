using System;

namespace XYS.Report.Config
{
    [AttributeUsage(AttributeTargets.Assembly,AllowMultiple=false)]
    [Serializable]
    public class RepositoryAttribute:Attribute
    {
        #region  构造函数
        public RepositoryAttribute()
        {
        }
        public RepositoryAttribute(string name)
        {
            m_name = name;
        }

        #endregion Public Instance Constructors

        #region 公共实例属性
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        public Type RepositoryType
        {
            get { return m_repositoryType; }
            set { m_repositoryType = value; }
        }

        #endregion

        #region 私有实例字段

        private string m_name = null;
        private Type m_repositoryType = null;

        #endregion
    }
}
