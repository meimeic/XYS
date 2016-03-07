using System;
using System.Reflection;

using XYS.Report.Repository;
namespace XYS.Report.Config
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public abstract class ConfiguratorAttribute : Attribute, IComparable
    {
        //权值
        private int m_priority = 0;

        #region 构造函数
        protected ConfiguratorAttribute(int priority)
        {
            m_priority = priority;
        }
        #endregion

        public abstract void Configure(Assembly sourceAssembly, IReporterRepository targetRepository);

        #region 实现IComparable方法
        public int CompareTo(object obj)
        {
            if ((object)this == obj)
            {
                return 0;
            }
            int result = -1;
            ConfiguratorAttribute target = obj as ConfiguratorAttribute;
            if (target != null)
            {
                result = target.m_priority.CompareTo(this.m_priority);
                if (result == 0)
                {
                    result = -1;
                }
            }
            return result;
        }
        #endregion
    }
}
