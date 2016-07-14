using System;

using XYS.Util;
namespace XYS.Lis.Report.Util
{
    public class FillElement
    {
        #region 私有字段
        private string m_name;
        private readonly Type m_type;
        private readonly Type m_handleType;
        #endregion

        #region 构造方法
        public FillElement(string name, Type type, Type handleType)
        {
            this.m_type = type;
            this.m_name = name;
            this.m_handleType = handleType;
        }
        public FillElement(string name, string typeName, string handleName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentNullException("typeName");
            }
            if (string.IsNullOrEmpty(handleName))
            {
                throw new ArgumentNullException("handleName");
            }
            Type type = SystemInfo.GetTypeFromString(typeName, true, true);
            Type handleType = SystemInfo.GetTypeFromString(handleName, true, true);

            this.m_type = type;
            this.m_name = name;
            this.m_handleType = handleType;
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
        public Type HandleType
        {
            get { return this.m_handleType; }
        }
        #endregion
    }
}
