using System;
using System.Reflection;
using System.Collections.Generic;

using XYS.Lis.Repository;
namespace XYS.Lis.Core
{
    public delegate void ReporterRepositoryCreationEventHandler(object sender,ReporterRepositoryCreationEventArgs e);
    public class ReporterRepositoryCreationEventArgs : EventArgs
    {
        private IReporterRepository m_repository;

        public ReporterRepositoryCreationEventArgs(IReporterRepository repository)
        {
            this.m_repository = repository;
        }

        public IReporterRepository ReporterRepository
        {
            get { return this.m_repository; }
        }
    }
    public interface IRepositorySelector
    {
        IReporterRepository GetRepository(Assembly assembly);
        IReporterRepository GetRepository(string repositoryName);
        IReporterRepository CreateRepository(Assembly assembly, Type repositoryType);
        IReporterRepository CreateRepository(string repositoryName, Type repositoryType);
        bool ExistsRepository(string repositoryName);
        IReporterRepository[] GetAllRepositories();
        event ReporterRepositoryCreationEventHandler ReporterRepositoryCreationEvent;
    }
}
