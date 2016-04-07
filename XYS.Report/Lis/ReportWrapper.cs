using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYS.Report.Lis
{
    public abstract class ReportWrapper
    {
        private readonly IReporter m_reporter;
        protected ReportWrapper(IReporter reporter)
        {
            this.m_reporter = reporter;
        }
        public virtual IReporter Reporter
        {
            get { return this.m_reporter; }
        }
    }
}
