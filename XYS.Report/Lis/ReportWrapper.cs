using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYS.Report.Lis
{
    public abstract class ReportWrapper
    {
        private readonly Reporter m_reporter;
        protected ReportWrapper(Reporter reporter)
        {
            this.m_reporter = reporter;
        }
        public virtual Reporter Reporter
        {
            get { return this.m_reporter; }
        }
    }
}
