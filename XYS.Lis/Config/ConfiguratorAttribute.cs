using System;
using System.Reflection;

using XYS.Lis.Repository;
namespace XYS.Lis.Config
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public abstract class ConfiguratorAttribute : Attribute, IComparable
    {
        private int m_priority = 0;
        protected ConfiguratorAttribute(int priority)
        {
            m_priority = priority;
        }
        public abstract void Configure(Assembly sourceAssembly, IReporterRepository targetRepository);
        public int CompareTo(object obj)
        {
            // Reference equals
            if ((object)this == obj)
            {
                return 0;
            }

            int result = -1;

            ConfiguratorAttribute target = obj as ConfiguratorAttribute;
            if (target != null)
            {
                // Compare the priorities
                result = target.m_priority.CompareTo(m_priority);
                if (result == 0)
                {
                    // Same priority, so have to provide some ordering
                    result = -1;
                }
            }
            return result;
        }
    }
}
