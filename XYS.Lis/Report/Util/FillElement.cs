using System;

using XYS.Util;
namespace XYS.Lis.Report.Util
{
    public class FillElement
    {
        #region 私有字段
        private string m_name;
        private readonly Type m_type;
        #endregion

        #region 构造方法
        public FillElement(Type type)
            : this(null, type)
        { }
        public FillElement(string typeName)
            : this(null, typeName)
        { }
        public FillElement(string name, Type type)
        {
            this.m_type = type;
            this.m_name = name;
        }
        public FillElement(string name, string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentNullException("typeName");
            }
            Type type = SystemInfo.GetTypeFromString(typeName, true, true);
            this.m_type = type;
            this.m_name = name;
        }
        #endregion

        #region 实例属性
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_name))
                {
                    return this.m_type.Name;
                }
                return this.m_name;
            }
            set { this.m_name = value; }
        }
        public Type EType
        {
            get { return this.m_type; }
        }
        #endregion
    }
}
