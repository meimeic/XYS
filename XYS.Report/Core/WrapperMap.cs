using System;
using System.Collections;

using XYS.Lis.Repository;
namespace XYS.Lis.Core
{
    public delegate IReporterWrapper WrapperCreationHandler(IReporter reporter);
    public class WrapperMap
    {
        private readonly WrapperCreationHandler m_createWrapperHandler;
        private readonly Hashtable m_repositories = new Hashtable(3);
        public WrapperMap(WrapperCreationHandler createWrapperHandler)
        {
            this.m_createWrapperHandler = createWrapperHandler;
        }
        protected Hashtable Repositories
        {
            get { return this.m_repositories; }
        }
        public virtual IReporterWrapper GetWrapper(IReporter reporter)
        {
            if (reporter == null)
            {
                return null;
            }
            lock (this)
            {
                // Lookup hierarchy in map.
                Hashtable wrappersMap = (Hashtable)m_repositories[reporter.Repository];
                if (wrappersMap == null)
                {
                    // Hierarchy does not exist in map.
                    // Must register with hierarchy
                    wrappersMap = new Hashtable();
                    m_repositories[reporter.Repository] = wrappersMap;

                    // Register for config reset & shutdown on repository
                    // logger.Repository.ShutdownEvent += m_shutdownHandler;
                }
                // Look for the wrapper object in the map
                IReporterWrapper wrapperObject = wrappersMap[reporter] as IReporterWrapper;

                if (wrapperObject == null)
                {
                    // No wrapper object exists for the specified logger
                    // Create a new wrapper wrapping the logger
                    wrapperObject = CreateNewWrapperObject(reporter);
                    // Store wrapper logger in map
                    wrappersMap[reporter] = wrapperObject;
                }
                return wrapperObject;
            }
        }
        protected virtual IReporterWrapper CreateNewWrapperObject(IReporter reporter)
        {
            if (m_createWrapperHandler != null)
            {
                return m_createWrapperHandler(reporter);
            }
            return null;
        }
    }
}