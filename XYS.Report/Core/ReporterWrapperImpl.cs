using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS.Lis.Core
{
    public abstract class ReporterWrapperImpl : IReporterWrapper
    {
        private readonly IReporter m_reporter;
        protected ReporterWrapperImpl(IReporter reporter)
        {
            this.m_reporter = reporter;
        }
        public virtual IReporter Reporter
        {
            get { return this.m_reporter; }
        }
    }
}
