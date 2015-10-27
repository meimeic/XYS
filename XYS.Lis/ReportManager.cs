using System;
using System.Reflection;

using XYS.Lis.Core;
namespace XYS.Lis
{
    public class ReportManager
    {

        private static readonly WrapperMap s_wrapperMap = new WrapperMap(new WrapperCreationHandler(WrapperCreationHandler));
        public static IReport Exists(ReporterKey key)
        {
            return Exists(Assembly.GetCallingAssembly(), key);
        }
        public static IReport Exists(Assembly repositoryAssembly, ReporterKey key)
        {
            return WrapReporter(ReporterManager.Exists(repositoryAssembly, key));
        }

        public static IReport GetReporter(ReporterKey key)
        {
            return GetReporter(Assembly.GetCallingAssembly(), key);
        }
        public static IReport GetReporter(Type type)
        {
            return GetReporter(Assembly.GetCallingAssembly(), type);
        }
        public static IReport GetReporter(Type type, string strategyName)
        {
            return GetReporter(Assembly.GetCallingAssembly(), type, strategyName);
        }
        
        public static IReport GetReporter(string repository, ReporterKey key)
        {
            return WrapReporter(ReporterManager.GetReporter(repository, key));
        }
        public static IReport GetReporter(string repository, Type type)
        {
            return WrapReporter(ReporterManager.GetReporter(repository, type));
        }
        public static IReport GetReporter(string repository, Type type, string strategyName)
        {
            return WrapReporter(ReporterManager.GetReporter(repository, type, strategyName));
        }
        
        public static IReport GetReporter(Assembly repositoryAssembly, ReporterKey key)
        {
            return WrapReporter(ReporterManager.GetReporter(repositoryAssembly, key));
        }
        public static IReport GetReporter(Assembly repositoryAssembly, Type type)
        {
            return WrapReporter(ReporterManager.GetReporter(repositoryAssembly, type));
        }
        public static IReport GetReporter(Assembly repositoryAssembly, Type type, string strategyName)
        {
            return WrapReporter(ReporterManager.GetReporter(repositoryAssembly, type,strategyName));
        }

        private static IReport WrapReporter(ILisReporter reporter)
        {
            return (IReport)s_wrapperMap.GetWrapper(reporter);
        }
        private static IReporterWrapper WrapperCreationHandler(ILisReporter reporter)
        {
            return new ReportImpl(reporter);
        }
    }
}
