using System;
using System.Reflection;
using System.Collections;

using XYS.Report.Lis.Core;
using XYS.Report.Lis.Repository;
namespace XYS.Report.Lis
{
    public class ReportManager
    {

        private static readonly Hashtable RepositoryMap = new Hashtable(2);

        public static IReport Exists(ReporterKey key)
        {
            return Exists(Assembly.GetCallingAssembly(), key);
        }
        public static IReport Exists(Assembly repositoryAssembly, ReporterKey key)
        {
            return WrapReporter(ReporterManager.Exists(repositoryAssembly, key));
        }

        #region
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
            return WrapReporter(ReporterManager.GetReporter(repositoryAssembly, type, strategyName));
        }
        #endregion

        #region 获取Repository
        public static IReporterRepository GetRepository()
        {
            return GetRepository(Assembly.GetCallingAssembly());
        }
        public static IReporterRepository GetRepository(string repository)
        {
            return ReporterManager.GetRepository(repository);
        }
        public static IReporterRepository GetRepository(Assembly repositoryAssembly)
        {
            return ReporterManager.GetRepository(repositoryAssembly);
        }
        public static IReporterRepository CreateRepository(Type repositoryType)
        {
            return CreateRepository(Assembly.GetCallingAssembly(), repositoryType);
        }
        public static IReporterRepository CreateRepository(string repository)
        {
            return ReporterManager.CreateRepository(repository);
        }
        public static IReporterRepository CreateRepository(string repository, Type repositoryType)
        {
            return ReporterManager.CreateRepository(repository, repositoryType);
        }
        public static IReporterRepository CreateRepository(Assembly repositoryAssembly, Type repositoryType)
        {
            return ReporterManager.CreateRepository(repositoryAssembly, repositoryType);
        }
        #endregion

        private static IReport WrapReporter(ILisReporter reporter)
        {
            Hashtable ReportMap = RepositoryMap[reporter.Repository] as Hashtable;
            if (ReportMap == null)
            {
                ReportMap = new Hashtable(10);
                RepositoryMap[reporter.Repository] = ReportMap;
            }
            IReport reportImpl = ReportMap[reporter] as IReport;
            if (reportImpl == null)
            {
                reportImpl = new ReportImpl(reporter);
                ReportMap[reporter] = reportImpl;
            }
            return reportImpl;
        }
    }
}
