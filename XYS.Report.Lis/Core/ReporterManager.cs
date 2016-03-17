using System;
using System.Reflection;

using XYS.Util;

using XYS.Report.Lis.Repository;
namespace XYS.Report.Lis.Core
{
    //不可实例化
    public class ReporterManager
    {
        #region
        private static RepositorySelector s_repositorySelector;
        private static readonly Type declaringType = typeof(ReporterManager);
        #endregion

        #region 构造函数
        private ReporterManager()
        {
        }
        static ReporterManager()
        {
            //使用默认配置
            if (s_repositorySelector == null)
            {
                s_repositorySelector = new RepositorySelector();
            }
        }
        #endregion

        #region 静态属性
        public static RepositorySelector RepositorySelector
        {
            get { return s_repositorySelector; }
            set { s_repositorySelector = value; }
        }
        #endregion

        #region 通过库名获取库
        public static IReporterRepository GetRepository(string repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            return RepositorySelector.GetRepository(repository);
        }
        public static IReporterRepository GetRepository(Assembly repositoryAssembly)
        {
            if (repositoryAssembly == null)
            {
                throw new ArgumentNullException("repositoryAssembly");
            }
            return RepositorySelector.GetRepository(repositoryAssembly);
        }
        #endregion

        #region 查看reporter 是否存在
        public static ILisReporter Exists(string repository, ReporterKey key)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (key == null)
            {
                throw new ArgumentNullException("name");
            }
            return RepositorySelector.GetRepository(repository).Exists(key);
        }
        public static ILisReporter Exists(Assembly repositoryAssembly, ReporterKey key)
        {
            if (repositoryAssembly == null)
            {
                throw new ArgumentNullException("repositoryAssembly");
            }
            if (key == null)
            {
                throw new ArgumentNullException("ReporterKey");
            }
            return RepositorySelector.GetRepository(repositoryAssembly).Exists(key);
        }
        #endregion

        #region 获取reporter 若不存在，则创建
        public static ILisReporter GetReporter(string repository, ReporterKey key)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (key == null)
            {
                throw new ArgumentNullException("ReporterKey");
            }
            return RepositorySelector.GetRepository(repository).GetReporter(key);
        }
        public static ILisReporter GetReporter(string repository, Type type)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            ReporterKey key = new ReporterKey(type.FullName);
            return RepositorySelector.GetRepository(repository).GetReporter(key);
        }
        public static ILisReporter GetReporter(string repository, Type type, string strategyName)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            ReporterKey key = new ReporterKey(type.FullName, strategyName);
            return RepositorySelector.GetRepository(repository).GetReporter(key);
        }
        public static ILisReporter GetReporter(Assembly repositoryAssembly, ReporterKey key)
        {
            if (repositoryAssembly == null)
            {
                throw new ArgumentNullException("repositoryAssembly");
            }
            if (key == null)
            {
                throw new ArgumentNullException("ReporterKey");
            }
            return RepositorySelector.GetRepository(repositoryAssembly).GetReporter(key);
        }
        public static ILisReporter GetReporter(Assembly repositoryAssembly, Type type)
        {
            if (repositoryAssembly == null)
            {
                throw new ArgumentNullException("repositoryAssembly");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            ReporterKey key = new ReporterKey(type.FullName);
            return RepositorySelector.GetRepository(repositoryAssembly).GetReporter(key);
        }
        public static ILisReporter GetReporter(Assembly repositoryAssembly, Type type, string strategyName)
        {
            if (repositoryAssembly == null)
            {
                throw new ArgumentNullException("repositoryAssembly");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            ReporterKey key = new ReporterKey(type.FullName, strategyName);
            return RepositorySelector.GetRepository(repositoryAssembly).GetReporter(key);
        }
        #endregion

        #region 创建Repository
        public static IReporterRepository CreateRepository(string repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            return RepositorySelector.CreateRepository(repository, null);
        }
        public static IReporterRepository CreateRepository(string repository, Type repositoryType)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (repositoryType == null)
            {
                throw new ArgumentNullException("repositoryType");
            }
            return RepositorySelector.CreateRepository(repository, repositoryType);
        }
        public static IReporterRepository CreateRepository(Assembly repositoryAssembly, Type repositoryType)
        {
            if (repositoryAssembly == null)
            {
                throw new ArgumentNullException("repositoryAssembly");
            }
            if (repositoryType == null)
            {
                throw new ArgumentNullException("repositoryType");
            }
            return RepositorySelector.CreateRepository(repositoryAssembly, repositoryType);
        }
        #endregion
    }
}
