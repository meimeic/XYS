using System;
using System.Reflection;

using XYS.Lis.Core;
namespace XYS.Lis
{
    public class ReportManager
    {
        public static IReport GetLogger(Type type)
        {
            return GetLogger(Assembly.GetCallingAssembly(), type.FullName);
        }
        public static IReport GetLogger(Assembly repositoryAssembly, string name)
        {
            return WrapReporter(LoggerManager.GetLogger(repositoryAssembly, name));
        }
        public static IReport GetReporter(Assembly repositoryAssembly, string name)
        {
            if (repositoryAssembly == null)
            {
                throw new ArgumentNullException("repositoryAssembly");
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            return RepositorySelector.GetRepository(repositoryAssembly).GetReporter(name);
        }
        public static IRepositorySelector RepositorySelector
        {
            get { return s_repositorySelector; }
            set { s_repositorySelector = value; }
        }
    }
}
